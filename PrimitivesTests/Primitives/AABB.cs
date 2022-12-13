using Microsoft.Xna.Framework;
using System;
namespace PrimitivesTests.Primitives
{
    public struct AABB:Shape
    {
        public Vector2 center;
        public Vector2 extents;

        public ShapeType Type { get => ShapeType.AABB; }

        public AABB(Vector2 center, Vector2 extents)
        {
            this.center = center;
            this.extents = extents;
        }

        public AABB(Vector2 center, float halfWidth, float halfHeight) : this(center, new Vector2(halfWidth, halfHeight)) { }

        public Vector2 ClosestPoint(Vector2 point)
        {
            float x = Math.Clamp(point.X, center.X - extents.X, center.X + extents.X);
            float y = Math.Clamp(point.Y, center.Y - extents.Y, center.Y + extents.Y);
            return new Vector2(x, y);
        }
    }
}
