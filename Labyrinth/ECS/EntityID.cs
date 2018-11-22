using System;

using JetBrains.Annotations;

namespace Labyrinth.ECS
{
	public struct EntityID : IEquatable<EntityID>
	{
		public readonly int Value;

		public EntityID(int value)
		{
			Value = value;
		}

		public bool Equals(EntityID other)
		{
			return Value == other.Value;
		}

		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is EntityID other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Value;
		}

		public static bool operator==(EntityID left, EntityID right)
		{
			return left.Equals(right);
		}

		public static bool operator!=(EntityID left, EntityID right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"#{Value}";
		}
	}
}
