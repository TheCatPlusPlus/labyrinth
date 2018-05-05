using System;
using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Geometry;
using Labyrinth.Map;

namespace Labyrinth.Entities
{
	public abstract class Entity : IEquatable<Entity>
	{
		public EntityID ID { get; }

		public Int2? Position { get; private set; }
		[CanBeNull]
		public Level Level { get; private set; }

		[CanBeNull]
		private ILevelEntity LevelImpl => Level;

		protected Entity(EntityID id)
		{
			ID = id;
		}

		public void Move(Int2 position)
		{
			Debug.Assert(Position != null, "Position != null");
			Debug.Assert(LevelImpl != null, "LevelImpl != null");
			LevelImpl.Move(this, Position.Value, position);
			Position = position;
		}

		public void Spawn(Level level, Int2? position = null)
		{
			Despawn();
			Debug.Assert(LevelImpl != null, "LevelImpl != null");
			Position = LevelImpl.Spawn(this, position);
			Level = level;
		}

		public void Despawn()
		{
			LevelImpl?.Despawn(this);
			Level = null;
			Position = null;
		}

		public bool Equals([CanBeNull] Entity other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return ID.Equals(other.ID);
		}

		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return (obj.GetType() == GetType()) && Equals((Entity)obj);
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public static bool operator==(Entity left, Entity right)
		{
			return Equals(left, right);
		}

		public static bool operator!=(Entity left, Entity right)
		{
			return !Equals(left, right);
		}

		[NotNull]
		public override string ToString()
		{
			var type = GetType().Name;
			return Level == null
				? $"{type}: {ID}"
				: $"{type}: {ID} on {Level} at {Position}";
		}
	}
}
