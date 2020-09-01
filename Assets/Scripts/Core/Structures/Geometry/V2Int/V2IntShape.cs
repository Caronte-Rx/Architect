using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class V2IntShape : BaseShape<V2IntVertex>
    {
        public List<Vector2Int> Positions
            { get; set; }

        public override V2IntVertex this[int index]
        {
            get => Vertices[ClampIndex(index)];
            set
            {
                index = ClampIndex(index);
                Vertices    [index] = value;
                Positions   [index] = value.Position;
            }
        }

        public V2IntShape() : this(new List<Vector2Int>()) { }

        public V2IntShape(List<Vector2Int> vertices)
        {
            Positions = vertices;
            foreach (var vertex in vertices)
                Vertices.Add(new V2IntVertex(vertex));

            for (int i = 0; i <= Vertices.Count; i++)
            {
                var previousEdge = this[i - 1].EdgeB;
                if (this[i].EdgeA == null && previousEdge != null)
                    this[i].EdgeA = previousEdge;

                if (this[i].EdgeB == null)
                    this[i].EdgeB = new V2IntEdge(this[i], this[i + 1]);
            }
        }

        public RectInt CalculateMask()
        {
            if (Vertices.Count == 0)
                return new RectInt();

            int minX, minZ, maxX, maxZ;
            maxX = minX = Vertices[0].X;
            maxZ = minZ = Vertices[0].Y;

            foreach (var vertex in Vertices)
            {
                minX = vertex.X < minX ? vertex.X : minX;
                minZ = vertex.Y < minZ ? vertex.Y : minZ;

                maxX = vertex.X > maxX ? vertex.X : maxX;
                maxZ = vertex.Y > maxZ ? vertex.Y : maxZ;
            }
            return new RectInt(minX, minZ, maxX, maxZ);
        }

        public void InsertVertexAt(int index, V2IntVertex newVertex)
        {
            index = ClampIndex(index);
            Vertices.Insert(index, newVertex); Positions.Insert(index, newVertex.Position);
        }

        public bool IsPointInside(Vector2 point)
        {
            bool result = false;

            int j = Positions.Count - 1;
            for (int i = 0; i < Positions.Count; i++)
            {
                if (Positions[i].y < point.y && Positions[j].y >= point.y ||
                    Positions[j].y < point.y && Positions[i].y >= point.y)
                    if (Positions[i].x + (point.y - Positions[i].y) / (Positions[j].y - Positions[i].y) * (Positions[j].x - Positions[i].x) < point.x)
                        result = !result;
                j = i;
            }
            return result;
        }

        public V2IntEdge RemoveVertex(V2IntVertex vertex)
        {
            var index = Vertices.IndexOf(vertex);
            Vertices.RemoveAt(index); Positions.RemoveAt(index);
            return vertex.Remove();
        }

        public void SplitEdge(V2IntEdge edge, Vector2Int position)
        {
            var index = Vertices.IndexOf(edge.VertexB);
            InsertVertexAt(index, edge.SplitEdgeAt(position));
        }

        public void MoveShapeTo(Vector2Int destination)
        {
            var v = destination - CalculateMask().position;
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Position += v;
                UpdateVertex(Vertices[i]);
            }
        }

        public void UpdateVertex(V2IntVertex vertex)
            => this[Vertices.IndexOf(vertex)] = vertex;

        private int ClampIndex(int index)
            => (index + Vertices.Count) % Vertices.Count;
    }
}
