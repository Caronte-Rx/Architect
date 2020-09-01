#if UNITY_EDITOR
using Core;
using UnityEngine;

namespace EditorTools
{
    [SingletonSettings(typeof(RoomLayoutCreatorSettings))]
    [RequireComponent(typeof(Grid), typeof(Transform))]
    public class RoomLayoutCreator : BaseEditMode
    {
        public override BaseEditMode Initialize(ScriptableObject roomLayout, string previousScenePath = "")
        {
            base.Initialize(roomLayout, previousScenePath);
            gameObject.HideComponentInInspector<Grid>();
            gameObject.HideComponentInInspector<Transform>();
            return this;
        }
    }
}
#endif
