using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CarRace.Shaders.Tools
{
    [CustomEditor(typeof(MatcapBaker))]
    public sealed class MatcapBakerEditor : Editor
    {
        private const string SceneSavePath    = "Assets/Source/Shaders/Tools/MatcapBakerScene.unity";
        private const string SphereMaterialPath = "Assets/Source/Shaders/Tools/MatcapBakeSphere.mat";

        public override void OnInspectorGUI()
        {
            var baker = (MatcapBaker)target;

            DrawInstructions();
            EditorGUILayout.Space(6);

            DrawDefaultInspector();
            EditorGUILayout.Space(10);

            using (new EditorGUI.DisabledScope(baker.bakeCamera == null))
            {
                var prevColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.55f, 0.85f, 0.55f);
                if (GUILayout.Button("BAKE  MatCap  →  PNG", GUILayout.Height(46)))
                {
                    BakeToPng(baker);
                }
                GUI.backgroundColor = prevColor;
            }

            if (baker.bakeCamera == null)
            {
                EditorGUILayout.HelpBox("Поле 'Bake Camera' не заполнено. Перетащи объект BakeCamera из иерархии.", MessageType.Warning);
            }
        }

        private static void DrawInstructions()
        {
            EditorGUILayout.HelpBox(
                "КАК ЗАПЕКАТЬ MATCAP:\n" +
                "1. Открой окно Game View (Window → General → Game) — там видишь, что попадёт в matcap.\n" +
                "2. В иерархии выдели KeyLight / FillLight / RimLight и крути их вращение / цвет / интенсивность,\n" +
                "   пока сфера в Game View не станет выглядеть как тебе нужно.\n" +
                "3. (опц.) Выдели BakeSphere → поменяй материал MatcapBakeSphere.mat (URP/Lit) — цвет / metallic / smoothness.\n" +
                "4. Здесь, ниже, поставь Resolution и File Name.\n" +
                "5. Жми зелёную кнопку BAKE. PNG сохранится в Assets/Source/Shaders/Matcaps/ и будет подсвечен.",
                MessageType.Info);
        }

        [MenuItem("Tools/CarRace/Open MatCap Baker Scene")]
        public static void OpenOrCreateBakerScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            if (File.Exists(SceneSavePath))
            {
                EditorSceneManager.OpenScene(SceneSavePath, OpenSceneMode.Single);
                FocusBakerAndShowGameView();
                return;
            }

            CreateFreshBakerScene();
        }

        private static void CreateFreshBakerScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // --- Camera (orthographic, frames a unit sphere) ---
            var camGO = new GameObject("BakeCamera");
            camGO.transform.position = new Vector3(0f, 0f, -5f);
            var cam = camGO.AddComponent<Camera>();
            cam.clearFlags       = CameraClearFlags.SolidColor;
            cam.backgroundColor  = Color.black;
            cam.orthographic     = true;
            cam.orthographicSize = 0.505f;
            cam.nearClipPlane    = 0.1f;
            cam.farClipPlane     = 50f;
            cam.allowHDR         = false;
            cam.allowMSAA        = true;

            // --- Sphere with URP/Lit material (built-in Default-Material показывает магенту в URP) ---
            var sphereGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereGO.name = "BakeSphere";
            sphereGO.transform.position = Vector3.zero;
            sphereGO.transform.localScale = Vector3.one;
            if (sphereGO.TryGetComponent<Collider>(out var col)) Object.DestroyImmediate(col);

            var sphereRenderer = sphereGO.GetComponent<MeshRenderer>();
            sphereRenderer.sharedMaterial = GetOrCreateSphereMaterial();

            // --- Lights (Key / Fill / Rim) ---
            CreateLight("KeyLight",  new Vector3(50f, -30f, 0f),  new Color(1.00f, 0.96f, 0.88f), 1.00f, LightShadows.Soft);
            CreateLight("FillLight", new Vector3(15f, 150f, 0f),  new Color(0.55f, 0.65f, 0.80f), 0.35f, LightShadows.None);
            CreateLight("RimLight",  new Vector3(-25f, -160f, 0f), new Color(0.85f, 0.95f, 0.75f), 0.60f, LightShadows.None);

            // --- Baker GameObject ---
            var bakerGO = new GameObject("MatcapBaker");
            var baker = bakerGO.AddComponent<MatcapBaker>();
            baker.bakeCamera     = cam;
            baker.sphereRenderer = sphereRenderer;

            Directory.CreateDirectory(Path.GetDirectoryName(SceneSavePath)!);
            EditorSceneManager.SaveScene(scene, SceneSavePath);

            FocusBakerAndShowGameView();
            Debug.Log($"[MatcapBaker] Scene created at {SceneSavePath}");
        }

        private static Material GetOrCreateSphereMaterial()
        {
            var existing = AssetDatabase.LoadAssetAtPath<Material>(SphereMaterialPath);
            if (existing != null) return existing;

            var urpLit = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLit == null)
            {
                Debug.LogWarning("[MatcapBaker] URP/Lit shader not found — falling back to Standard.");
                urpLit = Shader.Find("Standard");
            }

            var mat = new Material(urpLit) { name = "MatcapBakeSphere" };
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", Color.white);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.5f);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0.0f);

            Directory.CreateDirectory(Path.GetDirectoryName(SphereMaterialPath)!);
            AssetDatabase.CreateAsset(mat, SphereMaterialPath);
            AssetDatabase.SaveAssets();
            return mat;
        }

        private static void FocusBakerAndShowGameView()
        {
            var bakerComp = Object.FindFirstObjectByType<MatcapBaker>();
            if (bakerComp != null)
            {
                Selection.activeGameObject = bakerComp.gameObject;
                SceneView.lastActiveSceneView?.FrameSelected();
            }
            EditorApplication.ExecuteMenuItem("Window/General/Game");
            EditorApplication.ExecuteMenuItem("Window/General/Inspector");
        }

        private static void CreateLight(string name, Vector3 eulerAngles, Color color, float intensity, LightShadows shadows)
        {
            var go = new GameObject(name);
            go.transform.rotation = Quaternion.Euler(eulerAngles);
            var light = go.AddComponent<Light>();
            light.type      = LightType.Directional;
            light.color     = color;
            light.intensity = intensity;
            light.shadows   = shadows;
        }

        private static void BakeToPng(MatcapBaker baker)
        {
            int res = Mathf.Max(64, baker.resolution);
            int aa  = Mathf.Clamp(baker.antiAliasing, 1, 8);
            if (aa != 1 && aa != 2 && aa != 4 && aa != 8) aa = 8;

            var desc = new RenderTextureDescriptor(res, res, RenderTextureFormat.ARGB32, 24)
            {
                msaaSamples = aa,
                sRGB = true,
                useMipMap = false,
                autoGenerateMips = false,
            };
            var rt = RenderTexture.GetTemporary(desc);

            var prevTarget = baker.bakeCamera.targetTexture;
            var prevActive = RenderTexture.active;

            baker.bakeCamera.targetTexture = rt;
            baker.bakeCamera.Render();
            RenderTexture.active = rt;

            var tex = new Texture2D(res, res, TextureFormat.RGB24, false, false);
            tex.ReadPixels(new Rect(0, 0, res, res), 0, 0);
            tex.Apply(false, false);

            baker.bakeCamera.targetTexture = prevTarget;
            RenderTexture.active = prevActive;
            RenderTexture.ReleaseTemporary(rt);

            byte[] png = tex.EncodeToPNG();
            Object.DestroyImmediate(tex);

            Directory.CreateDirectory(baker.outputFolder);
            string safeName = string.IsNullOrWhiteSpace(baker.fileName) ? "Matcap_New" : baker.fileName;
            string fullPath = Path.Combine(baker.outputFolder, safeName + ".png").Replace('\\', '/');
            File.WriteAllBytes(fullPath, png);

            AssetDatabase.Refresh();

            if (baker.applyImportSettings)
            {
                var importer = AssetImporter.GetAtPath(fullPath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType    = TextureImporterType.Default;
                    importer.wrapMode       = TextureWrapMode.Clamp;
                    importer.filterMode     = FilterMode.Bilinear;
                    importer.mipmapEnabled  = false;
                    importer.alphaSource    = TextureImporterAlphaSource.None;
                    importer.sRGBTexture    = true;
                    importer.maxTextureSize = Mathf.Max(res, 256);
                    importer.SaveAndReimport();
                }
            }

            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath);
            EditorGUIUtility.PingObject(asset);
            Debug.Log($"[MatcapBaker] Saved: {fullPath}", asset);
        }
    }
}
