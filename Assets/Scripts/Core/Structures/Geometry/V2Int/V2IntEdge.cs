using UnityEditor;
using UnityEngine;

namespace Core
{
    public class V2IntEdge : BaseEdge<V2IntVertex>
    {
        public bool AreVerticesInSamePosition
            { get => IsVertical && IsHorizontal; }

        public Direction Direction
            { get => GetDirection(); }

        public bool IsHorizontal
            { get => VertexA.Y == VertexB.Y; }

        public bool IsVertical
            { get => VertexA.X == VertexB.X; }

        public V2IntEdge NextEdge
            { get => VertexB.EdgeB; }

        public V2IntEdge PreviousEdge
            { get => VertexA.EdgeA; }

        public V2IntEdge(V2IntVertex vertexA, V2IntVertex vertexB)
            : base(vertexA, vertexB) { }

        public Vector2Int ClampPositionInEdge(Vector2Int position)
        {
            if (IsHorizontal)
            {
                var min = Mathf.Min(VertexA.X, VertexB.X);
                var max = Mathf.Max(VertexA.X, VertexB.X);

                position.x = Mathf.Clamp(position.x, min, max);
                position.y = VertexA.Y;
            }
            if (IsVertical)
            {
                var min = Mathf.Min(VertexA.Y, VertexB.Y);
                var max = Mathf.Max(VertexA.Y, VertexB.Y);

                position.x = VertexA.X;
                position.y = Mathf.Clamp(position.y, min, max);
            }
            return position;
        }

        public V2IntVertex CloneVertexA()
            => SplitEdgeAt(new V2IntVertex(VertexA.Position));

        public V2IntVertex CloneVertexB()
            => SplitEdgeAt(new V2IntVertex(VertexB.Position));

        public float GetDistancePointToEdge(Vector2 position)
            => HandleUtility.DistancePointToLineSegment(position, VertexA.Position, VertexB.Position);

        public V2IntLine GetLine()
            => new V2IntLine(VertexA.Position, VertexB.Position);

        public bool IntersectsWith(V2IntEdge otherEdge)
        {
            if (VertexA.Position == otherEdge.VertexA.Position || VertexA.Position == otherEdge.VertexB.Position ||
                VertexB.Position == otherEdge.VertexA.Position || VertexB.Position == otherEdge.VertexB.Position)
                return false;
            return GetLine().IntersectsWith(otherEdge.GetLine());
        }

        public bool IsPerpendicularWith(V2IntEdge otherEdge)
        {
            if (otherEdge.Direction == Direction.None || Direction == Direction.None)
                return false;
            return
                IsHorizontal    && otherEdge.IsVertical ||
                IsVertical      && otherEdge.IsHorizontal;
        }

        public void MoveHorizontally(Vector2Int position)
        {
            VertexA.Position = new Vector2Int(position.x, VertexA.Y);
            VertexB.Position = new Vector2Int(position.x, VertexB.Y);
        }

        public void MoveVertically(Vector2Int position)
        {
            VertexA.Position = new Vector2Int(VertexA.X, position.y);
            VertexB.Position = new Vector2Int(VertexB.X, position.y);
        }

        public V2IntVertex SplitEdgeAt(Vector2Int position)
            => SplitEdgeAt(new V2IntVertex(ClampPositionInEdge(position)));

        public override string ToString()
            => $"{{{VertexA.Position} -> {VertexB.Position}}}";

        private Direction GetDirection()
        {
            if (AreVerticesInSamePosition)
                return Direction.None;
            if (IsHorizontal)
                return VertexA.X > VertexB.X ? Direction.West   : Direction.East;
            if (IsVertical)
                return VertexA.Y > VertexB.Y ? Direction.South  : Direction.North;
            return Direction.None;
        }

        private V2IntVertex SplitEdgeAt(V2IntVertex newVertex)
        {
            var newEdge =
                new V2IntEdge(newVertex, VertexB);
            newVertex.EdgeA = this;
            newVertex.EdgeB = newEdge;

            VertexB.EdgeA = newEdge;
            return VertexB = newVertex;
        }
    }
}
