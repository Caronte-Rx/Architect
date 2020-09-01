using System;
using Core;
using UnityEngine;

namespace Architect
{
    [RequireComponent(typeof(FundationBuilder))]
    public class RoomEdge : MonoBehaviour
    {
        public RoomEdgeData EdgeData;

        public virtual void BuildEdge()
        {
            try
            {
                transform.localRotation = EdgeData.Side.ToRotation();
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to build '{GetType().Name}' with {nameof(EdgeData)}: {EdgeData}. " +
                    $"Details: {ex.Message}");
                Destroy(gameObject);
            }
        }

        public virtual RoomEdge Initialize(IMarker marker)
        {
            EdgeData = (marker as Marker<RoomEdgeData>).Data;
            name = $"Edge {marker.Position}";
            transform.localPosition = marker.Position.ToXZVector3();

            GetComponent<FundationBuilder>().OnBuild();
            return this;
        }

        public static RoomEdge CreateEdge(Transform parent)
            => Instantiate(ArchitectSettings.SObject.EdgePrefab, parent, false);
    }
}
