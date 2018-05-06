using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Labyrinth.Database
{
	public struct EntityID : IEquatable<EntityID>
	{
		public const string Creatures = "Creatures";
		public static readonly string Mobs = $"{Creatures}/Mobs";
		public const string Items = "Items";

		public string Value { get; }

		public EntityID(string value)
		{
			Value = value;
		}

		[Conditional("DEBUG")]
		public void RequireNamespace(string prefix)
		{
			if (!Value.StartsWith(prefix))
			{
				throw new Exception($"{this}: should have prefix {prefix}");
			}
		}

		public bool Equals(EntityID other)
		{
			return string.Equals(Value, other.Value);
		}

		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is EntityID id && Equals(id);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static bool operator==(EntityID left, EntityID right)
		{
			return left.Equals(right);
		}

		public static bool operator!=(EntityID left, EntityID right)
		{
			return !left.Equals(right);
		}

		[NotNull]
		public override string ToString()
		{
			return $"<{Value}>";
		}
	}
}
