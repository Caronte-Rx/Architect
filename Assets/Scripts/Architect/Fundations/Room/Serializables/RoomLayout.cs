using UnityEngine;

namespace Architect
{ 
    [CreateAssetMenu(
        fileName = "RoomLayout",
        menuName = "Architect/Fundation/Room/Room Layout", order = 0)]
    public class RoomLayout : ScriptableObject
    {
        public RoomShapeData ShapeData;
    }
}
