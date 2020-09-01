using System;
using Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorTools
{
    public class SaveAndReturnGUI : TooltipGUI
    {
        public string PreviousScenePath
            { get; private set; }

        public event EventHandler<EventArgs> SaveAndReturnButtonClick;
        public event EventHandler<EventArgs> DiscardChangesButtonClick;

        private bool _isQuitting;

        public SaveAndReturnGUI(string previousScenePath):base()
        {
            PreviousScenePath = previousScenePath;

            Position = new Vector2(20, 20);
            ButtonWidth = 120f;

            InspectorButton button;
            button = new InspectorButton("Save and Return") 
                { Spacing = InspectorButtonSpacing.None };
            button.Click  += OnSaveAndReturnButtonClick;
            Buttons.Add(button);

            button = new InspectorButton("Discard Changes") 
                { Spacing = InspectorButtonSpacing.None };
            button.Click += OnDiscardChangesButtonClick;
            Buttons.Add(button);
        }

        public override void OnSceneGUI(SceneView sceneView)
        {
            base.OnSceneGUI(sceneView);

            if (_isQuitting)
                GoToPreviousScene();
        }

        protected virtual void OnDiscardChangesButtonClick(object sender, EventArgs args)
        {
            if (EditorUtility.DisplayDialog("Discard Changes", "Do you want to discard the changes?", "Yes", "No"))
            {
                _isQuitting = true;
                DiscardChangesButtonClick?.Invoke(sender, args);
            }
        }

        protected virtual void OnSaveAndReturnButtonClick(object sender, EventArgs args)
        {
            _isQuitting = true;
            SaveAndReturnButtonClick?.Invoke(sender, args);
        }

        private void GoToPreviousScene()
        {
            if (!string.IsNullOrEmpty(PreviousScenePath))
                EditorSceneManager.OpenScene(PreviousScenePath);
            else
            {
                var scene = SceneManager.GetActiveScene();
                OnActiveSceneChanged(scene, scene);
            }
            _isQuitting = false;
        }
    }
}
