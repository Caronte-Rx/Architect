using System;
using Core;
using UnityEngine;

namespace Architect
{
    [RequireComponent(typeof(FundationBuilder))]
    public class RoomFloor : MonoBehaviour
    {
        public RoomFloorData FloorData;

        private BaseGrid<int> _floorGrid;

        public virtual void BuildFloor()
        {
            try
            {
                name = $"Floor ({FloorData.Size.x}x{FloorData.Size.y})";
                InstantiateMarkers();
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to build '{GetType().Name}' with {nameof(FloorData)}: {FloorData}. " +
                    $"Details: {ex.Message}");
                Destroy(gameObject);
            }
        }

        public RoomFloor Initialize(RoomFloorData floorData)
        {
            FloorData = floorData;

            GetComponent<FundationBuilder>().OnBuild();
            return this;
        }

        public static RoomFloor CreateFloor(Transform parent)
            => Instantiate(ArchitectSettings.SObject.FloorPrefab, parent, false);

        private void InstantiateMarkers()
        {
            _floorGrid = new BaseGrid<int>(FloorData.Size, (int)MarkerType.Cell);
            if (FloorData.WallsMarkers?.Count > 0)
                PlaceCustomShapeMarkers();
            else
                PlaceDefaultShapeMarkers();

            for (int y = 0; y < _floorGrid.Height; y++)
            {
                for (int x = 0; x < _floorGrid.Width; x++)
                {
                    if (_floorGrid[x, y] == (int)MarkerType.Edge ||
                        _floorGrid[x, y] == (int)MarkerType.Cell)
                        RoomCell.CreateCell(transform).Initialize(new Marker<RoomCellData>(new Vector2Int(x, y), new RoomCellData()));
                }
            }
        }

        private void PlaceCustomShapeMarkers()
        {
            foreach (var marker in FloorData.WallsMarkers)
                _floorGrid[marker] = (int)MarkerType.Edge;

            for (int x = 0; x < _floorGrid.Width; x++)
            {
                _floorGrid.Fill(new Vector2Int(x, 0), (int)MarkerType.None, (int)MarkerType.Cell);
                _floorGrid.Fill(new Vector2Int(x, _floorGrid.Height - 1), (int)MarkerType.None, (int)MarkerType.Cell);
            }
            for (int y = 0; y < _floorGrid.Height; y++)
            {
                _floorGrid.Fill(new Vector2Int(0, y), (int)MarkerType.None, (int)MarkerType.Cell);
                _floorGrid.Fill(new Vector2Int(_floorGrid.Width - 1, y), (int)MarkerType.None, (int)MarkerType.Cell);
            }
        }

        private void PlaceDefaultShapeMarkers()
        {
            for (int x = 0; x < _floorGrid.Width; x++)
            {
                _floorGrid[x, 0] = (int)MarkerType.Edge;
                _floorGrid[x, _floorGrid.Height - 1] = (int)MarkerType.Edge;
            }
            for (int y = 0; y < _floorGrid.Height; y++)
            {
                _floorGrid[0, y] = (int)MarkerType.Edge;
                _floorGrid[_floorGrid.Width - 1, y] = (int)MarkerType.Edge;
            }
        }
    }
}
