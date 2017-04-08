using System;

using JetBrains.Annotations;

namespace Labyrinth.Utils.Geometry
{
    public struct Vector2I
    {
        public static readonly Vector2I Zero = new Vector2I(0, 0);

        public static readonly Vector2I Up = new Vector2I(0, -1);
        public static readonly Vector2I Down = new Vector2I(0, 1);
        public static readonly Vector2I Left = new Vector2I(-1, 0);
        public static readonly Vector2I Right = new Vector2I(1, 0);

        public static readonly Vector2I UpLeft = Up + Left;
        public static readonly Vector2I UpRight = Up + Right;
        public static readonly Vector2I DownLeft = Down + Left;
        public static readonly Vector2I DownRight = Down + Right;

        public static readonly Vector2I North = Up;
        public static readonly Vector2I South = Down;
        public static readonly Vector2I West = Left;
        public static readonly Vector2I East = Right;

        public static readonly Vector2I NorthWest = North + West;
        public static readonly Vector2I NorthEast = North + East;
        public static readonly Vector2I SouthWest = South + West;
        public static readonly Vector2I SouthEast = South + East;

        public readonly int X;
        public readonly int Y;

        public int Width => X;
        public int Height => Y;

        public Vector2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2I(Vector2F other)
            : this(MathExt.RoundInt(other.X), MathExt.RoundInt(other.Y))
        {
        }

        public static explicit operator Vector2I(Vector2F other)
        {
            return new Vector2I(other);
        }

        [Pure]
        public bool Equals(Vector2I other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Vector2I other && Equals(other);
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
        public static bool operator==(Vector2I left, Vector2I right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator!=(Vector2I left, Vector2I right)
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
        public float NormL2()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        [Pure]
        public Vector2I Normalize()
        {
            var v2F = (Vector2F)this;
            v2F = v2F.Normalize();
            return (Vector2I)v2F;
        }

        [Pure]
        public Vector2I Abs()
        {
            return new Vector2I(Math.Abs(X), Math.Abs(Y));
        }

        [Pure]
        public Vector2I Swap()
        {
            return new Vector2I(Y, X);
        }

        [Pure]
        public static Vector2I operator+(Vector2I left, Vector2I right)
        {
            return new Vector2I(left.X + right.X, left.Y + right.Y);
        }

        [Pure]
        public static Vector2I operator-(Vector2I left, Vector2I right)
        {
            return left + -right;
        }

        [Pure]
        public static Vector2I operator-(Vector2I self)
        {
            return new Vector2I(-self.X, -self.Y);
        }

        [Pure]
        public static Vector2I operator*(Vector2I self, int scale)
        {
            return new Vector2I(self.X * scale, self.Y * scale);
        }

        [Pure]
        public static Vector2I operator*(Vector2I self, float scale)
        {
            return new Vector2I(MathExt.RoundInt(self.X * scale), MathExt.RoundInt(self.Y * scale));
        }

        [Pure]
        public static Vector2I operator/(Vector2I self, int scale)
        {
            return new Vector2I(self.X / scale, self.Y / scale);
        }

        [Pure]
        public static Vector2I operator/(Vector2I self, float scale)
        {
            return new Vector2I(MathExt.RoundInt(self.X / scale), MathExt.RoundInt(self.Y / scale));
        }
    }
}
