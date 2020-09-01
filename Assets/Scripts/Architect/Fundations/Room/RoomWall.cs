using System;
using Core;
using UnityEngine;

namespace Architect
{
    [RequireComponent(typeof(FundationBuilder))]
    public class RoomWall : MonoBehaviour
    {
        public RoomWallData WallData;

        public IMarker[] WallMarkers
            { get; private set; }

        public virtual void BuildWall()
        {
            try
            {
                name = $"Wall {WallData.SerializedSide}";
                if (!WallData.IsHorizontal && !WallData.IsVertical)
                    throw new Exception($"The wall is not perpendicular.");

                if(WallData.Direction != Direction.None &&
                    (WallData.SerializedSide != WallData.Direction.ToSide() && 
                     WallData.SerializedSide != WallData.Direction.Opposite().ToSide()))
                    throw new Exception($"The wall side doesn't correspond to its direction.");

                WallMarkers = WallData.GetMarkers();
                foreach (var marker in WallMarkers)
                {
                    if (marker.Type == MarkerType.Edge)
                        RoomEdge.CreateEdge(transform).Initialize(marker);
                    if (marker.Type == MarkerType.Connection)
                        RoomConnection.CreateConnection(transform).Initialize(marker);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to build '{GetType().Name}' with {nameof(WallData)}: {WallData}. " +
                    $"Details: {ex.Message}");
                throw ex;
            }
        }

        public RoomWall Initialize(RoomWallData wallData)
        {
            WallData = wallData;

            GetComponent<FundationBuilder>().OnBuild();
            return this;
        }

        public static RoomWall CreateWall(Transform parent)
            => Instantiate(ArchitectSettings.SObject.WallPrefab, parent, false);
    }
}
