using System;

namespace Labyrinth.Utils
{
    public static class MathExt
    {
        public static int Mod(int a, int b)
        {
            if (b < 0)
            {
                b = -b;
            }
            return ((a % b) + b) % b;
        }

        public static int RoundInt(float x)
        {
            return (int)Math.Round(x);
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}
