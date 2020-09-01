#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Architect;
using Core;
using UnityEngine;

namespace EditorTools
{
    public partial class RoomShapeHandler
    {
        public V2IntShape RoomShape
            { get; private set; }

        public RoomShapeSelectionInfo SelectionInfo
            { get; private set; }

        public List<RoomWallData> WallsData
            { get; private set; }

        private readonly RoomLayoutCreatorSettings _settings;

        public RoomShapeHandler(RoomShapeData shapeData)
        {
            SelectionInfo = new RoomShapeSelectionInfo();
            SelectionInfo.SelectionChanged += OnSelectionChanged;

            RoomShape =
                new V2IntShape(shapeData.Shape);
            WallsData = shapeData.Walls;

            _settings = RoomLayoutCreatorSettings.SObject;
        }

        public void CreateCornerUnderMouse(Vector2 mousePosition)
        {
            if (!SelectionInfo.IsEdgeFocused)
                return;

            var mouseIntPosition = mousePosition.ToVector2Int();
            var wall = SelectionInfo.EdgeFocused;

            if (!(mouseIntPosition == wall.VertexA.Position ||
                  mouseIntPosition == wall.VertexB.Position))
            {
                var index =
                    RoomShape.Vertices.IndexOf(wall.VertexB);
                WallsData.Insert(index, new RoomWallData());

                RoomShape.SplitEdge(wall, mousePosition.ToVector2Int());
                NeedsRepaint = true;
            }
        }

        public void DeleteCornerUnderMouse()
        {
            if (!SelectionInfo.IsVertexFocused)
                return;

            var corner = SelectionInfo.VertexFocused;
            if (!corner.AreEdgesPerpendicular())
            {
                DeleteWallDataNextTo(corner);

                RoomShape.RemoveVertex(corner);
                SelectionInfo.DeselectCorner();
                NeedsRepaint = true;
            }
        }

        public void DeselectCorner()
        {
            if (!SelectionInfo.IsVertexSelected)
                return;

            var corner = SelectionInfo.VertexSelected;
            if (corner.EdgeA.AreVerticesInSamePosition || corner.EdgeB.AreVerticesInSamePosition)
                DeleteCornerUnderMouse();
            SelectionInfo.DeselectCorner();
        }

        public void DeselectWall()
        {
            if (!SelectionInfo.IsEdgeSelected)
                return;

            RemoveDuplicateCorners();
            SelectionInfo.DeselectWall();
        }

        public void Dispose()
        {
            SelectionInfo.SelectionChanged -= OnSelectionChanged;
        }

        public bool HasIntersections()
        {
            if (!SelectionInfo.IsEdgeSelected)
                return false;

            var wall = SelectionInfo.EdgeSelected;
            foreach (var corner in RoomShape)
            {
                var wallIterator = corner.EdgeB;
                if (wall != wallIterator &&
                    wall.IntersectsWith(wallIterator))
                    return true;

                if (wall.PreviousEdge != wallIterator &&
                    wall.PreviousEdge.IntersectsWith(wallIterator))
                    return true;

                if (wall.NextEdge != wallIterator &&
                    wall.NextEdge.IntersectsWith(wallIterator))
                    return true;
            }
            return false;
        }

        public void MoveCornerUnderMouse(Vector2 mousePosition)
        {
            if (!SelectionInfo.IsVertexSelected)
                return;

            var corner = SelectionInfo.VertexSelected;
            if (!corner.AreEdgesPerpendicular())
            {
                corner.Position =
                    new V2IntEdge(corner.PreviousVertex, corner.NextVertex)
                        .ClampPositionInEdge(mousePosition.ToVector2Int());
                RoomShape.UpdateVertex(corner);
                NeedsRepaint = true;
            }
        }

