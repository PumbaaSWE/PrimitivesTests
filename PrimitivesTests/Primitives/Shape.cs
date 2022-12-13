namespace PrimitivesTests.Primitives
{
    public enum ShapeType { AABB, Circle, OBB, Capsule, Hull } //Hull is here a convex polygon
    public interface Shape
    {
        public ShapeType Type { get; }
    }
}
