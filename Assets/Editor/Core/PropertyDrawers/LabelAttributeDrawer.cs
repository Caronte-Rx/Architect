using UnityEditor;
using UnityEngine;

namespace Core
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var labelAttribute = attribute as LabelAttribute;
            EditorGUI.PropertyField(position, property, new GUIContent(labelAttribute.Label));
        }
    }
}