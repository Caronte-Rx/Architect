using System;
using System.Collections.Generic;
using UnityEditor;

namespace Core
{
    public class IBaseSettingsEditor : Editor
    {
        protected Dictionary<string, SerializedProperty> Properties
            { get; set; }

        private InspectorButton _resetButton;

        public void OnEnable()
        {
            var DEFAULT_PROPERTIES = new List<string>() { "m_Script", "_editor" };
            Properties = new Dictionary<string, SerializedProperty>();

            var property = serializedObject.GetIterator();
            if (property.NextVisible(true))
            {
                do
                {
                    if (!DEFAULT_PROPERTIES.Contains(property.name))
                        Properties.Add(property.name, serializedObject.FindProperty(property.name));
                }
                while (property.NextVisible(false));
            }

            _resetButton = new InspectorButton("Reset Settings");
            _resetButton.Click += OnResetSettings;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _resetButton.OnInspectorGUI();
        }

        public virtual void OnResetSettings(object sender, EventArgs args)
        {
            foreach (var field in AttributeUtility.GetFields<DefaultPrefabAttribute>(target.GetType()))
                serializedObject.FindProperty(field.Name).objectReferenceValue = DefaultPrefabAttribute.GetPrefab(field);
            serializedObject.ApplyModifiedProperties();
        }
    }
}