using UnityEngine;

namespace Architect
{
    public class Marker<TData> : IMarker
    {
        public MarkerType Type
            { get; private set; } = MarkerType.None;
        public Vector2Int Position
            { get; set; }
        public TData Data
            { get; set; }

        public Marker(Vector2Int position, TData data)
        {
            Position = position; Data = data;
            if(typeof(RoomCellData).IsAssignableFrom(typeof(TData)))
                Type = MarkerType.Cell;
            if (typeof(RoomEdgeData).IsAssignableFrom(typeof(TData)))
                Type = MarkerType.Edge;
            if (typeof(RoomConnectionData).IsAssignableFrom(typeof(TData)))
                Type = MarkerType.Connection;
        }
    }
}
