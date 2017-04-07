using System.Drawing;

namespace Labyrinth.Utils
{
    public static class PointExt
    {
        public static readonly Point Invalid = new Point(-1, -1);

        public static bool IsInvalid(this Point point)
        {
            return point == Invalid;
        }
    }
}