        public void MoveWallUnderMouse(Vector2 mousePosition)
        {
            if (!SelectionInfo.IsEdgeSelected)
                return;

            var wall = SelectionInfo.EdgeSelected;
            if (!wall.IsPerpendicularWith(wall.PreviousEdge) &&
                !wall.PreviousEdge.AreVerticesInSamePosition)
            {
                var index = RoomShape.Vertices.IndexOf(wall.VertexA);
                RoomShape.InsertVertexAt(index + 1, wall.CloneVertexA());
                WallsData.Insert(index, new RoomWallData());

                SelectionInfo.EdgeSelected = wall.NextEdge;
            }

            wall = SelectionInfo.EdgeSelected;
            if (!wall.IsPerpendicularWith(wall.NextEdge) &&
                !wall.NextEdge.AreVerticesInSamePosition)
            {
                var index = RoomShape.Vertices.IndexOf(wall.VertexB);
                WallsData.Insert(index, new RoomWallData());

                RoomShape.InsertVertexAt(index, wall.CloneVertexB());
            }

            if (wall.IsHorizontal)
                wall.MoveVertically(mousePosition.ToVector2Int());

            if (wall.IsVertical)
                wall.MoveHorizontally(mousePosition.ToVector2Int());

            if (wall.IsHorizontal || wall.IsVertical)
            {
                RoomShape.UpdateVertex(wall.VertexA);
                RoomShape.UpdateVertex(wall.VertexB);
            }
            NeedsRepaint = true;
        }

        public void RemoveDuplicateCorners()
        {
            if (!SelectionInfo.IsEdgeSelected)
                return;

            var wall = SelectionInfo.EdgeSelected;
            if (wall.PreviousEdge.AreVerticesInSamePosition)
            {
                DeleteWallDataNextTo(wall.VertexA.PreviousVertex);
                SelectionInfo.EdgeSelected = wall = RoomShape.RemoveVertex(wall.VertexA);
            }

            if (wall.NextEdge.AreVerticesInSamePosition)
            {
                DeleteWallDataNextTo(wall.VertexB.NextVertex);
                SelectionInfo.EdgeSelected = RoomShape.RemoveVertex(wall.VertexB);
            }
        }

        public void SelectUnderMouse()
        {
            if (SelectionInfo.IsVertexFocused)
            {
                SelectionInfo.SelectCorner();
                SelectionInfo.VertexPositionAtStartOfDrag = SelectionInfo.VertexSelected.Position;
            }
            if (SelectionInfo.IsEdgeFocused)
            {
                SelectionInfo.SelectWall();
                SelectionInfo.EdgePositionAtStartOfDrag = SelectionInfo.EdgeSelected.VertexA.Position;
            }
        }

        public void UpdateShape(Vector2 mousePosition)
        {
            if (SelectionInfo.IsVertexSelected || SelectionInfo.IsEdgeSelected)
                return;

            V2IntVertex vertexFocused = null;
            foreach (var vertex in RoomShape)
            {
                if (Vector2.Distance(mousePosition, vertex.Position) < _settings.LineWeight / 2f)
                {
                    vertexFocused = vertex;
                    break;
                }
            }
            SelectionInfo.VertexFocused = vertexFocused;
            if (!SelectionInfo.IsVertexFocused)
            {
                V2IntEdge edgeFocused = null;
                float closestDistance = _settings.LineWeight;
                foreach (var corner in RoomShape)
                {
                    float distance
                        = corner.EdgeB.GetDistancePointToEdge(mousePosition);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        edgeFocused = corner.EdgeB;
                    }
                }
                SelectionInfo.EdgeFocused = edgeFocused;
            }
        }

        private void DeleteWallDataNextTo(V2IntVertex corner)
        {
            var wallDataA = WallsData[RoomShape.Vertices.IndexOf(corner.PreviousVertex)];
            var wallDataB = WallsData[RoomShape.Vertices.IndexOf(corner)];

            wallDataA.AllowConnection |= wallDataB.AllowConnection;
            WallsData.Remove(wallDataB);
        }

        private void OnSelectionChanged(object sender, EventArgs args)
            => NeedsRepaint = true;
    }
}
#endif
