using System;

namespace Core
{
    public abstract class BaseVertex<TEdge> : IBaseVertex where TEdge : IBaseEdge
    {
        public TEdge EdgeA { get; set; }
        public TEdge EdgeB { get; set; }

        public BaseVertex() { }
    }
}
