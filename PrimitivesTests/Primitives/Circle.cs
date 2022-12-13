using Microsoft.Xna.Framework;
namespace PrimitivesTests.Primitives
{
    public struct Circle:Shape
    {
        public Vector2 center;
        public float radius;
        public ShapeType Type { get => ShapeType.Circle; }

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

    }
}
