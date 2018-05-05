using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using JetBrains.Annotations;

namespace Labyrinth.Geometry
{
	public struct Rect
	{
		public readonly Int2 Origin;
		public readonly Int2 Size;

		public int X => Origin.X;
		public int Y => Origin.Y;
		public int Width => Size.Width;
		public int Height => Size.Height;

		public int Left => X;
		public int Right => X + Width;
		public int Top => Y;
		public int Bottom => Y + Height;

		public Int2 TopLeft => new Int2(Left, Top);
		public Int2 TopRight => new Int2(Right, Top);
		public Int2 BottomLeft => new Int2(Left, Bottom);
		public Int2 BottomRight => new Int2(Right, Bottom);

		[NotNull]
		public IEnumerable<Int2> Points => GetPoints();
		[NotNull]
		public IEnumerable<Int2> RelativePoints => GetRelativePoints();
		[NotNull]
		public IEnumerable<Int2> EdgePoints => GetEdgePoints();

		public Rect(int x, int y, int width, int height)
			: this(new Int2(x, y), new Int2(width, height))
		{
		}

		public Rect(int x, int y, Int2 size)
			: this(new Int2(x, y), size)
		{
		}

		public Rect(Int2 origin, int width, int height)
			: this(origin, new Int2(width, height))
		{
		}

		public Rect(Int2 origin, Int2 size)
		{
			Origin = origin;
			Size = size;
		}

		public Rect(Rectangle other)
			: this((Int2)other.Location, (Int2)other.Size)
		{
		}

		public static explicit operator Rect(Rectangle other)
		{
			return new Rect(other);
		}

		public static implicit operator Rectangle(Rect other)
		{
			return new Rectangle(other.Origin, other.Size);
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
			return Inflated(new Int2(x, y));
		}

		[Pure]
		public Rect Inflated(Int2 size)
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
			return Contains(new Int2(x, y));
		}

		[Pure]
		public bool Contains(Int2 point)
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
		private IEnumerable<Int2> GetPoints()
		{
			var origin = Origin;
			return
				from point in GetRelativePoints()
				select point + origin;
		}

		[NotNull]
		[Pure]
		private IEnumerable<Int2> GetRelativePoints()
		{
			for (var x = 0; x < Width; ++x)
			for (var y = 0; y < Height; ++y)
			{
				yield return new Int2(x, y);
			}
		}

		[NotNull]
		[Pure]
		private IEnumerable<Int2> GetEdgePoints()
		{
			if ((Width > 1) && (Height > 1))
			{
				for (var x = Left; x < Right; ++x)
				{
					yield return new Int2(x, Top);
					yield return new Int2(x, Bottom - 1);
				}

				for (var y = Top; y < Bottom; ++y)
				{
					yield return new Int2(Left, y);
					yield return new Int2(Right - 1, y);
				}
			}
			else if ((Width == 1) && (Height == 1))
			{
				yield return new Int2(Left, Top);
			}
			else if ((Width > 1) && (Height == 1))
			{
				for (var x = Left; x < Right; ++x)
				{
					yield return new Int2(x, Top);
				}
			}
			else if ((Width == 1) && (Height > 1))
			{
				for (var y = Top; y < Bottom; ++y)
				{
					yield return new Int2(Left, y);
				}
			}
		}

		public override string ToString()
		{
			return $"Rect({X}, {Y}, {Width}, {Height})";
		}
	}
}
