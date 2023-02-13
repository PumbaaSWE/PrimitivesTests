using EzGraphics;
using Microsoft.Xna.Framework;

namespace PrimitivesTests
{
    public class DebugDraw
    {


        public static Color Color = Color.White;

        private static readonly int MAX_SHAPES = 1000;
        private static readonly IDrawShapes[] list = new IDrawShapes[MAX_SHAPES];
        private static int currentIndex = 0;
        private static bool overflow = false;


        public static void AddShape(IDrawShapes drawShapes)
        {
            list[currentIndex++] = drawShapes;
            if (currentIndex >= MAX_SHAPES)
            {
                currentIndex = 0;
                overflow = true;
            }
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            AddShape(new DrawLine(start, end, color));
        }

        public static void DrawLine(Vector2 start, Vector2 end)
        {
            DrawLine(start, end, Color);
        }

        public static void DrawCircle(Vector2 start, float radius, Color color)
        {
            AddShape(new DrawCircle(start, radius, color));
        }

        public static void DrawCircle(Vector2 start, float radius)
        {
            DrawCircle(start, radius, Color);
        }

        public static void Draw(ShapeBatch shapeBatch, float scale = 1)
        {
            int length = overflow ? MAX_SHAPES : currentIndex;
            for (int i = 0; i < length; i++)
            {
                list[i].Draw(shapeBatch, scale);
                //list[i] = null;
            }
            currentIndex = 0;
            overflow = false;
        }
    }

    public interface IDrawShapes
    {
        public void Draw(ShapeBatch shapeBatch, float scale);
    }
    struct DrawLine : IDrawShapes
    {
        Vector2 start;
        Vector2 end;
        Color color;
        public DrawLine(Vector2 start, Vector2 end, Color color)
        {
            this.start = start;
            this.end = end;
            this.color = color;
        }
        public void Draw(ShapeBatch shapeBatch, float scale)
        {
            shapeBatch.DrawLine(start * scale, end * scale, color);
        }
    }

    struct DrawCircle : IDrawShapes
    {
        Vector2 center;
        float radius;
        Color color;
        public DrawCircle(Vector2 center, float radius, Color color)
        {
            this.center = center;
            this.radius = radius;
            this.color = color;
        }
        public void Draw(ShapeBatch shapeBatch, float scale)
        {
            shapeBatch.DrawCircle(center * scale, radius * scale, color);
        }
    }
    
}
