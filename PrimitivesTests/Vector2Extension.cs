using Microsoft.Xna.Framework;
using System;

namespace PrimitivesTests
{
    internal static class Vector2Extension
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
    }
}
