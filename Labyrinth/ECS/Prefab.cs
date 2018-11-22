using System;

using JetBrains.Annotations;

namespace Labyrinth.ECS
{
	public struct Prefab : IEquatable<Prefab>
	{
		public readonly string Name;

		public Prefab(string name)
		{
			Name = name;
		}

		public bool Equals(Prefab other)
		{
			return string.Equals(Name, other.Name);
		}

		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is Prefab other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Name?.GetHashCode() ?? 0;
		}

		public static bool operator==(Prefab left, Prefab right)
		{
			return left.Equals(right);
		}

		public static bool operator!=(Prefab left, Prefab right)
		{
			return !left.Equals(right);
		}

		[NotNull]
		public override string ToString()
		{
			return $"{Name}";
		}
	}
}
