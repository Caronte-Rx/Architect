using UnityEngine;

namespace Core
{
    public static class SideConverter
    {
        public static Quaternion ToRotation(this Side side)
        {
            switch (side)
            {
                case Side.Behind    : return Quaternion.identity;
                case Side.Right     : return Quaternion.Euler(0f,  90f, 0f);
                case Side.Front     : return Quaternion.Euler(0f, 180f, 0f);
                case Side.Left      : return Quaternion.Euler(0f, 270f, 0f);
                default:
                    return Quaternion.identity;
            }
        }
    }
}
