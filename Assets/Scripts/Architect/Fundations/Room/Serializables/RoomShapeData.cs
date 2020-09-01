using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Architect
{
    [Serializable]
    public class RoomShapeData : IEnumerable<RoomWallData>
    {
        public Vector2Int RoomSize;

        [HideInInspector]
        public List<Vector2Int> Shape = new List<Vector2Int>();
        [HideInInspector]
        public List<RoomWallData> Walls = new List<RoomWallData>();

        #region IEnumerable

        public IEnumerator<RoomWallData> GetEnumerator()
        {
            foreach (var wall in Walls)
                yield return wall;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion
    }
}
