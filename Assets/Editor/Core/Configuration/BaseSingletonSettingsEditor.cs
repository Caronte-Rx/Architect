using UnityEditor;

namespace Core
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BaseSingletonSettings<>), true)]
    public class BaseSingletonSettingsEditor : IBaseSettingsEditor { }
}