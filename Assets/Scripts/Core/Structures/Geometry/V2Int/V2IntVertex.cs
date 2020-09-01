using UnityEngine;

namespace Core
{
    public class V2IntVertex : BaseVertex<V2IntEdge>
    {
        public V2IntVertex NextVertex
            { get => EdgeB.VertexB; }

        public V2IntVertex PreviousVertex
            { get => EdgeA.VertexA; }

        public Vector2Int Position
            { get; set; }

        public int X
            { get => Position.x; }

        public int Y
            { get => Position.y; }

        public V2IntVertex(int x, int y) 
            : this(new Vector2Int(x, y)) { }

        public V2IntVertex(Vector2Int position) : base()
        {
            Position = position;
        }

        public bool AreEdgesPerpendicular()
            => EdgeA.IsPerpendicularWith(EdgeB);

        public V2IntEdge Remove()
        {
            EdgeA.VertexB = NextVertex;
            return NextVertex.EdgeA = EdgeA;
        }
    }
}
