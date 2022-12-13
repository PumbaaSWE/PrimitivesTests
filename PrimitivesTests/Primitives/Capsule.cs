using Microsoft.Xna.Framework;
using System;

namespace PrimitivesTests.Primitives
{
    internal struct Capsule : Shape
    {
        public ShapeType Type => ShapeType.Capsule;

        public Vector2 center;
        public Vector2 extent; //line segment points are center+-extent
        public float radius;

        public void Rotate(float theta)
        {
            float sin = MathF.Sin(theta);
            float cos = MathF.Cos(theta);
            float x = extent.X * cos - extent.Y * sin;
            float y = extent.X * sin - extent.Y * cos;
            extent.X = x;
            extent.Y = y;
        }
    }
}
