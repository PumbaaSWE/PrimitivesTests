using Microsoft.Xna.Framework;
using System;

namespace PrimitivesTests.Primitives
{
    public struct Hull : Shape
    {
        public ShapeType Type => ShapeType.Hull;

        public Vector2[] model;
        private Vector2 center;
        private Vector2[] world;

        private Vector2 min = Vector2.Zero;
        private Vector2 max = Vector2.Zero;

        private float rotation;
        private float sin;
        private float cos;

        private bool dirty;

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = MathHelper.WrapAngle(value);
                sin = MathF.Sin(rotation);
                cos = MathF.Cos(rotation);
                dirty = true;
            }
        }

        public Vector2 Center
        {
            get { return center; }
            set
            {
                center = value;
                dirty = true;
            }
        }

        public Hull(Vector2[] model)
        {
            this.model = model;
            world = new Vector2[model.Length];
            center = Vector2.Zero;
            sin = 0;
            cos = 1;
            rotation = 0;
            dirty = true;
        }

        public Hull(Vector2 center, Vector2[] model) : this(model)
        {
            this.center = center;
        }
        /// <summary>
        /// Returns this hulls verticies rotated and translated into world coordinates, also clears dirty flag and computes bounds
        /// </summary>
        /// <returns>Vector2 array rotated and translated</returns>
        public Vector2[] GetWorldCoords()
        {
            if (!dirty) return world;
            float minx = float.MaxValue;
            float maxx = float.MinValue;
            float miny = float.MaxValue;
            float maxy = float.MinValue;
            for (int i = 0; i < model.Length; i++)
            {
                world[i] = GetRotatedVector(model[i]) + center;
                minx = MathF.Min(minx, world[i].X);
                maxx = MathF.Max(maxx, world[i].X);
                miny = MathF.Min(miny, world[i].Y);
                maxy = MathF.Max(maxy, world[i].Y);

            }
            min.X = minx;
            min.Y = miny;
            max.X = maxx;
            max.Y = maxy;
            dirty = false;
            return world;
        }
        /// <summary>
        /// Creates a rotated vector in the same rotation as this Hull.
        /// </summary>
        /// <param name="vector">Vector to rotate</param>
        /// <returns>new Vector2</returns>
        public Vector2 GetRotatedVector(Vector2 vector)
        {
            float x = vector.X * cos - vector.Y * sin;
            float y = vector.X * sin + vector.Y * cos;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Finds the vertex/point furthest along given direction and the distance on the projection axis aka Dot(direction, vertex)
        /// </summary>
        /// <param name="direction">Direction to find point in</param>
        /// <param name="max">Distance on the projection axis aka Dot(direction, vertex)</param>
        /// <returns>Vector2 vertex furthest along given direction</returns>
        public Vector2 SupportPoint(Vector2 direction, out float max)
        {
            max = float.MinValue;
            Vector2 bestVertex = Vector2.Zero;
            for (int i = 0; i < model.Length; i++)
            {
                Vector2 vertex = GetRotatedVector(model[i]) + center;
                float projection = Vector2.Dot(direction, vertex);
                if (projection > max)
                {
                    max = projection;
                    bestVertex = vertex;
                }
            }
            return bestVertex;
        }
    }
}
