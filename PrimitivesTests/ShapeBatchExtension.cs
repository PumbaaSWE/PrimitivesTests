using EzGraphics;
using Microsoft.Xna.Framework;
using PrimitivesTests.Primitives;

namespace PrimitivesTests
{
    public static class ShapeBatchExtension
    {
        public static void DrawShape(this ShapeBatch shapeBatch, Shape shape, Color color, float lineWidth = 2)
        {
            switch (shape.Type)
            {
                case ShapeType.AABB:
                    AABB aabb = (AABB)shape;
                    shapeBatch.DrawRectangle(aabb.center, aabb.extents, 0, color, lineWidth);
                    break;
                case ShapeType.Circle:
                    shapeBatch.DrawCircle((Circle)shape, color, lineWidth);
                    break;
                case ShapeType.OBB:
                    OBB obb = (OBB)shape;
                    shapeBatch.DrawRectangle(obb.center, obb.extents, obb.Rotation, color, lineWidth);
                    break;
                case ShapeType.Capsule:
                    break;
                case ShapeType.Hull:
                    shapeBatch.DrawPolygon(((Hull)shape).GetWorldCoords(), color, lineWidth);
                    break;
                default:
                    break;
            }
        }


        public static void DrawHull(this ShapeBatch shapeBatch, Hull hull, Color color, float lineWidth = 2)
        {
            shapeBatch.DrawPolygon(hull.GetWorldCoords(), color, lineWidth);
        }

        public static void DrawCircle(this ShapeBatch shapeBatch, Circle circle, Color color, float lineWidth = 2)
        {
            shapeBatch.DrawCircle(circle.center, circle.radius, color, lineWidth);
        }
    }
}
