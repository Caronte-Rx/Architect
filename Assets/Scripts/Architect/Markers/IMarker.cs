using UnityEngine;

namespace Architect
{
    public interface IMarker
    {
        MarkerType Type { get; }
        Vector2Int Position { get; set; }
    }
}
