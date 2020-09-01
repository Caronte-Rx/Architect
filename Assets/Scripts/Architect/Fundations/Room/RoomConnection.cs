using System;
using UnityEngine;

namespace Architect
{
    public class RoomConnection : RoomEdge
    {
        public RoomConnectionData ConnectionData;

        public override void BuildEdge()
        {
            try
            {
                base.BuildEdge();
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to build '{GetType().Name}' with {nameof(ConnectionData)}: {ConnectionData}. " +
                    $"Details: {ex.Message}");
                Destroy(gameObject);
            }
        }

        public override RoomEdge Initialize(IMarker marker)
        {
            ConnectionData = (marker as Marker<RoomConnectionData>).Data;
            base.Initialize(new Marker<RoomEdgeData>(marker.Position, ConnectionData.EdgeData));
            name = $"Connection {marker.Position}";

            return this;
        }

        public static RoomConnection CreateConnection(Transform parent)
            => Instantiate(ArchitectSettings.SObject.ConnectionPrefab, parent, false);
    }
}
