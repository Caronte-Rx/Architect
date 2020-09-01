#if UNITY_EDITOR 
using Core;
using UnityEngine;

namespace EditorTools
{
    [CreateAssetMenu(
        fileName = "RoomLayoutCreatorSettings", 
        menuName = "Architect/Settings/Edit Mode/Room Creator Settings", order = 0)]
    public class RoomLayoutCreatorSettings : EditModeSettings<RoomLayoutCreatorSettings>
    {
        [Header("Colors")]

        [Label("Corner")]
        public Color CornerColor            = new Color32( 97, 179, 231, 255);
        [Label("Corner Focused")]
        public Color CornerFocusedColor     = new Color32(255,   0,   0, 255);
        [Label("Wall")]
        public Color WallColor              = new Color32(142, 142, 142, 255);
        [Label("Wall Focused")]
        public Color WallFocusedColor       = new Color32(255, 255, 255, 255);

        [Header("Prefabs")]

        [DefaultPrefab(typeof(GameObject), "RoomLayoutCreator")]
        [Label("Edit Mode")]
        public GameObject editModePrefab;

        [Header("Other")]

        [Range(0.1f, 1f)]
        public float LineWeight = 0.4f;

        public override GameObject EditModePrefab
        {
            get => editModePrefab;
            set => editModePrefab = value;
        }
    }
}
#endif
