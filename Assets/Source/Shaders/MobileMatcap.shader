// MobileMatcap — полностью unlit шейдер: ни одного обращения к рантайм-свету.
// Вся "освещённость" нарисована художником в matcap-текстуре, плюс дешёвая
// hemisphere-подсветка (sky/ground) и rim-fresnel. Идеально для мобильного
// WebGL: один проход, ноль multi_compile, без Lighting.hlsl.
//
// Цена кадра: ~2 texture samples + несколько MAD'ов. Легче, чем MobileLite,
// потому что выпилена вся light-arithmetic и подключаемые библиотеки.
Shader "CarRace/MobileMatcap"
{
    Properties
    {
        [MainTexture] _BaseMap     ("Base Map", 2D) = "white" {}
        [MainColor]   _BaseColor   ("Base Color", Color) = (1,1,1,1)

        [Header(MatCap)]
        [NoScaleOffset] _MatCap    ("MatCap (sphere)", 2D) = "gray" {}
        _MatCapColor    ("MatCap Tint",       Color)        = (1,1,1,1)
        _MatCapStrength ("MatCap Strength",   Range(0,2))   = 1.0

        [Header(Hemisphere Ambient)]
        _SkyColor      ("Sky Color (up)",     Color) = (0.62, 0.68, 0.72, 1)
        _GroundColor   ("Ground Color (down)",Color) = (0.12, 0.10, 0.08, 1)

        [Header(Rim)]
        _RimColor      ("Rim Color",          Color) = (0.55, 0.70, 0.50, 1)
        _RimPower      ("Rim Power",          Range(0.5,8)) = 3.0
        _RimIntensity  ("Rim Intensity",      Range(0,2))   = 0.5

        [Header(Mood)]
        _ColorGrade    ("Color Grade",        Color) = (0.85, 0.90, 0.78, 1)
        _Desaturation  ("Desaturation",       Range(0,1))   = 0.3
    }

    SubShader
    {
        Tags
        {
            "RenderType"      = "Opaque"
            "Queue"           = "Geometry"
            "RenderPipeline"  = "UniversalPipeline"
            "IgnoreProjector" = "True"
        }
        LOD 100

        Pass
        {
            Name "Unlit"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4  _BaseColor;
                half4  _MatCapColor;
                half4  _SkyColor;
                half4  _GroundColor;
                half4  _RimColor;
                half4  _ColorGrade;
                half   _MatCapStrength;
                half   _RimPower;
                half   _RimIntensity;
                half   _Desaturation;
            CBUFFER_END

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_MatCap);  SAMPLER(sampler_MatCap);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                half3  normalWS   : TEXCOORD1;
                half3  viewDirWS  : TEXCOORD2;
                half   rim        : TEXCOORD3;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs posIn = GetVertexPositionInputs(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 viewDirWS = normalize(GetCameraPositionWS() - posIn.positionWS);

                OUT.positionCS = posIn.positionCS;
                OUT.uv         = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS   = (half3)normalWS;
                OUT.viewDirWS  = (half3)viewDirWS;

                // Rim считается на вершине — на машинах с нормальной топологией
                // разницы с per-pixel почти не видно, а инструкций экономим много.
                OUT.rim = pow(saturate(1.0h - dot(normalWS, viewDirWS)), _RimPower);

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half3 nWS = normalize(IN.normalWS);
                half3 vWS = normalize(IN.viewDirWS);

                // Sphere map mapping (Blinn 1976) — корректно сворачивает заднюю
                // полусферу к центру matcap'а. Без этого на silhouette-краях
                // лезет тёмная кромка matcap-текстуры (визуальный "шов").
                // Множитель 0.46 (вместо 0.5) поджимает UV-диск до 92% от полного
                // радиуса — на гранях под скользящим углом перестаём попадать в
                // самые тёмные пиксели силуэта сферы. Константа сворачивается
                // компилятором — рантайм-стоимости ноль.
                half3 rWS = reflect(-vWS, nWS);
                half3 rVS = mul((half3x3)UNITY_MATRIX_V, rWS);
                rVS.z += 1.0h;
                half  invM = 0.46h * rsqrt(dot(rVS, rVS));
                half2 matcapUV = rVS.xy * invM + 0.5h;
                half3 matcap   = SAMPLE_TEXTURE2D(_MatCap, sampler_MatCap, matcapUV).rgb * _MatCapColor.rgb;

                // Hemisphere: верх -> SkyColor, низ -> GroundColor.
                half hemi = nWS.y * 0.5h + 0.5h;
                half3 hemiColor = lerp(_GroundColor.rgb, _SkyColor.rgb, hemi);

                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half3 baseCol = tex.rgb * _BaseColor.rgb * hemiColor;

                // Multiply-блендинг сохраняет читаемость текстуры,
                // strength=0 -> чисто hemisphere, =1 -> полный matcap.
                half3 col = baseCol * lerp(half3(1,1,1), matcap, _MatCapStrength);

                col += _RimColor.rgb * (IN.rim * _RimIntensity);

                half lum = dot(col, half3(0.299h, 0.587h, 0.114h));
                col = lerp(col, lum.xxx, _Desaturation);
                col *= _ColorGrade.rgb;

                return half4(col, tex.a * _BaseColor.a);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex   shadowVert
            #pragma fragment shadowFrag
            #pragma target   3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4  _BaseColor;
                half4  _MatCapColor;
                half4  _SkyColor;
                half4  _GroundColor;
                half4  _RimColor;
                half4  _ColorGrade;
                half   _MatCapStrength;
                half   _RimPower;
                half   _RimIntensity;
                half   _Desaturation;
            CBUFFER_END

            float3 _LightDirection;
            float3 _LightPosition;

            struct AttributesShadow
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct VaryingsShadow
            {
                float4 positionCS : SV_POSITION;
            };

            float4 GetShadowPositionHClip(AttributesShadow IN)
            {
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS   = TransformObjectToWorldNormal(IN.normalOS);

            #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                float3 lightDirectionWS = normalize(_LightPosition - positionWS);
            #else
                float3 lightDirectionWS = _LightDirection;
            #endif

                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

            #if UNITY_REVERSED_Z
                positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
            #else
                positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
            #endif
                return positionCS;
            }

            VaryingsShadow shadowVert (AttributesShadow IN)
            {
                VaryingsShadow OUT;
                OUT.positionCS = GetShadowPositionHClip(IN);
                return OUT;
            }

            half4 shadowFrag (VaryingsShadow IN) : SV_Target { return 0; }
            ENDHLSL
        }
    }

    FallBack Off
}
