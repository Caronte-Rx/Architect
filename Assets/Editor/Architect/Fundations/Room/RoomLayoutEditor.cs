using EditorTools;
using UnityEditor;

namespace Architect
{
    [CustomEditor(typeof(RoomLayout))]
    public class RoomLayoutEditor : Editor
    {
        private EditModeButton<RoomLayoutCreator> _editModeButton;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _editModeButton.OnInspectorGUI();
        }

        public void OnEnable()
        {
            _editModeButton =
                new EditModeButton<RoomLayoutCreator>(target as RoomLayout);
        }
    }
}
