namespace Labyrinth.Utils
{
    public static class Conditions
    {
        public static bool Within(this int value, int min, int max)
        {
            return value >= min && value < max;
        }

        public static bool Within(this float value, float min, float max)
        {
            return value >= min && value < max;
        }

        public static bool Within(this double value, double min, double max)
        {
            return value >= min && value < max;
        }

        public static bool WithinInclusive(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static bool WithinInclusive(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public static bool WithinInclusive(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }
    }
}
