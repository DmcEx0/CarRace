using UnityEditor;
using UnityEngine;

namespace CarRace
{
    [CustomEditor(typeof(PartTrack2))]
    public class PartTrackEditor : Editor
    {
        private SerializedProperty _typeRegistryProperty;
        private SerializedProperty _typeNameProperty;
        private SerializedProperty _includeMaskProperty;
        private SerializedProperty _excludeMaskProperty;

        private string _newTypeName = string.Empty;

        private void OnEnable()
        {
            _typeRegistryProperty = serializedObject.FindProperty("_typeRegistry");
            _typeNameProperty = serializedObject.FindProperty("_typeName");
            _includeMaskProperty = serializedObject.FindProperty("_includeMask");
            _excludeMaskProperty = serializedObject.FindProperty("_excludeMask");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_typeRegistryProperty);

            var registry = _typeRegistryProperty.objectReferenceValue as PartTrackTypeRegistry;

            if (registry == null)
            {
                EditorGUILayout.HelpBox("Assign PartTrackTypeRegistry first.", MessageType.Warning);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            DrawTypeSection(registry);
            EditorGUILayout.Space(8);
            DrawMasksSection(registry);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTypeSection(PartTrackTypeRegistry registry)
        {
            EditorGUILayout.LabelField("Part Track Type", EditorStyles.boldLabel);

            string[] typeNames = registry.GetTypeNames();

            if (typeNames.Length == 0)
            {
                EditorGUILayout.HelpBox("Registry is empty. Add first type below.", MessageType.Info);
            }
            else
            {
                int currentIndex = registry.IndexOf(_typeNameProperty.stringValue);
                int selectedIndex = Mathf.Max(0, currentIndex);

                selectedIndex = EditorGUILayout.Popup("Type", selectedIndex, typeNames);

                if (selectedIndex >= 0 && selectedIndex < typeNames.Length)
                {
                    _typeNameProperty.stringValue = typeNames[selectedIndex];
                }
            }

            EditorGUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            _newTypeName = EditorGUILayout.TextField("New Type", _newTypeName);

            GUI.enabled = string.IsNullOrWhiteSpace(_newTypeName) == false;
            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                AddTypeToRegistry(registry, _newTypeName);
            }

            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMasksSection(PartTrackTypeRegistry registry)
        {
            EditorGUILayout.LabelField("Connections", EditorStyles.boldLabel);

            string[] typeNames = registry.GetTypeNames();

            if (typeNames.Length == 0)
            {
                EditorGUILayout.HelpBox("No types available in registry.", MessageType.Info);
                return;
            }

            _includeMaskProperty.intValue =
                EditorGUILayout.MaskField("Include", _includeMaskProperty.intValue, typeNames);
            _excludeMaskProperty.intValue =
                EditorGUILayout.MaskField("Exclude", _excludeMaskProperty.intValue, typeNames);
        }

        private void AddTypeToRegistry(PartTrackTypeRegistry registry, string newTypeName)
        {
            newTypeName = newTypeName.Trim();

            if (string.IsNullOrWhiteSpace(newTypeName))
                return;

            Undo.RecordObject(registry, "Add PartTrack Type");

            bool added = registry.TryAddType(newTypeName);

            if (added == false)
            {
                EditorUtility.DisplayDialog("Type exists", $"Type '{newTypeName}' already exists.", "OK");
                return;
            }

            EditorUtility.SetDirty(registry);

            _typeNameProperty.stringValue = newTypeName;
            _newTypeName = string.Empty;

            AssetDatabase.SaveAssets();
        }
    }
}