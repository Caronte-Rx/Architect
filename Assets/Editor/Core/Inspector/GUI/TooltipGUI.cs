using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorTools
{
    public class TooltipGUI : IEnumerable<InspectorButton>
    {
        public List<InspectorButton> Buttons 
            { get; set; }  = new List<InspectorButton>();
        public Vector2 Position
            { get; set; } = Vector2.zero;
        public float ButtonWidth
            { get; set; } = 100f;

        public TooltipGUI()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            EditorSceneManager.activeSceneChangedInEditMode += OnActiveSceneChanged;
        }

        public virtual void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();
            {
                GUILayout.BeginArea(new Rect(Position, new Vector2(ButtonWidth, Buttons.Count * 100)));
                {
                    EditorGUILayout.BeginVertical();
                    {
                        foreach (var button in Buttons)
                            button.OnInspectorGUI();
                    }
                    EditorGUILayout.EndVertical();
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }

        public void Add(InspectorButton button)
        {
            Buttons.Add(button);
        }

        protected void OnActiveSceneChanged(Scene fromScene, Scene toScene)
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            EditorSceneManager.activeSceneChangedInEditMode -= OnActiveSceneChanged;
        }

        #region IEnumerable
        public IEnumerator<InspectorButton> GetEnumerator()
        {
            foreach (var button in Buttons)
                yield return button;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion
    }
}
