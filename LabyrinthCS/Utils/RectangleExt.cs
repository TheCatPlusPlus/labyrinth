using System.Collections.Generic;
using System.Drawing;

namespace Labyrinth.Utils
{
    public static class RectangleExt
    {
        public static IEnumerable<Point> Points(this Rectangle rect)
        {
            for (var x = 0; x < rect.Width; ++x)
            {
                for (var y = 0; y < rect.Height; ++y)
                {
                    yield return new Point(rect.Left + x, rect.Top + y);
                }
            }
        }

        public static IEnumerable<Point> RelativePoints(this Rectangle rect)
        {
            for (var x = 0; x < rect.Width; ++x)
            {
                for (var y = 0; y < rect.Height; ++y)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public static IEnumerable<Point> EdgePoints(this Rectangle rect)
        {
            if (rect.Width > 1 && rect.Height > 1)
            {
                for (var x = rect.Left; x < rect.Right; ++x)
                {
                    yield return new Point(x, rect.Top);
                    yield return new Point(x, rect.Bottom - 1);
                }

                for (var y = rect.Top; y < rect.Bottom; ++y)
                {
                    yield return new Point(rect.Left, y);
                    yield return new Point(rect.Right - 1, y);
                }
            }
            else if (rect.Width == 1 && rect.Height == 1)
            {
                yield return new Point(rect.Left, rect.Top);
            }
            else if (rect.Width > 1 && rect.Height == 1)
            {
                for (var x = rect.Left; x < rect.Right; ++x)
                {
                    yield return new Point(x, rect.Top);
                }
            }
            else if (rect.Width == 1 && rect.Height > 1)
            {
                for (var y = rect.Top; y < rect.Bottom; ++y)
                {
                    yield return new Point(rect.Left, y);
                }
            }
        }

        public static int DistanceTo(this Rectangle self, Rectangle other)
        {
            var vertical = -1;
            var horizontal = -1;

            if (self.Top >= other.Bottom)
            {
                vertical = self.Top - other.Bottom;
            }
            else if (other.Top >= self.Bottom)
            {
                vertical = other.Top - self.Bottom;
            }

            if (self.Left >= other.Right)
            {
                horizontal = self.Left - other.Right;
            }
            else if (other.Left >= self.Right)
            {
                horizontal = other.Left - self.Right;
            }

            if (vertical == -1 && horizontal == -1)
            {
                return -1;
            }

            if (vertical == -1)
            {
                return horizontal;
            }

            if (horizontal == -1)
            {
                return vertical;
            }

            return horizontal + vertical;
        }

        public static Rectangle Inflated(this Rectangle rect, int x, int y)
        {
            return Rectangle.Inflate(rect, x, y);
        }

        public static Rectangle Inflated(this Rectangle rect, Size size)
        {
            return rect.Inflated(size.Width, size.Height);
        }
    }
}
