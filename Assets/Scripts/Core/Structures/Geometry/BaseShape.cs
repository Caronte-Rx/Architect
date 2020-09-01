using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public abstract class BaseShape<TVertex> : IEnumerable<TVertex>
    {
        public List<TVertex> Vertices
            { get; private set; } = new List<TVertex>();

        public virtual TVertex this[int index]
        {
            get => Vertices[index];
            set => Vertices[index] = value;
        }

        public BaseShape() : this(new List<TVertex>()) { }

        public BaseShape(List<TVertex> vertices)
        {
            Vertices = vertices;
        }

        #region IEnumerable

        public IEnumerator<TVertex> GetEnumerator()
        {
            foreach (var vertex in Vertices)
                yield return vertex;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion
    }
}
