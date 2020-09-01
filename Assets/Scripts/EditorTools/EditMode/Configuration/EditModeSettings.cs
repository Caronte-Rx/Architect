#if UNITY_EDITOR
using Core;
using UnityEngine;

namespace EditorTools
{
    public abstract class EditModeSettings<TSettings> : BaseSingletonSettings<TSettings>, IEditModeSettings 
        where TSettings : ScriptableObject
    {
        public virtual GameObject EditModePrefab { get; set; }
    }
}
#endif
