using System;

namespace Core
{
    public abstract class BaseEdge<TVertex> : IBaseEdge where TVertex : IBaseVertex
    {
        public TVertex VertexA { get; set; }
        public TVertex VertexB { get; set; }

        public BaseEdge(TVertex vertexA, TVertex vertexB)
        {
            VertexA = vertexA;
            VertexB = vertexB;
        }
    }
}
