using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Labyrinth.Utils.Geometry
{
    public struct Rect
    {
        public readonly Vector2I Origin;
        public readonly Vector2I Size;

        public int X => Origin.X;
        public int Y => Origin.Y;
        public int Width => Size.Width;
        public int Height => Size.Height;

        public int Left => X;
        public int Right => X + Width;
        public int Top => Y;
        public int Bottom => Y + Height;

        public Vector2I TopLeft => new Vector2I(Left, Top);
        public Vector2I TopRight => new Vector2I(Right, Top);
        public Vector2I BottomLeft => new Vector2I(Left, Bottom);
        public Vector2I BottomRight => new Vector2I(Right, Bottom);

        [NotNull]
        public IEnumerable<Vector2I> Points => GetPoints();
        [NotNull]
        public IEnumerable<Vector2I> RelativePoints => GetRelativePoints();
        [NotNull]
        public IEnumerable<Vector2I> EdgePoints => GetEdgePoints();

        public Rect(int x, int y, int width, int height)
            : this(new Vector2I(x, y), new Vector2I(width, height))
        {
        }

        public Rect(int x, int y, Vector2I size)
            : this(new Vector2I(x, y), size)
        {
        }

        public Rect(Vector2I origin, int width, int height)
            : this(origin, new Vector2I(width, height))
        {
        }

        public Rect(Vector2I origin, Vector2I size)
        {
            Origin = origin;
            Size = size;
        }

        [Pure]
        public int DistanceTo(Rect other)
        {
            var vertical = -1;
            var horizontal = -1;

            if (Top >= other.Bottom)
            {
                vertical = Top - other.Bottom;
            }
            else if (other.Top >= Bottom)
            {
                vertical = other.Top - Bottom;
            }

            if (Left >= other.Right)
            {
                horizontal = Left - other.Right;
            }
            else if (other.Left >= Right)
            {
                horizontal = other.Left - Right;
            }

            if ((vertical == -1) && (horizontal == -1))
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

        [Pure]
        public Rect Inflated(int size)
        {
            return Inflated(size, size);
        }

        [Pure]
        public Rect Inflated(int x, int y)
        {
            return Inflated(new Vector2I(x, y));
        }

        [Pure]
        public Rect Inflated(Vector2I size)
        {
            return new Rect(
                X - size.Width,
                Y - size.Height,
                Width + size.Width * 2,
                Height + size.Height * 2
            );
        }

        [Pure]
        public bool Contains(int x, int y)
        {
            return Contains(new Vector2I(x, y));
        }

        [Pure]
        public bool Contains(Vector2I point)
        {
            return (point.X >= Left) && (point.X < Right)
                   && (point.Y >= Top) && (point.Y < Bottom);
        }

        [Pure]
        public bool Overlaps(Rect other)
        {
            return DistanceTo(other) <= 0;
        }

        [NotNull]
        [Pure]
        private IEnumerable<Vector2I> GetPoints()
        {
            var origin = Origin;
            return
                from point in GetRelativePoints()
                select point + origin;
        }

        [NotNull]
        [Pure]
        private IEnumerable<Vector2I> GetRelativePoints()
        {
            for (var x = 0; x < Width; ++x)
            for (var y = 0; y < Height; ++y)
            {
                yield return new Vector2I(x, y);
            }
        }

        [NotNull]
        [Pure]
        private IEnumerable<Vector2I> GetEdgePoints()
        {
            if ((Width > 1) && (Height > 1))
            {
                for (var x = Left; x < Right; ++x)
                {
                    yield return new Vector2I(x, Top);
                    yield return new Vector2I(x, Bottom - 1);
                }

                for (var y = Top; y < Bottom; ++y)
                {
                    yield return new Vector2I(Left, y);
                    yield return new Vector2I(Right - 1, y);
                }
            }
            else if ((Width == 1) && (Height == 1))
            {
                yield return new Vector2I(Left, Top);
            }
            else if ((Width > 1) && (Height == 1))
            {
                for (var x = Left; x < Right; ++x)
                {
                    yield return new Vector2I(x, Top);
                }
            }
            else if ((Width == 1) && (Height > 1))
            {
                for (var y = Top; y < Bottom; ++y)
                {
                    yield return new Vector2I(Left, y);
                }
            }
        }

        public override string ToString()
        {
            return $"Rect({X}, {Y}, {Width}, {Height})";
        }
    }
}
