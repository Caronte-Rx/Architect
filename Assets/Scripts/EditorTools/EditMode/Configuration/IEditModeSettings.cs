#if UNITY_EDITOR
using UnityEngine;

namespace EditorTools
{
    public interface IEditModeSettings
    {
        GameObject EditModePrefab { get; set; }
    }
}
#endif
