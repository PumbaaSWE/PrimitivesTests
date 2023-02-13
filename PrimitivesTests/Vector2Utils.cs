using Microsoft.Xna.Framework;
using System;

namespace PrimitivesTests
{
    internal static class Vector2Utils
    {
        /// <summary>
        /// Rorate the vector by theta radians
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="theta">In Radians</param>
        public static void Rotate(this Vector2 vec, float theta)
        {
            float sin = MathF.Sin(theta);
            float cos = MathF.Cos(theta);
            float x = vec.X * cos - vec.Y * sin;
            float y = vec.X * sin + vec.Y * cos;
            vec.X = x;
            vec.Y = y;
        }

        public static float Epsilon = 0.0005f;
        public static bool IsZero(this Vector2 vec)
        {
            return MathF.Abs(vec.X) < Epsilon && MathF.Abs(vec.Y) < Epsilon;
        }

        public static bool IsEqual(Vector2 a, Vector2 b)
        {
            return MathF.Abs(a.X - b.X) < Epsilon && MathF.Abs(a.Y - b.Y) < Epsilon;
        }

        public static float PseudoCross(Vector2 a, Vector2 b)
        {
            // cz = ax * by − ay * bx
            return a.X * b.Y - a.Y * b.X;
        }

        /// <summary>
        /// To get a vector with length of one at specified angle
        /// </summary>
        /// <param name="angle">Desired angle in radians</param>
        /// <returns>Vector2 with length one and rotation of angle</returns>
        public static Vector2 GetUnitVector(float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);
            return new Vector2(cos, sin);
        }
    }
}
