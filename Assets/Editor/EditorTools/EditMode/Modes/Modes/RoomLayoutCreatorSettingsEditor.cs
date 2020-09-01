using System;
using Core;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RoomLayoutCreatorSettings))]
    public class RoomLayoutCreatorSettingsEditor : BaseSettingsEditor
    {
        public override void OnResetSettings(object sender, EventArgs args)
        {
            Properties["CornerColor"]
                .colorValue = new Color32( 97, 179, 231, 255);
            Properties["CornerFocusedColor"]
                .colorValue = new Color32(255,   0,   0, 255);
            Properties["WallColor"]
                .colorValue = new Color32(142, 142, 142, 255);
            Properties["WallFocusedColor"]
                .colorValue = new Color32(255, 255, 255, 255);
            Properties["LineWeight"]
                .floatValue = 0.4f;
            base.OnResetSettings(sender, args);
        }
    } 
}