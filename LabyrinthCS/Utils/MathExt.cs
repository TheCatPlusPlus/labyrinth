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
            return (a % b + b) % b;
        }

        public static int RoundInt(float x)
        {
            return (int)Math.Round(x);
        }

        public static int FloorInt(float x)
        {
            return (int)Math.Floor(x);
        }

        public static int CeilInt(float x)
        {
            return (int)Math.Ceiling(x);
        }

        public static int RoundInt(decimal x)
        {
            return (int)Math.Round(x);
        }

        public static int FloorInt(decimal x)
        {
            return (int)Math.Floor(x);
        }

        public static int CeilInt(decimal x)
        {
            return (int)Math.Ceiling(x);
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }

        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }
    }
}
