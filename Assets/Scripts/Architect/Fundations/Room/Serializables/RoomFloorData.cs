using System;
using System.Collections.Generic;
using UnityEngine;

namespace Architect
{
    [Serializable]
    public class RoomFloorData
    {
        public Vector2Int Size;

        [HideInInspector]
        public List<Vector2Int> WallsMarkers = new List<Vector2Int>();

        public RoomFloorData(Vector2Int size)
        {
            Size = size;
        }

        public override string ToString() =>
            $"{{{nameof(Size)}:{Size}}}";
    }
}
