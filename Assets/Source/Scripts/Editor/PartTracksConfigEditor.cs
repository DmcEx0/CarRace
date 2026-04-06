using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarRace
{
    [CustomEditor(typeof(PartTracksConfig))]
    public class PartTracksConfigEditor : Editor
    {
        private SerializedProperty _partTracksData;

        private void OnEnable()
        {
            _partTracksData = serializedObject.FindProperty("_partTracksData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawListToolbar();

            EditorGUILayout.Space();

            List<string> existingTypes = CollectTypes();

            for (int i = 0; i < _partTracksData.arraySize; i++)
            {
                SerializedProperty element = _partTracksData.GetArrayElementAtIndex(i);

                SerializedProperty prefabProp = element.FindPropertyRelative("_prefab");
                SerializedProperty typeProp = element.FindPropertyRelative("_type");
                SerializedProperty useIncludeProp = element.FindPropertyRelative("_useInclude");
                SerializedProperty includeProp = element.FindPropertyRelative("_include");
                SerializedProperty useExcludeProp = element.FindPropertyRelative("_useExclude");
                SerializedProperty excludeProp = element.FindPropertyRelative("_exclude");

                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField($"Element {i}", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(prefabProp);
                EditorGUILayout.PropertyField(typeProp);

                EditorGUILayout.PropertyField(useIncludeProp);
                if (useIncludeProp.boolValue)
                {
                    DrawStringPopup(includeProp, "Include", existingTypes, typeProp.stringValue);
                }

                EditorGUILayout.PropertyField(useExcludeProp);
                if (useExcludeProp.boolValue)
                {
                    DrawStringPopup(excludeProp, "Exclude", existingTypes, typeProp.stringValue);
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    _partTracksData.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawListToolbar()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add"))
            {
                _partTracksData.InsertArrayElementAtIndex(_partTracksData.arraySize);
            }

            EditorGUILayout.EndHorizontal();
        }

        private List<string> CollectTypes()
        {
            var result = new List<string>();

            for (int i = 0; i < _partTracksData.arraySize; i++)
            {
                SerializedProperty element = _partTracksData.GetArrayElementAtIndex(i);
                SerializedProperty typeProp = element.FindPropertyRelative("_type");

                string value = typeProp.stringValue?.Trim();

                if (string.IsNullOrEmpty(value))
                    continue;

                if (!result.Contains(value))
                    result.Add(value);
            }

            return result;
        }

        private void DrawStringPopup(SerializedProperty property, string label, List<string> options,
            string currentType)
        {
            List<string> filtered = new List<string>();

            for (int i = 0; i < options.Count; i++)
            {
                if (options[i] == currentType)
                    continue; // опционально: нельзя ссылаться на самого себя

                filtered.Add(options[i]);
            }

            if (filtered.Count == 0)
            {
                EditorGUILayout.PropertyField(property, new GUIContent(label));
                EditorGUILayout.HelpBox("Нет доступных Type для выбора.", MessageType.Info);
                return;
            }

            int currentIndex = Mathf.Max(0, filtered.IndexOf(property.stringValue));
            int newIndex = EditorGUILayout.Popup(label, currentIndex, filtered.ToArray());
            property.stringValue = filtered[newIndex];
        }
    }
}