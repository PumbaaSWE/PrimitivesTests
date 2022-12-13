using Microsoft.Xna.Framework;
using System;

namespace PrimitivesTests.Primitives
{
    public struct OBB : Shape
    {
        public Vector2 center;
        public Vector2 extents;

        public Vector2 Up { get; private set; }
        public Vector2 Right { get; private set; }

        public ShapeType Type { get => ShapeType.OBB; }

        public OBB(Vector2 center, Vector2 extents)
        {
            this.center = center;
            this.extents = extents;
            Up = -Vector2.UnitY;
            Right = Vector2.UnitX;
        }

        public OBB(Vector2 center, float halfWidth, float halfHeight) :this(center, new Vector2(halfWidth, halfHeight)) { }


        public void Rotate(float theta)
        {
            float sin = MathF.Sin(theta);
            float cos = MathF.Cos(theta);
            Up = new Vector2(sin, -cos); //actually (-sin, cos) but positive Y is down in MonoGame
            Right = new Vector2(cos, sin);
        }

        public Vector2 ClosestPoint(Vector2 point)
        {
            Vector2 d = point - center;
            Vector2 closestPoint = center;

            float dist = Vector2.Dot(d, Right);
            if (dist > extents.X) dist = extents.X;
            if (dist < -extents.X) dist = -extents.X;
            closestPoint += dist * Right;

            dist = Vector2.Dot(d, Up);
            if (dist > extents.Y) dist = extents.Y;
            if (dist < -extents.Y) dist = -extents.Y;
            closestPoint += dist * Up;

            return closestPoint;
        }
    }
}
