using System;
using System.Collections.Generic;
using System.Drawing;

using JetBrains.Annotations;

using Labyrinth.Utils;

namespace Labyrinth.Geometry
{
	public struct Int2
	{
		public static readonly Int2 Zero = new Int2(0, 0);

		public static readonly Int2 Up = new Int2(0, -1);
		public static readonly Int2 Down = new Int2(0, 1);
		public static readonly Int2 Left = new Int2(-1, 0);
		public static readonly Int2 Right = new Int2(1, 0);

		public static readonly Int2 UpLeft = Up + Left;
		public static readonly Int2 UpRight = Up + Right;
		public static readonly Int2 DownLeft = Down + Left;
		public static readonly Int2 DownRight = Down + Right;

		public static readonly Int2 North = Up;
		public static readonly Int2 South = Down;
		public static readonly Int2 West = Left;
		public static readonly Int2 East = Right;

		public static readonly Int2 NorthWest = North + West;
		public static readonly Int2 NorthEast = North + East;
		public static readonly Int2 SouthWest = South + West;
		public static readonly Int2 SouthEast = South + East;

		public static readonly Dictionary<Direction, Int2> Movement = new Dictionary<Direction, Int2>
		{
			{ Direction.North, North },
			{ Direction.South, South },
			{ Direction.West, West },
			{ Direction.East, East },
			{ Direction.NorthEast, NorthEast },
			{ Direction.NorthWest, NorthWest },
			{ Direction.SouthEast, SouthEast },
			{ Direction.SouthWest, SouthWest }
		};

		public readonly int X;
		public readonly int Y;

		public int Width => X;
		public int Height => Y;

		public Int2(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Int2(Point other)
		{
			X = other.X;
			Y = other.Y;
		}

		public Int2(Size other)
		{
			X = other.Width;
			Y = other.Height;
		}

		public static explicit operator Int2(Point other)
		{
			return new Int2(other);
		}

		public static explicit operator Int2(Size other)
		{
			return new Int2(other);
		}

		public static implicit operator Point(Int2 other)
		{
			return new Point(other.X, other.Y);
		}

		public static implicit operator Size(Int2 other)
		{
			return new Size(other.X, other.Y);
		}

		[Pure]
		public bool Equals(Int2 other)
		{
			return (X == other.X) && (Y == other.Y);
		}

		[Pure]
		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is Int2 other && Equals(other);
		}

		[Pure]
		public override int GetHashCode()
		{
			unchecked
			{
				return (X * 397) ^ Y;
			}
		}

		[Pure]
		public static bool operator==(Int2 left, Int2 right)
		{
			return left.Equals(right);
		}

		[Pure]
		public static bool operator!=(Int2 left, Int2 right)
		{
			return !left.Equals(right);
		}

		[Pure]
		public override string ToString()
		{
			return $"[{X}, {Y}]";
		}

		[Pure]
		public int NormL1()
		{
			var abs = Abs();
			return abs.X + abs.Y;
		}

		[Pure]
		public Int2 Abs()
		{
			return new Int2(Math.Abs(X), Math.Abs(Y));
		}

		[Pure]
		public Int2 Swap()
		{
			return new Int2(Y, X);
		}

		[Pure]
		public static Int2 operator+(Int2 left, Int2 right)
		{
			return new Int2(left.X + right.X, left.Y + right.Y);
		}

		[Pure]
		public static Int2 operator-(Int2 left, Int2 right)
		{
			return left + -right;
		}

		[Pure]
		public static Int2 operator-(Int2 self)
		{
			return new Int2(-self.X, -self.Y);
		}

		[Pure]
		public static Int2 operator*(Int2 self, int scale)
		{
			return new Int2(self.X * scale, self.Y * scale);
		}

		[Pure]
		public static Int2 operator*(Int2 self, decimal scale)
		{
			return new Int2(MathExt.RoundInt(self.X * scale), MathExt.RoundInt(self.Y * scale));
		}

		[Pure]
		public static Int2 operator/(Int2 self, int scale)
		{
			return new Int2(self.X / scale, self.Y / scale);
		}

		[Pure]
		public static Int2 operator/(Int2 self, decimal scale)
		{
			return new Int2(MathExt.RoundInt(self.X / scale), MathExt.RoundInt(self.Y / scale));
		}
	}
}
