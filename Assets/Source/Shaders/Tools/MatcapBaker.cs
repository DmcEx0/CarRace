#if UNITY_EDITOR
using UnityEngine;

namespace CarRace.Shaders.Tools
{
    // Editor-only компонент. Сам класс лежит в runtime-сборке (так требует Unity
    // для AddComponent), но #if UNITY_EDITOR выкидывает его из WebGL-билда.
    // Соответственно сцена с этим компонентом — тоже editor-only, в Build
    // Settings её включать нельзя.
    [DisallowMultipleComponent]
    public sealed class MatcapBaker : MonoBehaviour
    {
        [Header("Render Target")]
        public Camera bakeCamera;

        [Range(64, 2048)]
        public int resolution = 512;

        [Tooltip("Anti-aliasing samples for the bake render texture. 8 даёт мягкие переходы.")]
        [Range(1, 8)]
        public int antiAliasing = 8;

        [Header("Sphere")]
        public MeshRenderer sphereRenderer;

        [Header("Output")]
        public string outputFolder = "Assets/Source/Shaders/Matcaps";
        public string fileName    = "Matcap_New";

        [Tooltip("Если включено — после Bake сразу применит импортные настройки (Clamp, без мипов, sRGB).")]
        public bool applyImportSettings = true;
    }
}
#endif
