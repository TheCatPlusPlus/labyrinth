using System;
using System.Diagnostics;
using System.Drawing;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI
{
    public static class TerminalExt
    {
        public const char BoxTlCorner = '\u250C';
        public const char BoxTrCorner = '\u2510';
        public const char BoxBlCorner = '\u2514';
        public const char BoxBrCorner = '\u2518';
        public const char BoxTlCornerSoft = '\u256C';
        public const char BoxTrCornerSoft = '\u256D';
        public const char BoxBlCornerSoft = '\u2570';
        public const char BoxBrCornerSoft = '\u256F';
        public const char BoxVLine = '\u2502';
        public const char BoxHLine = '\u2500';
        public const char BoxVLineLSplit = '\u251C';
        public const char BoxVLineRSplit = '\u2524';
        public const char BoxHLineDSplit = '\u252C';
        public const char BoxHLineUSplit = '\u2534';
        public const char BoxCrossSplit = '\u253C';

        [NotNull]
        private static Guard ColorGuard(Color? color, int state, Action<Color> set)
        {
            var previous = Color.FromArgb(Terminal.State(state));
            if (color.HasValue)
            {
                set(color.Value);
            }
            return new Guard(() => set(previous));
        }

        [NotNull]
        public static Guard Background(Color? color)
        {
            return ColorGuard(color, Terminal.TK_BKCOLOR, Terminal.BkColor);
        }

        [NotNull]
        public static Guard Foreground(Color? color)
        {
            return ColorGuard(color, Terminal.TK_COLOR, Terminal.Color);
        }

        [NotNull]
        public static Guard Colors(Color? fg, Color? bg)
        {
            var fgGuard = Foreground(fg);
            var bgGuard = Background(bg);
            return new Guard(
                () =>
                {
                    bgGuard.Dispose();
                    fgGuard.Dispose();
                });
        }

        public static void Box(Vector2I origin, Vector2I size)
        {
            Box(new Rect(origin, size));
        }

        public static void Box(Rect rect)
        {
            VLine(rect.TopLeft, rect.Height);
            VLine(rect.TopRight, rect.Height);
            HLine(rect.BottomLeft, rect.Width);
            HLine(rect.BottomRight, rect.Width);

            Terminal.Put(rect.TopLeft, BoxTlCorner);
            Terminal.Put(rect.TopRight, BoxTrCorner);
            Terminal.Put(rect.BottomLeft, BoxBlCorner);
            Terminal.Put(rect.BottomRight, BoxBrCorner);
        }

        public static void VLine(Vector2I point, int height, char ch = BoxVLine)
        {
            for (var y = 0; y < height; ++y)
            {
                Terminal.Put(point.X, point.Y + y, ch);
            }
        }

        public static void HLine(Vector2I point, int width, char ch = BoxHLine)
        {
            for (var x = 0; x < width; ++x)
            {
                Terminal.Put(point.X + x, point.Y, ch);
            }
        }

        public static void HBar(Vector2I point, int width, Color? color)
        {
            using (Background(color))
            {
                HLine(point, width, ' ');
            }
        }

        [NotNull]
        public static Guard Composition(bool enabled = true)
        {
            var current = Terminal.State(Terminal.TK_COMPOSITION) != 0;
            Terminal.Composition(enabled);
            return new Guard(() => Terminal.Composition(current));
        }

        [NotNull]
        public static Guard Layer(int layer)
        {
            Debug.Assert(layer.Within(0, 256));
            var current = Terminal.State(Terminal.TK_LAYER);
            Terminal.Layer(layer);
            return new Guard(() => Terminal.Layer(current));
        }

        public static Guard Crop(Rect rect)
        {
            Terminal.Crop(rect);
            return new Guard(() => Terminal.Crop(0, 0, 0, 0));
        }

        public static void Fill(Rect rect, char ch)
        {
            foreach (var point in rect.Points)
            {
                Terminal.Put(point, ch);
            }
        }
    }
}
