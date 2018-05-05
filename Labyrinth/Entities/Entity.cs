using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Geometry;
using Labyrinth.Map;

namespace Labyrinth.Entities
{
	public abstract class Entity
	{
		public EntityID ID { get; }

		public Int2? Position { get; private set; }
		[CanBeNull]
		public Level Level { get; private set; }

		[CanBeNull]
		private ILevelEntity LevelImpl => Level;

		protected Game Game { get; }

		protected Entity(Game game, EntityID id)
		{
			ID = id;
			Game = game;
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
			var impl = (ILevelEntity)level;
			Position = impl.Spawn(this, position);
			Level = level;
		}

		public void Despawn()
		{
			LevelImpl?.Despawn(this);
			Level = null;
			Position = null;
		}

		[NotNull]
		public override string ToString()
		{
			var desc = $"{ID}";

			if ((Level != null) && (Position != null))
			{
				desc = $"{desc} (on {Level} at {Position})";
			}

			return desc;
		}
	}
}
