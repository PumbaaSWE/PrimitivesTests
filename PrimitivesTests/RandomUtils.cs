using Microsoft.Xna.Framework;
using System;

namespace MissileMath
{
    public static class RandomUtils
    {
        private static readonly Random random = new();

        public static float RandomFloat(float minValue, float maxValue)
        {
            return random.NextSingle() * (maxValue - minValue) + minValue;
        }


        public static float RandomFloat(float maxValue)
        {
            return random.NextSingle() * maxValue;
        }
        public static Vector2 RandomVector(float x, float y)
        {
            return new Vector2(RandomFloat(x), RandomFloat(y));
        }

        public static Vector2 RandomVector(float length)
        {
            return RandomUnitVector() * length;
        }

        public static Vector2 RandomUnitVector()
        {
            float angle = RandomFloat(-MathF.PI, MathF.PI);
            float x = MathF.Cos(angle);
            float y = MathF.Sin(angle);
            return new Vector2(x, y);
        }

        public static Vector2 RandomVector(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(RandomFloat(minX, maxX), RandomFloat(minY, maxY));
        }

        public static int RandomInt(int maxValue)
        {
            return random.Next(maxValue);
        }

        public static int RandomInt(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public static Color RandomColor()
        {
            return new Color(random.Next(255), random.Next(255), random.Next(255));
        }
    }
}
