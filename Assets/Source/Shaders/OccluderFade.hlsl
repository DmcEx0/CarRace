#ifndef CARRACE_OCCLUDER_FADE_INCLUDED
#define CARRACE_OCCLUDER_FADE_INCLUDED

// =============================================================================
//  OccluderFade — растворение / срез объектов, перекрывающих игрока.
// -----------------------------------------------------------------------------
//  Камера фиксирована сзади-сверху (изометрия). Если объект оказался МЕЖДУ
//  игроком и камерой, его надо либо растворить целиком (dither-fade), либо
//  оставить только основание (foundation / "фундамент").
//
//  Детект — чисто шейдерный, через две глобали, которые раз в кадр выставляет
//  OccluderFadeDriver.cs. Никакой per-object логики и "щёлканья" Renderer.enabled.
//
//  Стоимость на фрагмент: ~десяток ALU + один clip. Никакого alpha-blending,
//  объект остаётся в Opaque-очереди -> нет overdraw-штрафа, SRP Batcher живёт.
// =============================================================================

// Глобали (Shader.SetGlobalVector). НЕ в CBUFFER(UnityPerMaterial) — они
// глобальные, а не per-material, поэтому SRP Batcher не ломается.
float4 _OccluderPlayer;  // xy = экранная позиция игрока (0..1, конвенция WorldToScreenPoint/Screen),
                         // z  = eye-depth игрока (мировые ед., из WorldToScreenPoint.z),
                         // w  = глобальный вкл/выкл (0/1)
float4 _OccluderParams;  // x = радиус зоны (доля высоты экрана), y = мягкость края (доля),
                         // z = depth bias (мировые ед.), w = не используется

// Interleaved Gradient Noise (Jimenez 2014) — дешёвый упорядоченный дизер
// без текстуры, одна цепочка frac. На вход — оконные пиксели (SV_POSITION.xy).
float CarRace_IGN(float2 px)
{
    return frac(52.9829189 * frac(dot(px, float2(0.06711056, 0.00583715))));
}

// Возвращает hide 0..1: 0 = полностью видим, 1 = полностью скрыт.
//   screenUV01   — экранные UV фрагмента (0..1, Unity-конвенция, снизу-слева)
//   eyeDepth     — положительная глубина фрагмента (-positionVS.z)
//   worldY       — мировая Y фрагмента
//   foundationMode — 0 = растворять весь объект, 1 = оставлять основание
//   keepHeight   — высота над основанием объекта, которую сохраняем (мир. ед.)
//   foundationEdge — мягкость горизонтального среза (мир. ед.)
float CarRace_OccluderHide(float2 screenUV01, float eyeDepth, float worldY,
                           float foundationMode, float keepHeight, float foundationEdge)
{
    // Глобальный рубильник: эффект выключен -> ничего не считаем.
    if (_OccluderPlayer.w < 0.5)
        return 0.0;

    // 1) Насколько фрагмент близок к игроку на экране (круг с поправкой на аспект).
    float2 d = screenUV01 - _OccluderPlayer.xy;
    d.x *= _ScreenParams.x / _ScreenParams.y;          // делаем зону круглой, а не овальной
    float dist = length(d);
    float prox = 1.0 - smoothstep(_OccluderParams.x - _OccluderParams.y, _OccluderParams.x, dist);

    // 2) Фрагмент ближе к камере, чем игрок -> он реально перекрывает игрока.
    float inFront = step(eyeDepth, _OccluderPlayer.z - _OccluderParams.z);

    float occ = prox * inFront;

    // 3) Режим "только фундамент": прячем лишь часть выше keepHeight над
    //    основанием объекта. Основание берём из позиции пивота объекта
    //    (предполагается, что пивот строения стоит на земле).
    float objectBaseY  = GetObjectToWorldMatrix()._m13; // мировая Y пивота
    float heightAbove  = worldY - objectBaseY;
    float aboveKeep    = saturate((heightAbove - keepHeight) / max(foundationEdge, 1e-3));
    float hideMask     = lerp(1.0, aboveKeep, foundationMode); // mode 0 -> весь объект; mode 1 -> только верх

    return occ * hideMask;
}

// Удобная обёртка: считает hide и выбивает пиксели через дизер.
// Вызывать в самом начале frag.
//   windowPx — IN.positionCS.xy фрагментного шейдера (SV_POSITION, оконные пиксели)
void CarRace_OccluderClip(float2 windowPx, float2 screenUV01, float eyeDepth, float worldY,
                          float foundationMode, float keepHeight, float foundationEdge)
{
    float hide = CarRace_OccluderHide(screenUV01, eyeDepth, worldY,
                                      foundationMode, keepHeight, foundationEdge);
    // hide=0 -> clip(dither) >= 0 (ничего не режем); hide=1 -> почти всё уходит в минус.
    clip(CarRace_IGN(windowPx) - hide);
}

#endif // CARRACE_OCCLUDER_FADE_INCLUDED
