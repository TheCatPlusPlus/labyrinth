using System.Diagnostics;
using System.Drawing;

namespace Labyrinth.Utils
{
    public static class ColorExt
    {
        public static Color Lighten(this Color color, float factor)
        {
            Debug.Assert(factor.WithinInclusive(0, 1));
            var r = (255 - color.R) * factor + color.R;
            var g = (255 - color.G) * factor + color.G;
            var b = (255 - color.B) * factor + color.B;
            return Color.FromArgb(color.A, (int)r, (int)g, (int)b);
        }

        public static Color Darken(this Color color, float factor)
        {
            Debug.Assert(factor.WithinInclusive(0, 1));
            factor = 1 - factor;
            var r = color.R * factor;
            var g = color.G * factor;
            var b = color.B * factor;
            return Color.FromArgb(color.A, (int)r, (int)g, (int)b);
        }
    }
}
