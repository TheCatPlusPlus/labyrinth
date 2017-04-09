using System.Diagnostics;
using System.Drawing;

using JetBrains.Annotations;

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

        [NotNull]
        public static string ToHex(this Color color, bool withPrefix = true)
        {
            var prefix = withPrefix ? "#" : "";
            return $"{prefix}{color.A:X02}{color.R:X02}{color.G:X02}{color.B:X02}";
        }
    }
}
