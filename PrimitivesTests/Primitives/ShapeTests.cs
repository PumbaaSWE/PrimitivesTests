using Microsoft.Xna.Framework;
using System;

namespace PrimitivesTests.Primitives
{
    public static class ShapeTests
    {
        
        /// <summary>
        /// Can so far only do Circle vs All and AABB vs AABB, not OBB vs OBB or AABB vs OBB. Hull and Capsule not implemented at all..
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool Overlap(Shape s1, Shape s2)
        {
            return (s1.Type, s2.Type)
            switch
            {
                (ShapeType.Circle, ShapeType.Circle) => CircleOverlap((Circle)s1, (Circle)s2),
                (ShapeType.Circle, ShapeType.OBB) => CircleOBBOverlap((Circle)s1, (OBB)s2),
                (ShapeType.OBB, ShapeType.Circle) => CircleOBBOverlap((Circle)s2, (OBB)s1),
                (ShapeType.Circle, ShapeType.AABB) => CircleAABBOverlap((Circle)s1, (AABB)s2),
                (ShapeType.AABB, ShapeType.Circle) => CircleAABBOverlap((Circle)s2, (AABB)s1),
                (ShapeType.AABB, ShapeType.AABB) => AABBAABBOverlap((AABB)s1, (AABB)s2),
                _ => false,
            };
        }

        public static bool AABBAABBOverlap(AABB a1, AABB a2)
        {
            Vector2 dist = a2.center - a1.center;
            if (Math.Abs(dist.X) > a1.extents.X + a2.extents.X) return false;
            if (Math.Abs(dist.Y) > a1.extents.Y + a2.extents.Y) return false;
            return true;
        }

        public static bool CircleOBBOverlap(Circle circle, OBB obb)
        {
            Vector2 closestPoint = obb.ClosestPoint(circle.center);
            float dist = Vector2.DistanceSquared(closestPoint, circle.center);
            return dist <= circle.radius * circle.radius;
        }

        public static bool CircleOverlap(Circle circle1, Circle circle2)
        {
            float dist = Vector2.DistanceSquared(circle1.center, circle2.center);
            float r = circle1.radius + circle2.radius;
            return dist <= r * r;
        }

        public static bool CircleAABBOverlap(Circle circle, AABB aabb)
        {
            Vector2 closestPoint = aabb.ClosestPoint(circle.center);
            float dist = Vector2.DistanceSquared(closestPoint, circle.center);
            return dist <= circle.radius * circle.radius;
        }
    }
}
