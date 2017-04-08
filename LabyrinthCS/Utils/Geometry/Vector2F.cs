using System;

using JetBrains.Annotations;

namespace Labyrinth.Utils.Geometry
{
    public struct Vector2F
    {
        public static readonly Vector2F Zero = new Vector2F(0, 0);

        public static readonly Vector2F Up = new Vector2F(0, -1);
        public static readonly Vector2F Down = new Vector2F(0, 1);
        public static readonly Vector2F Left = new Vector2F(-1, 0);
        public static readonly Vector2F Right = new Vector2F(1, 0);

        public static readonly Vector2F UpLeft = Up + Left;
        public static readonly Vector2F UpRight = Up + Right;
        public static readonly Vector2F DownLeft = Down + Left;
        public static readonly Vector2F DownRight = Down + Right;

        public static readonly Vector2F North = Up;
        public static readonly Vector2F South = Down;
        public static readonly Vector2F West = Left;
        public static readonly Vector2F East = Right;

        public static readonly Vector2F NorthWest = North + West;
        public static readonly Vector2F NorthEast = North + East;
        public static readonly Vector2F SouthWest = South + West;
        public static readonly Vector2F SouthEast = South + East;

        public readonly float X;
        public readonly float Y;

        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2F(Vector2I other)
            : this(other.X, other.Y)
        {
        }

        public static explicit operator Vector2F(Vector2I other)
        {
            return new Vector2F(other);
        }

        [Pure]
        public bool Equals(Vector2F other)
        {
            return MathExt.ApproxEquals(X, other.X) && MathExt.ApproxEquals(Y, other.Y);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Vector2F && Equals((Vector2F)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        [Pure]
        public static bool operator==(Vector2F left, Vector2F right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator!=(Vector2F left, Vector2F right)
        {
            return !left.Equals(right);
        }

        [Pure]
        public static bool operator==(Vector2F left, Vector2I right)
        {
            return left == (Vector2F)right;
        }

        [Pure]
        public static bool operator!=(Vector2F left, Vector2I right)
        {
            return left != (Vector2F)right;
        }

        [Pure]
        public static bool operator==(Vector2I left, Vector2F right)
        {
            return right == left;
        }

        [Pure]
        public static bool operator!=(Vector2I left, Vector2F right)
        {
            return right != left;
        }

        [Pure]
        public override string ToString()
        {
            return $"[{X:F3}, {Y:F3}]";
        }

        [Pure]
        public float NormL1()
        {
            return Math.Abs(X) + Math.Abs(Y);
        }

        [Pure]
        public float NormL2()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        [Pure]
        public Vector2F Normalize()
        {
            var length = NormL2();
            return new Vector2F(X / length, Y / length);
        }

        [Pure]
        public static Vector2F operator+(Vector2F left, Vector2F right)
        {
            return new Vector2F(left.X + right.X, left.Y + right.Y);
        }

        [Pure]
        public static Vector2F operator-(Vector2F left, Vector2F right)
        {
            return left + -right;
        }

        [Pure]
        public static Vector2F operator-(Vector2F self)
        {
            return new Vector2F(-self.X, -self.Y);
        }

        [Pure]
        public static Vector2F operator*(Vector2F self, int scale)
        {
            return self * (float)scale;
        }

        [Pure]
        public static Vector2F operator*(Vector2F self, float scale)
        {
            return new Vector2F(self.X * scale, self.Y * scale);
        }

        [Pure]
        public static Vector2F operator/(Vector2F self, int scale)
        {
            return new Vector2F(self.X / scale, self.Y / scale);
        }

        [Pure]
        public static Vector2F operator/(Vector2F self, float scale)
        {
            return new Vector2F(self.X / scale, self.Y / scale);
        }
    }
}
