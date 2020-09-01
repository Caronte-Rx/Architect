#if UNITY_EDITOR
using Core;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    public abstract class BaseEditMode : MonoBehaviour, IBaseEditMode
    {
        public ScriptableObject EditModeTarget
            { get; set; }

        public string PreviousScenePath
            { get; set; }

        public virtual BaseEditMode Initialize(ScriptableObject editModeTarget, string previousScenePath = "")
        {
            EditModeTarget      = editModeTarget; 
            PreviousScenePath   = previousScenePath;
            return this;
        }

        public static void InstantiateEditMode<TEditMode>(ScriptableObject editModeTarget, string previousScenePath)
            where TEditMode : IBaseEditMode
        {
            var editModeSettings =
                (IEditModeSettings)SingletonSettingsAttribute.GetSingletonSettings<TEditMode>();
            var editModeObject =
                Instantiate(editModeSettings.EditModePrefab);
            editModeObject.name = "EditModeHandler";

            ((IBaseEditMode)editModeObject.AddComponent(typeof(TEditMode)))
                .Initialize(editModeTarget, previousScenePath);
            Selection.activeGameObject = editModeObject;
        }
    }
}
#endif