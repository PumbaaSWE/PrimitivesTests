using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PrimitivesTests.Primitives
{
    public struct OBB : Shape
    {
        public Vector2 center;
        public Vector2 extents;

        private float rotation = 0;
        private Vector2 up;
        private Vector2 right;
        /// <summary>
        /// Computed Up vector of this OBB, can be uset to set the rotation also
        /// </summary>
        public Vector2 Up { get { return up; } set { SetRotation(MathF.Atan2(value.Y, value.X)); } }
        /// <summary>
        /// Computed Right vector of this OBB, can be uset to set the rotation also
        /// </summary>
        public Vector2 Right { get { return right; } set { SetRotation(MathF.Atan2(-value.X, value.Y)); } }

        public ShapeType Type { get => ShapeType.OBB; }

        public OBB(Vector2 center, Vector2 extents)
        {
            this.center = center;
            this.extents = extents;
            up = -Vector2.UnitY;
            right = Vector2.UnitX;
        }

        public OBB(Vector2 center, float halfWidth, float halfHeight) :this(center, new Vector2(halfWidth, halfHeight)) { }

        /// <summary>
        /// Sets the rotation of this OBB in radians
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set
            {
                SetRotation(value);
            }
        }
        private void SetRotation(float angle)
        {
            rotation = MathHelper.WrapAngle(angle);
            float sin = MathF.Sin(angle);
            float cos = MathF.Cos(angle);
            up = new Vector2(sin, -cos); //actually (-sin, cos) but positive Y is down in MonoGame
            right = new Vector2(cos, sin);
        }

        /// <summary>
        /// Returns the closet point on this OBB to specified point
        /// </summary>
        /// <param name="point">The specified point</param>
        /// <returns>new Vector2 that is the closest to point</returns>
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
