using UnityEngine;

namespace CarRace.Infrastructure.Rendering
{
    /// <summary>
    /// Раз в кадр кладёт позицию игрока в глобальные шейдер-параметры, которые
    /// читают шейдеры CarRace/*Occluder. Сами шейдеры решают, растворять ли
    /// объект (или оставлять основание), если он оказался между игроком и камерой.
    ///
    /// Повесь на любой постоянный объект сцены (например, на камеру), укажи
    /// Player и Camera. Никакой per-object логики не нужно — достаточно, чтобы
    /// перекрывающие объекты использовали материал на *Occluder-шейдере.
    /// </summary>
    [DefaultExecutionOrder(10000)] // после движения камеры/игрока в этом кадре
    public class OccluderFadeDriver : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform _player;
        [SerializeField] private Camera _camera;

        [Header("Zone")]
        [Tooltip("Точка на игроке, вокруг которой центрируется зона (обычно центр тела).")]
        [SerializeField] private Vector3 _playerWorldOffset = new Vector3(0f, 1f, 0f);

        [Tooltip("Радиус зоны растворения в долях высоты экрана (0..1).")]
        [Range(0f, 1f)]
        [SerializeField] private float _radiusScreenFraction = 0.12f;

        [Tooltip("Мягкость края зоны в долях высоты экрана.")]
        [Range(0f, 0.5f)]
        [SerializeField] private float _softnessFraction = 0.05f;

        [Tooltip("Запас по глубине (мир. ед.): объекты ближе игрока минус этот запас считаются перекрывающими.")]
        [SerializeField] private float _depthBias = 0.5f;

        private static readonly int PlayerId = Shader.PropertyToID("_OccluderPlayer");
        private static readonly int ParamsId = Shader.PropertyToID("_OccluderParams");

        private void Reset()
        {
            _camera = Camera.main;
        }

        private void OnDisable()
        {
            Shader.SetGlobalVector(PlayerId, new Vector4(0f, 0f, 0f, 0f));
        }

        private void LateUpdate()
        {
            var cam = _camera != null ? _camera : Camera.main;
            if (cam == null || _player == null)
            {
                Shader.SetGlobalVector(PlayerId, Vector4.zero);
                return;
            }

            Vector3 sp = cam.WorldToScreenPoint(_player.position + _playerWorldOffset);
            
            float enable = sp.z > 0f ? 1f : 0f;
            float uvx = sp.x / cam.pixelWidth;
            float uvy = sp.y / cam.pixelHeight;

            Shader.SetGlobalVector(PlayerId, new Vector4(uvx, uvy, sp.z, enable));
            Shader.SetGlobalVector(ParamsId, new Vector4(_radiusScreenFraction, _softnessFraction, _depthBias, 0f));
        }
    }
}
