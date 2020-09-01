using UnityEngine;

namespace Core
{
    public static class Vector3Converter
    {
        public static Vector2 ToXZVector2(this Vector3 v3)
            => new Vector2(v3.x, v3.z);

        public static Vector2Int ToXZVector2Int(this Vector3 v3)
            => new Vector2Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.z));
    }
}
