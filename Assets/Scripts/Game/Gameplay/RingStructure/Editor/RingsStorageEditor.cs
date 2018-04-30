using Game.Gameplay.RingStructure.Configuration;
using UnityEditor;
using UnityEngine;

namespace Game.Gameplay.RingStructure.Editor
{
    [CustomEditor(typeof(RingsStorage))]
    public class RingsStorageEditor : UnityEditor.Editor
    {
        private RingsStorage _storage;
        private GUIStyle _headerStyle;

        private void OnEnable()
        {
            _storage = target as RingsStorage;
            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.black},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Rings Prefabs", _headerStyle);
            var list = serializedObject.FindProperty("RingsPrefabs");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                var count = list.arraySize;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = list.GetArrayElementAtIndex(i);
                        var elementName = element.objectReferenceValue != null
                            ? element.objectReferenceValue.name
                            : "None";

                        EditorGUILayout.PropertyField(element, new GUIContent(elementName));
                        if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                        {
                            RecordObject();
                            _storage.RingsPrefabs.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Add"))
            {
                RecordObject();
                _storage.RingsPrefabs.Add(null);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_storage);
            }
        }

        private void RecordObject(string changeDescription = "Rings Storage Change")
        {
            Undo.RecordObject(serializedObject.targetObject, changeDescription);
        }
    }
}