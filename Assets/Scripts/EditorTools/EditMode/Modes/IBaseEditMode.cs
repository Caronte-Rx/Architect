#if UNITY_EDITOR
using UnityEngine;

namespace EditorTools
{ 
    public interface IBaseEditMode
    {
        ScriptableObject EditModeTarget
            { get; set; }
        string PreviousScenePath
            { get; set; }

        BaseEditMode Initialize(ScriptableObject editModeTarget, string previousScenePath = "");
    }
}
#endif
