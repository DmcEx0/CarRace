// MobileLiteOccluder — копия MobileLite + растворение/срез перекрывающих объектов.
// Идентичен CarRace/MobileLite, но во фрагменте вызывает OccluderFade:
//  * если объект между игроком и камерой -> растворяется дизером (Foundation off),
//    либо остаётся только основание/фундамент (Foundation on);
//  * детект чисто шейдерный, через глобали из OccluderFadeDriver.cs;
//  * остаётся Opaque, ноль alpha-blending, ноль multi_compile, SRP Batcher.
Shader "CarRace/MobileLiteOccluder"
{
    Properties
    {
        [MainTexture] _BaseMap   ("Base Map", 2D) = "white" {}
        [MainColor]   _BaseColor ("Base Color", Color) = (1,1,1,1)
        _AmbientBoost ("Ambient Boost", Range(0,1)) = 0.25

        [Header(Occluder Fade)]
        [ToggleUI] _FoundationMode  ("Foundation mode (keep base)", Float) = 0
        _KeepHeight     ("Keep Height (world units)",   Float) = 1.0
        _FoundationEdge ("Foundation Edge Softness",    Float) = 0.25
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
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "OccluderFade.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4  _BaseColor;
                half   _AmbientBoost;
                float  _FoundationMode;
                float  _KeepHeight;
                float  _FoundationEdge;
            CBUFFER_END

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv          : TEXCOORD0;
                half3  vlight      : TEXCOORD1;
                float4 positionNDC : TEXCOORD2; // экранная позиция (для зоны вокруг игрока)
                float2 occDW       : TEXCOORD3; // x = eye-depth, y = worldY
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs posIn = GetVertexPositionInputs(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);

                OUT.positionCS  = posIn.positionCS;
                OUT.uv          = TRANSFORM_TEX(IN.uv, _BaseMap);

                Light mainLight = GetMainLight();
                half  NdotL     = dot(normalWS, mainLight.direction);
                half  halfLam   = saturate(NdotL * 0.5h + 0.5h);
                OUT.vlight      = mainLight.color * halfLam + _AmbientBoost;

                OUT.positionNDC = posIn.positionNDC;
                OUT.occDW       = float2(-posIn.positionVS.z, posIn.positionWS.y);

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 screenUV01 = IN.positionNDC.xy / IN.positionNDC.w;
                CarRace_OccluderClip(IN.positionCS.xy, screenUV01, IN.occDW.x, IN.occDW.y,
                                     _FoundationMode, _KeepHeight, _FoundationEdge);

                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half3 rgb = tex.rgb * _BaseColor.rgb * IN.vlight;
                return half4(rgb, tex.a * _BaseColor.a);
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
                half   _AmbientBoost;
                float  _FoundationMode;
                float  _KeepHeight;
                float  _FoundationEdge;
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
