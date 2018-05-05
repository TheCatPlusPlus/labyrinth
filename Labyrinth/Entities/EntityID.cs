using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Labyrinth.Entities
{
	public struct EntityID : IEquatable<EntityID>
	{
		public const string Creatures = "Creatures/";
		public static readonly string Mobs = $"{Creatures}/Mobs/";
		public const string Items = "Items/";

		private readonly string _value;

		public EntityID(string value)
		{
			_value = value;
		}

		[Conditional("DEBUG")]
		public void RequireNamespace(string prefix)
		{
			if (!_value.StartsWith(prefix))
			{
				throw new Exception($"{this}: should have prefix {prefix}");
			}
		}

		public bool Equals(EntityID other)
		{
			return string.Equals(_value, other._value);
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
			return _value.GetHashCode();
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
			return $"EntityID({_value})";
		}
	}
}
