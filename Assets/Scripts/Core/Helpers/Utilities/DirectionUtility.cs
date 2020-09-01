using System;

namespace Core
{ 
    public static class DirectionUtility
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North        : return Direction.South;
                case Direction.East         : return Direction.West;
                case Direction.South        : return Direction.North;
                case Direction.West         : return Direction.East;
                default:
                    return Direction.None;
            }
        }
    }
}
