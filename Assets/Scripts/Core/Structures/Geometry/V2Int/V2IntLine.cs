using System;
using UnityEngine;

namespace Core
{
    public class V2IntLine
    {
        public Vector2Int P1
            { get; set; }

        public Vector2Int P2
            { get; set; }

        public V2IntLine(Vector2Int p1, Vector2Int p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public AngleOrientation CalculateOrientationWith(Vector2Int Point)
        {
            int val = (P2.y - P1.y) * (Point.x - P2.x) -
                      (P2.x - P1.x) * (Point.y - P2.y);
            if (val == 0)
                return AngleOrientation.Collinear;
            return val > 0 ? AngleOrientation.Clockwise : AngleOrientation.Counterclockwise;
        }

        public bool Contains(Vector2Int Point)
        {
            return
                Point.x <= Math.Max(P1.x, P2.x) && Point.x >= Math.Min(P1.x, P2.x) &&
                Point.y <= Math.Max(P1.y, P2.y) && Point.y >= Math.Min(P1.y, P2.y);
        }

        public bool IntersectsWith(V2IntLine otherLine)
        {
            var oA = CalculateOrientationWith(otherLine.P1);
            var oB = CalculateOrientationWith(otherLine.P2);
            var oC = otherLine.CalculateOrientationWith(P1);
            var oD = otherLine.CalculateOrientationWith(P2);

            if (oA != oB && oC != oD) return true;
            if (oA == AngleOrientation.Collinear && Contains(otherLine.P1)) return true;
            if (oB == AngleOrientation.Collinear && Contains(otherLine.P2)) return true;
            if (oC == AngleOrientation.Collinear && otherLine.Contains(P1)) return true;
            if (oD == AngleOrientation.Collinear && otherLine.Contains(P2)) return true;
            return false;
        }
    }
}
