using System;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class InspectorButton
    {
        public InspectorButtonMode Mode
            { get; set; } = InspectorButtonMode.AlwaysEnabled;
        public InspectorButtonSpacing Spacing
            { get; set; } = InspectorButtonSpacing.Before;
        public string Text
            { get; private set; }
        public Texture Icon
            { get; private set; }

        public event EventHandler<EventArgs> Click;

        public InspectorButton(string text)
        {
            Text = text;
        }

        public InspectorButton(Texture icon)
        {
            Icon = icon;
        }

        public void OnInspectorGUI()
        {
            var wasEnabled = GUI.enabled;
            GUI.enabled = Mode == InspectorButtonMode.AlwaysEnabled ||
                (EditorApplication.isPlaying ?
                    Mode == InspectorButtonMode.EnabledInPlayMode :
                    Mode == InspectorButtonMode.DisabledInPlayMode);

            if ((Spacing & InspectorButtonSpacing.Before) != 0) GUILayout.Space(10);

            if (Icon != null)
            {
                if (GUILayout.Button(Icon))
                    OnClick();
            }
            else
            {
                if (GUILayout.Button(Text))
                    OnClick();
            }

            if ((Spacing & InspectorButtonSpacing.After) != 0)  GUILayout.Space(10);
            GUI.enabled = wasEnabled;
        }

        protected virtual void OnClick()
            => Click?.Invoke(this, EventArgs.Empty);
    }
}
