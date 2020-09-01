using Core;
using UnityEngine;

namespace Architect
{
    [CreateAssetMenu(
        fileName = "ArchitectSettings", 
        menuName = "Architect/Settings/Architect Settings", order = 0)]
    public class ArchitectSettings : BaseSingletonSettings<ArchitectSettings>
    {
        [Header("Room Prefabs")]

        [DefaultPrefab(typeof(Room))]
        [Label("Room")]
        public Room RoomPrefab;

        [DefaultPrefab(typeof(RoomCell))]
        [Label("Cell")]
        public RoomCell CellPrefab;

        [DefaultPrefab(typeof(RoomConnection))]
        [Label("Connection")]
        public RoomConnection ConnectionPrefab;

        [DefaultPrefab(typeof(RoomEdge))]
        [Label("Edge")]
        public RoomEdge EdgePrefab;

        [DefaultPrefab(typeof(RoomFloor))]
        [Label("Floor")]
        public RoomFloor FloorPrefab;

        [DefaultPrefab(typeof(RoomWall))]
        [Label("Wall")]
        public RoomWall WallPrefab;
    }
}
