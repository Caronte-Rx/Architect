using UnityEditor;

namespace Core
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BaseSettings), true)]
    public class BaseSettingsEditor : IBaseSettingsEditor { }
}