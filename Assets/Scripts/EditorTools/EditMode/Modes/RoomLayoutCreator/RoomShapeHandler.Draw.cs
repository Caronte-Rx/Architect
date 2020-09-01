#if UNITY_EDITOR
using Core;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    public partial class RoomShapeHandler
    {
        public bool NeedsRepaint
            { get; set; } = true;


        public void DrawShape()
        {
            int i = 0;
            foreach (var vertex in RoomShape)
            {
                var color = SelectionInfo.EdgeFocused == vertex.EdgeB ? 
                    _settings.WallFocusedColor : 
                    _settings.WallColor;
                DrawWall(vertex.EdgeB, color, _settings.LineWeight, WallsData[i].AllowConnection);
                i++;
            }
            foreach (var vertex in RoomShape)
            {
                var color = SelectionInfo.VertexFocused == vertex ?
                    _settings.CornerFocusedColor :
                    _settings.CornerColor;
                DrawCorner(vertex, color, _settings.LineWeight);
            }
            NeedsRepaint = false;
        }

        private void DrawCorner(V2IntVertex corner, Color color, float lineWeight)
        {
            var weight = new Vector3(lineWeight / 2f, 0, lineWeight / 2f);
            Vector3 GetLocationToDraw(Vector3 direction)
                => corner.Position.ToXZVector3() + Vector3.Scale(weight, direction);

            var vectors = new Vector3[]
            {
                GetLocationToDraw(new Vector3( -1, 0, -1)),
                GetLocationToDraw(new Vector3(  1, 0, -1)),
                GetLocationToDraw(new Vector3(  1, 0,  1)),
                GetLocationToDraw(new Vector3( -1, 0,  1))
            };
            Handles.DrawSolidRectangleWithOutline(vectors, color, color);
        }

        private void DrawWall(V2IntEdge edge, Color color, float lineWeight, bool hasDoor)
        {
            if (edge.AreVerticesInSamePosition)
                return;

            const float DOOR_LENGHT = 1f;

            void DrawSegment(Vector3 p1, Vector3 p2)
            {
                if (edge.AreVerticesInSamePosition)
                    return;
                var width = Vector3.zero;
                if (edge.IsHorizontal)
                    width.z = lineWeight / 2f;
                if (edge.IsVertical)
                    width.x = lineWeight / 2f;
                var vectors = new Vector3[]
                {
                    p1 + width, p2 + width,
                    p2 - width, p1 - width
                };
                Handles.DrawSolidRectangleWithOutline(vectors, color, color);
            }

            var vertexA = edge.VertexA.Position.ToXZVector3();
            var vertexB = edge.VertexB.Position.ToXZVector3();

            if (hasDoor)
            {
                Vector3 space = Vector3.zero;

                var center = new Vector3(vertexA.x, 0, vertexA.z);
                if (edge.IsHorizontal)
                {
                    center.x = (vertexA.x + vertexB.x) / 2f; space.x = DOOR_LENGHT / 2f;
                }
                if (edge.IsVertical)
                {
                    center.z = (vertexA.z + vertexB.z) / 2f; space.z = DOOR_LENGHT / 2f;
                }

                Vector3 GetDoorEdgeLocation(Vector2 direction)
                    => center + new Vector3(space.x * direction.x, 0, space.z * direction.y);

                var doorVertexA = GetDoorEdgeLocation(edge.Direction.Opposite().ToVector2Int());
                var doorVertexB = GetDoorEdgeLocation(edge.Direction.ToVector2Int());

                DrawSegment(    vertexA,  doorVertexA);
                DrawSegment(doorVertexB,      vertexB);
            }
            else
            {
                DrawSegment(vertexA, vertexB);
            }
        }
    }
}
#endif
