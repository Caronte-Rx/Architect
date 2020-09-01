using System;
using Core;

namespace Architect
{
    [Serializable]
    public class RoomEdgeData
    {
        public Side Side;

        public RoomEdgeData(Side side)
        {
            Side = side;
        }

        public override string ToString()
            => $"{{{nameof(Side)}:{Side}}}";
    }
}
