using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(GameController))]
    public class GameControllerEditor : UnityEditor.Editor
    {
        private GameController _gameController;

        private void OnEnable()
        {
            _gameController = target as GameController;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                if (GUILayout.Button("Play") && EditorApplication.isPlaying)
                {
                    _gameController.Play();
                }

                if (GUILayout.Button("Stop") && EditorApplication.isPlaying)
                {
                    _gameController.Stop();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}