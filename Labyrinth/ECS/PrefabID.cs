using System;

using JetBrains.Annotations;

namespace Labyrinth.ECS
{
	public struct PrefabID : IEquatable<PrefabID>
	{
		public readonly string Name;

		public PrefabID(string name)
		{
			Name = name;
		}

		public bool Equals(PrefabID other)
		{
			return string.Equals(Name, other.Name);
		}

		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is PrefabID other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Name?.GetHashCode() ?? 0;
		}

		public static bool operator==(PrefabID left, PrefabID right)
		{
			return left.Equals(right);
		}

		public static bool operator!=(PrefabID left, PrefabID right)
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
