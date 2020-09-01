using UnityEngine;

namespace Core
{
    public static class Vector2Converter
    {
        public static Vector2Int ToVector2Int(this Vector2 v2)
            => new Vector2Int(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y));

        public static Vector3 ToXZVector3(this Vector2 v2)
            => v2.ToXZVector3(0);

        public static Vector3 ToXZVector3(this Vector2 v2, float y)
            => new Vector3(v2.x, y, v2.y);

        public static Vector3 ToXZVector3(this Vector2Int v2)
            => v2.ToXZVector3(0);

        public static Vector3 ToXZVector3(this Vector2Int v2, float y)
            => new Vector3(v2.x, y, v2.y);
    }
}
