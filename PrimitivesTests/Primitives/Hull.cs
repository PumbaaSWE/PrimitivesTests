using Microsoft.Xna.Framework;
using System;

namespace PrimitivesTests.Primitives
{
    internal struct Hull : Shape
    {
        public ShapeType Type => ShapeType.Hull;

        public Vector2[] model;
        public Vector2 center;
        public Vector2[] world;

        private float rotation;
        private float sin;
        private float cos;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = MathHelper.WrapAngle(value);
                sin = MathF.Sin(rotation);
                cos = MathF.Cos(rotation);
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
        }

        public Hull(Vector2 center, Vector2[] model) : this(model)
        {
            this.center = center;
        }

        public Vector2[] GetWorldCoords()
        {   
            for (int i = 0; i < model.Length; i++)
            {
                world[i] = GetRotatedVector(model[i]) + center;
            }
            return world;
        }

        private Vector2 GetRotatedVector(Vector2 vector)
        {
            float x = vector.X * cos - vector.Y * sin;
            float y = vector.X * sin + vector.Y * cos;
            return new Vector2(x, y);
        }
    }
}
