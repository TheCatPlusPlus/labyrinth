using System;
using System.Diagnostics;
using System.Drawing;

using BearLib;

using Labyrinth.Utils;

namespace Labyrinth.UI
{
    public static class TerminalExt
    {
        public const char BoxUlCorner = '\u250C';
        public const char BoxUrCorner = '\u2510';
        public const char BoxBlCorner = '\u2514';
        public const char BoxBrCorner = '\u2518';
        public const char BoxUlCornerSoft = '\u256C';
        public const char BoxUrCornerSoft = '\u256D';
        public const char BoxBlCornerSoft = '\u2570';
        public const char BoxBrCornerSoft = '\u256F';
        public const char BoxVLine = '\u2502';
        public const char BoxHLine = '\u2500';
        public const char BoxVLineLSplit = '\u251C';
        public const char BoxVLineRSplit = '\u2524';
        public const char BoxHLineDSplit = '\u252C';
        public const char BoxHLineUSplit = '\u2534';
        public const char BoxCrossSplit = '\u253C';

        private static Guard ColorGuard(Color? color, int state, Action<Color> set)
        {
            var previous = Color.FromArgb(Terminal.State(state));
            if (color.HasValue)
            {
                set(color.Value);
            }
            return new Guard(() => set(previous));
        }

        public static Guard Background(Color? color)
        {
            return ColorGuard(color, Terminal.TK_BKCOLOR, Terminal.BkColor);
        }

        public static Guard Foreground(Color? color)
        {
            return ColorGuard(color, Terminal.TK_COLOR, Terminal.Color);
        }

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

        public static void Box(Point origin, Size size)
        {
            Box(new Rectangle(origin, size));
        }

        public static void Box(Rectangle rect)
        {
            var ul = new Point(rect.X, rect.Y);
            var ur = new Point(rect.Right, rect.Y);
            var bl = new Point(rect.X, rect.Bottom);
            var br = new Point(rect.Right, rect.Bottom);

            VLine(ul, rect.Height);
            VLine(ur, rect.Height);
            HLine(ul, rect.Width);
            HLine(bl, rect.Width);

            Terminal.Put(ul, BoxUlCorner);
            Terminal.Put(ur, BoxUrCorner);
            Terminal.Put(bl, BoxBlCorner);
            Terminal.Put(br, BoxBrCorner);
        }

        public static void VLine(Point point, int height, char ch = BoxVLine)
        {
            for (var y = 0; y < height; ++y)
            {
                Terminal.Put(point.X, point.Y + y, ch);
            }
        }

        public static void HLine(Point point, int width, char ch = BoxHLine)
        {
            for (var x = 0; x < width; ++x)
            {
                Terminal.Put(point.X + x, point.Y, ch);
            }
        }

        public static void HBar(Point point, int width, Color? color)
        {
            using (Background(color))
            {
                HLine(point, width, ' ');
            }
        }

        public static Guard Composition(bool enabled = true)
        {
            var current = Terminal.State(Terminal.TK_COMPOSITION) != 0;
            Terminal.Composition(enabled);
            return new Guard(() => Terminal.Composition(current));
        }

        public static Guard Layer(int layer)
        {
            Debug.Assert(layer.Within(0, 256));
            var current = Terminal.State(Terminal.TK_LAYER);
            Terminal.Layer(layer);
            return new Guard(() => Terminal.Layer(current));
        }
    }
}
