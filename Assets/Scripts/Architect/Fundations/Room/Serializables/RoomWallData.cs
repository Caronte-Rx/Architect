using System;
using Core;
using UnityEngine;

namespace Architect
{
    [Serializable]
    public class RoomWallData
    {
        public bool AllowConnection;
        public Vector2Int CornerA;
        public Vector2Int CornerB;
        public Side SerializedSide;

        public Direction Direction
            { get => GetDirection(); }
        public bool IsHorizontal
            { get => CornerA.y == CornerB.y; }
        public bool IsVertical
            { get => CornerA.x == CornerB.x; }
        public int Length
            { get => GetLength(); }

        public RoomWallData() { }

        public RoomWallData(Vector2Int cornerA, Vector2Int cornerB)
        {
            CornerA = cornerA;
            CornerB = cornerB;
        }

        public override string ToString() =>
            $"{{" +
               $"{nameof(AllowConnection)}:{AllowConnection}, " +
               $"{nameof(CornerA)}:{CornerA}, " +
               $"{nameof(CornerB)}:{CornerB}, " +
               $"{nameof(SerializedSide)}: {SerializedSide}" +
            $"}}";

        public IMarker[] GetMarkers()
        {
            var markers = 
                new IMarker[Length];
            var position = CornerA;

            int connectionPosition = -1;
            if (AllowConnection)
                connectionPosition = new System.Random().Next(markers.Length);

            for (int i = 0; i < markers.Length; i++, position += Direction.ToVector2Int())
            {
                if(connectionPosition == i)
                    markers[i] = new Marker<RoomConnectionData>(position, new RoomConnectionData(SerializedSide));
                else
                    markers[i] = new Marker<RoomEdgeData>(position, new RoomEdgeData(SerializedSide));
            }
            return markers;
        }

        public void UpdateSerializedSide()
        {
            SerializedSide = Direction.ToSide();
        }

        private Direction GetDirection()
        {
            if (CornerA == CornerB)
                return Direction.None;
            if (IsHorizontal)
                return CornerA.x > CornerB.x ? Direction.West   : Direction.East;
            if (IsVertical)
                return CornerA.y > CornerB.y ? Direction.South  : Direction.North;
            return Direction.None;
        }

        private int GetLength()
        {
            if (IsHorizontal)
                return Math.Abs(CornerA.x - CornerB.x) + 1;
            if (IsVertical)
                return Math.Abs(CornerA.y - CornerB.y) + 1;
            return 0;
        }
    }
}
