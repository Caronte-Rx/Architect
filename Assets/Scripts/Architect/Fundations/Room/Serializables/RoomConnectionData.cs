using System;
using Core;

namespace Architect
{
    [Serializable]
    public class RoomConnectionData
    {
        public RoomEdgeData EdgeData 
            { get; set; }

        public RoomConnectionData(Side side)
        {
            EdgeData = new RoomEdgeData(side);
        }

        public override string ToString() =>
            $"{{{nameof(EdgeData)}:{EdgeData}}}";
    }
}
