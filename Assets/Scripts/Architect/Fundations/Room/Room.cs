using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Architect
{
    [RequireComponent(typeof(FundationBuilder))]
    public class Room : MonoBehaviour
    {
        public RoomLayout Layout;

        public virtual void BuildRoom()
        {
            name = $"Room";

            var wallMarkers = new List<Vector2Int>();
            foreach (var wall in Layout.ShapeData)
            {
                var wallComponent = RoomWall.CreateWall(transform).Initialize(wall);
                wallMarkers.AddRange(wallComponent.WallMarkers.Select(marker => marker.Position));
            }
            RoomFloor.CreateFloor(transform).Initialize(
                new RoomFloorData(Layout.ShapeData.RoomSize) { WallsMarkers = wallMarkers });
        }
    }
}
