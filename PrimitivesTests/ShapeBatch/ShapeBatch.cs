using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace EzGraphics
{
    public class ShapeBatch : IDisposable
    {
        private bool isDisposed;
        private readonly GraphicsDevice device;
        private BasicEffect effect;

        private readonly VertexPositionColor[] vertices;
        private readonly int[] indices;

        private int shapeCount;
        private int indexCount;
        private int vertexCount;

        private bool started;

        public ShapeBatch(GraphicsDevice device, int maxVertices = 2048)
        {
            this.device = device;
            Viewport viewport = device.Viewport;
            isDisposed = false;

            effect = new(device)
            {
                FogEnabled = false,
                LightingEnabled = false,
                TextureEnabled = false,
                VertexColorEnabled = true,
                PreferPerPixelLighting = false,
                View = Matrix.Identity,
                Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1)
            };

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            device.RasterizerState = rasterizerState;

            vertices = new VertexPositionColor[maxVertices];
            indices = new int[vertices.Length * 3];

            shapeCount = 0;
            vertexCount = 0;
            indexCount = 0;

            started = false;
        }

        ~ShapeBatch()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed) return;
            if (disposing)
            {
                effect?.Dispose();
                effect = null;
            }

            isDisposed = true;
        }

        public void Begin(Matrix? transforMatrix = null)
        {
            if (transforMatrix.HasValue)
                effect.View = transforMatrix.Value;
            else
                effect.View = Matrix.Identity;
            started = true;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width, device.Viewport.Height, 0, 0, 1);
        }

        public void End()
        {
            Flush();
            started = false;
        }

        private void Flush()
        {
            if (shapeCount == 0) return;

            int primitiveCount = indexCount / 3;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices,
                    0,
                    vertexCount,
                    indices,
                    0,
                    primitiveCount);
            }
            shapeCount = 0;
            vertexCount = 0;
            indexCount = 0;
        }

        public void EnsureStarted()
        {
            if (!started)
            {
                throw new Exception("ShapeBatch not started");
            }
        }

        private void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
        {
            int maxVertexCount = vertices.Length;
            int maxIndexCount = indices.Length;

            if (shapeVertexCount > maxVertexCount || shapeIndexCount > maxIndexCount)
            {
                throw new Exception("Max vertex or index count reached for one draw.");
            }

            if (vertexCount + shapeVertexCount > maxVertexCount || indexCount + shapeIndexCount > maxIndexCount)
            {
                Flush();
            }
        }

        public void FillTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            EnsureStarted();

            int shapeVertexCount = 3;
            int shapeIndexCount = 3;

            EnsureSpace(shapeVertexCount, shapeIndexCount);

            indices[indexCount++] = vertexCount + 0;
            indices[indexCount++] = vertexCount + 1;
            indices[indexCount++] = vertexCount + 2;

            vertices[vertexCount++] = new VertexPositionColor(new Vector3(a, 0f), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(b, 0f), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(c, 0f), color);
        }

        public void FillQuad(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureSpace(shapeVertexCount, shapeIndexCount);

            indices[indexCount++] = vertexCount + 0;
            indices[indexCount++] = vertexCount + 1;
            indices[indexCount++] = vertexCount + 2;
            indices[indexCount++] = vertexCount + 0;
            indices[indexCount++] = vertexCount + 2;
            indices[indexCount++] = vertexCount + 3;

            vertices[vertexCount++] = new VertexPositionColor(new Vector3(dx, dy, 0f), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(cx, cy, 0f), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(bx, by, 0f), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(ax, ay, 0f), color);

            shapeCount++;
        }

        public void FillRectangle(Vector2 center, Vector2 extents, float rotation, Color color)
        {
            FillRectangle(center, extents.X * 2, extents.Y * 2, rotation, color);
        }

        public void FillRectangle(Vector2 center, float width, float height, float rotation, Color color)
        {
            float left = -width * 0.5f;
            float right = left + width;
            float bottom = -height * 0.5f;
            float top = bottom + height;

            float sin = MathF.Sin(rotation);
            float cos = MathF.Cos(rotation);

            // Vector components
            float ax = left;
            float ay = top;
            float bx = right;
            float by = top;
            float cx = right;
            float cy = bottom;
            float dx = left;
            float dy = bottom;

            // Rotation
            float rx1 = ax * cos - ay * sin;
            float ry1 = ax * sin + ay * cos;
            float rx2 = bx * cos - by * sin;
            float ry2 = bx * sin + by * cos;
            float rx3 = cx * cos - cy * sin;
            float ry3 = cx * sin + cy * cos;
            float rx4 = dx * cos - dy * sin;
            float ry4 = dx * sin + dy * cos;

            // Translation
            ax = rx1 + center.X;
            ay = ry1 + center.Y;
            bx = rx2 + center.X;
            by = ry2 + center.Y;
            cx = rx3 + center.X;
            cy = ry3 + center.Y;
            dx = rx4 + center.X;
            dy = ry4 + center.Y;

            FillQuad(ax, ay, bx, by, cx, cy, dx, dy, color);
        }

        public void DrawRectangle(Vector2 center, Vector2 extents, float rotation, Color color, float lineWidth = 2.0f)
        {
            DrawRectangle(center, extents.X * 2, extents.Y * 2, rotation, color, lineWidth);
        }

        public void DrawRectangle(Vector2 center, float width, float height, float rotation, Color color, float lineWidth = 2.0f)
        {
            float left = -width * 0.5f;
            float right = left + width;
            float bottom = -height * 0.5f;
            float top = bottom + height;

            // Precompute the trig. functions.
            float sin = MathF.Sin(rotation);
            float cos = MathF.Cos(rotation);

            // Vector components:

            float ax = left;
            float ay = top;
            float bx = right;
            float by = top;
            float cx = right;
            float cy = bottom;
            float dx = left;
            float dy = bottom;

            // Rotation transform:

            float rx1 = ax * cos - ay * sin;
            float ry1 = ax * sin + ay * cos;
            float rx2 = bx * cos - by * sin;
            float ry2 = bx * sin + by * cos;
            float rx3 = cx * cos - cy * sin;
            float ry3 = cx * sin + cy * cos;
            float rx4 = dx * cos - dy * sin;
            float ry4 = dx * sin + dy * cos;

            // Translation transform:

            ax = rx1 + center.X;
            ay = ry1 + center.Y;
            bx = rx2 + center.X;
            by = ry2 + center.Y;
            cx = rx3 + center.X;
            cy = ry3 + center.Y;
            dx = rx4 + center.X;
            dy = ry4 + center.Y;

            DrawLine(ax, ay, bx, by, lineWidth, color);
            DrawLine(bx, by, cx, cy, lineWidth, color);
            DrawLine(cx, cy, dx, dy, lineWidth, color);
            DrawLine(dx, dy, ax, ay, lineWidth, color);
        }

        public void DrawLine(Vector2 a, Vector2 b, Color color, float width = 2.0f)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, width, color);
        }

        public void DrawLine(float x1, float y1, float x2, float y2, float lineWidth, Color color)
        {

            float halfWidth = lineWidth * 0.5f;

            float dx = x2 - x1;
            float dy = y2 - y1;

            float inverseLength = 1f / MathF.Sqrt(dx * dx + dy * dy);
            dx *= inverseLength;
            dy *= inverseLength;


            float nx = -dy;
            float ny = dx;

            dx *= halfWidth;
            dy *= halfWidth;

            nx *= halfWidth;
            ny *= halfWidth;


            float qax = x1 + nx - dx;
            float qay = y1 + ny - dy;

            float qbx = x2 + nx + dx;
            float qby = y2 + ny + dy;

            float qcx = x2 - nx + dx;
            float qcy = y2 - ny + dy;

            float qdx = x1 - nx - dx;
            float qdy = y1 - ny - dy;

            FillQuad(qax, qay, qbx, qby, qcx, qcy, qdx, qdy, color);
        }

        public void DrawCircle(Vector2 position, float radius, Color color, float lineWidth = 2.0f, int points = 64)
        {
            DrawCircle(position.X, position.Y, radius, lineWidth, points, color);
        }

        public void DrawCircle(float x, float y, float radius, float lineWidth, int points, Color color)
        {
            float angle = MathHelper.TwoPi / points;
            float sin = MathF.Sin(angle);
            float cos = MathF.Cos(angle);

            float ax = radius;
            float ay = 0f;

            for (int i = 0; i < points; i++)
            {
                float bx = ax * cos - ay * sin;
                float by = ax * sin + ay * cos;

                DrawLine(ax + x, ay + y, bx + x, by + y, lineWidth, color);
                ax = bx;
                ay = by;
            }
        }

        public void DrawArc(Vector2 position, float radius, float startAngle, float angleSpan, Color color, float lineWidth = 2.0f, int points = 64)
        {
            DrawArc(position.X, position.Y, radius, startAngle, angleSpan, lineWidth, points, color);
        }

        public void DrawArc(float x, float y, float radius, float startAngle, float angleSpan, float lineWidth, int points, Color color)
        {
            float angle = angleSpan / points;
            float sin = MathF.Sin(angle);
            float cos = MathF.Cos(angle);

            float startSin = MathF.Sin(startAngle);
            float startCos = MathF.Cos(startAngle);

            float ax = radius * startCos;
            float ay = radius * startSin;

            for (int i = 0; i < points; i++)
            {
                float bx = ax * cos - ay * sin;
                float by = ax * sin + ay * cos;

                DrawLine(ax + x, ay + y, bx + x, by + y, lineWidth, color);
                ax = bx;
                ay = by;
            }
        }

        public void DrawPolygon(Vector2[] vertices, Vector2 position, float rotation, Vector2 scale, Color color, float lineWidth = 2.0f)
        {
            float sin = MathF.Sin(rotation);
            float cos = MathF.Cos(rotation);
            // Get last vertex.
            Vector2 v = vertices[^1];
            // Scale
            float sx = v.X * scale.X;
            float sy = v.Y * scale.Y;
            // Rotate
            float rx = sx * cos - sy * sin;
            float ry = sx * sin + sy * cos;
            // Translate
            float x1 = rx + position.X;
            float y1 = ry + position.Y;

            for (int i = 0; i < vertices.Length; i++)
            {
                v = vertices[i];
                // Scale
                sx = v.X * scale.X;
                sy = v.Y * scale.Y;
                // Rotate
                rx = sx * cos - sy * sin;
                ry = sx * sin + sy * cos;
                // Translate
                float x2 = rx + position.X;
                float y2 = ry + position.Y;

                DrawLine(x1, y1, x2, y2, lineWidth, color);

                x1 = x2;
                y1 = y2;
            }
        }

        public void DrawPolygon(Vector2[] vertices, Color color, float lineWidth = 2.0f)
        {
            Vector2 a = vertices[^1];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 b = vertices[i];
                DrawLine(a, b, color, lineWidth);
                a = b;
            }
        }
    }
}
