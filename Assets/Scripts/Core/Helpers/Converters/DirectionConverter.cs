using UnityEngine;

namespace Core
{
    public static class DirectionConverter
    {
        public static Side ToSide(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North    : return Side.Left;
                case Direction.East     : return Side.Behind;
                case Direction.South    : return Side.Right;
                case Direction.West     : return Side.Front;
                default:
                    return Side.None;
            }
        }

        public static Vector2 ToVector2(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North    : return Vector2.up;
                case Direction.East     : return Vector2.right;
                case Direction.South    : return Vector2.down;
                case Direction.West     : return Vector2.left;
                default:
                    return Vector2.zero;
            }
        }

        public static Vector2Int ToVector2Int(this Direction direction)
            => direction.ToVector2().ToVector2Int();

        public static Vector3 ToXZVector3(this Direction direction)
            => direction.ToVector2().ToXZVector3();
    }
}
