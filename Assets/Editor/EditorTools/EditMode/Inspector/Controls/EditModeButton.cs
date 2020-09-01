using System;
using Core;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorTools
{
    public class EditModeButton<TEditMode> : InspectorButton where TEditMode : IBaseEditMode 
    {
        public ScriptableObject EditModeTarget
            { get; set; }

        public EditModeButton(ScriptableObject editModeTarget)
            : this(editModeTarget, "Edit Mode") { }

        public EditModeButton(ScriptableObject editModeTarget, string label)
            : base(label)
        {
            EditModeTarget = editModeTarget;
        }

        protected override void OnClick()
        {
            string scenePath = SceneManager.GetActiveScene().path;
            try
            {
                Scene editModeScene;
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    editModeScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    editModeScene.name = $"{EditModeTarget.name} [Edit Mode]";
                    BaseEditMode.InstantiateEditMode<TEditMode>(EditModeTarget, scenePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to open '{typeof(TEditMode)}' edit mode. " +
                    $"Details: {ex.Message}.");
                if (scenePath != SceneManager.GetActiveScene().path)
                    EditorSceneManager.OpenScene(scenePath);
            }
            GUIUtility.ExitGUI();
        }
    }
}
