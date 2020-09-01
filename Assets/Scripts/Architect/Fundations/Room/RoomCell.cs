using System;
using Core;
using UnityEngine;

namespace Architect
{
    [RequireComponent(typeof(FundationBuilder))]
    public class RoomCell : MonoBehaviour
    {
        public RoomCellData CellData;

        public virtual void BuildCell()
        {
            try
            {
                name = $"Cell {transform.localPosition.ToXZVector2Int()}";
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to build '{GetType().Name}' with {nameof(CellData)}: {CellData}. " +
                    $"Details: {ex.Message}");
                Destroy(gameObject);
            }
        }

        public RoomCell Initialize(IMarker marker)
        {
            CellData = (marker as Marker<RoomCellData>).Data;
            transform.localPosition = marker.Position.ToXZVector3();

            GetComponent<FundationBuilder>().OnBuild();
            return this;
        }

        public static RoomCell CreateCell(Transform parent)
            => Instantiate(ArchitectSettings.SObject.CellPrefab, parent, false);
    }
}
