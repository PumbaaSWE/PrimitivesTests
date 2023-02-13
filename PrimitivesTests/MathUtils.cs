using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PrimitivesTests
{
    internal class MathUtils
    {
        public static int[] GetNumbers(int number)
        {
            List<int> numbers = new();
            while (number > 0)
            {
                numbers.Add(number % 10);
                number /= 10;
            }
            return numbers.ToArray();
        }
        public static int[] GetNumbers(int number, int amount)
        {
            int[] numbers = new int[amount];
            int i = amount;
            while (number > 0 && i-- > 0)
            {
                numbers[i] = number % 10;
                number /= 10;
            }
            return numbers;
        }

        public static readonly float Epsilon = 0.0005f; //float.Epsilon /use?

        public static bool NearZero(float x)
        {
            return MathF.Abs(x) < Epsilon;
        }
        public static bool NearEqual(float a, float b)
        {
            return MathF.Abs(a - b) < Epsilon;
        }
    }
}
