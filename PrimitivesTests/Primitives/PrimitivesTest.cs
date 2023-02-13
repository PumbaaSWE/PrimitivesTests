using Microsoft.Xna.Framework;

namespace PrimitivesTests.Primitives
{
    public class PrimitivesTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="a">First point in segment</param>
        /// <param name="b">Second point in segment</param>
        /// <param name="t">returns distance</param>
        /// <returns>Vector2 as the closest point on the segment to the point</returns>
        public static Vector2 ClosestPtPointSegment(Vector2 point, Vector2 a, Vector2 b, out float t)
        {
            Vector2 ab = b - a;
            t = Vector2.Dot(point - a, ab);
            if (t <= 0.0f)
            {
                t = 0.0f;
                return a;
            }
            else
            {
                float denom = Vector2.Dot(ab, ab);
                if (t >= denom)
                {
                    t = 1.0f;
                    return b;
                }
                else
                {
                    t /= denom;
                    return a + t * ab;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Squared distance to the line segment ab from point c</returns>
        public static float SqDistPointSegment(Vector2 c, Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            Vector2 ac = c - a;
            Vector2 bc = c - b;
            float e = Vector2.Dot(ac, ab);
            if (e <= 0.0f) return Vector2.Dot(ac, ac);
            float f = Vector2.Dot(ab, ab);
            if (e >= f) return Vector2.Dot(bc, bc);
            return Vector2.Dot(ac, ac) - e * e / f;
        }
        public static float SqDistPointLine(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            Vector2 ap = p - a;
            float e = Vector2.Dot(ap, ab);
            float f = Vector2.Dot(ab, ab);
            return Vector2.Dot(ap, ap) - e * e / f;
        }

        public static Vector2 ClosestPtPointOBB(Vector2 p, OBB b)
        {
            Vector2 d = p - b.center;

            Vector2 q = b.center;
            //x
            float dist = Vector2.Dot(d, b.Up);
            if (dist > b.extents.X) dist = b.extents.X;
            if (dist < -b.extents.X) dist = -b.extents.X;
            q += dist * b.Up;
            //y
            dist = Vector2.Dot(d, b.Right);
            if (dist > b.extents.Y) dist = b.extents.Y;
            if (dist < -b.extents.Y) dist = -b.extents.Y;
            q += dist * b.Right;
            return q;
        }
        public static void ClosestPtPointOBB(Vector2 p, OBB b, out Vector2 q)
        {
            Vector2 d = p - b.center;

            q = b.center;
            //x
            float dist = Vector2.Dot(d, b.Up);
            if (dist > b.extents.X) dist = b.extents.X;
            if (dist < -b.extents .X) dist = -b.extents.X;
            q += dist * b.Up;
            //y
            dist = Vector2.Dot(d, b.Right);
            if (dist > b.extents.Y) dist = b.extents.Y;
            if (dist < -b.extents.Y) dist = -b.extents.Y;
            q += dist * b.Right;
        }

        public static float SqDistPointOBB2(Vector2 p, OBB b)
        {
            Vector2 closest = ClosestPtPointOBB(p, b);
            float sqDist = Vector2.Dot(closest - p, closest - p);
            return sqDist;
        }
        public static float SqDistPointOBB(Vector2 p, OBB b)
        {
            Vector2 v = p - b.center;
            float sqDist = .0f;

            float d = Vector2.Dot(v, b.Up), excess = 0.0f;
            if (d < -b.extents.X)
                excess = d + b.extents.X;
            else if (d > b.extents.X)
                excess = d - b.extents.X;
            sqDist += excess * excess;

            d = Vector2.Dot(v, b.Right); excess = 0.0f;
            if (d < -b.extents.Y)
                excess = d + b.extents.Y;
            else if (d > b.extents.Y)
                excess = d - b.extents.Y;
            sqDist += excess * excess;

            return sqDist;
        }
    }
}
