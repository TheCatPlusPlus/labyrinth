using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Entities;
using Labyrinth.Geometry;

namespace Labyrinth.Map
{
	public sealed class Level : ILevelEntity
	{
		public string Name { get; }
		public int Width { get; }
		public int Height { get; }

		public Scheduler Scheduler { get; }
		public Grid Grid { get; }

		public Level(string name, int width, int height)
		{
			Name = name;
			Width = width;
			Height = height;
			Scheduler = new Scheduler();
			Grid = new Grid(width, height);
		}

		Int2 ILevelEntity.Spawn(Entity entity, Int2? target)
		{
			Debug.Assert(entity.Position == null, "entity.Position == null");
			Debug.Assert(entity.Level == null, "entity.Level == null");

			var position = target ?? FindSpawnPoint();
			Debug.Assert(Grid[position] != null, "Grid[position] != null");
			Grid[position].Add(entity);

			if (entity is Creature actor)
			{
				Scheduler.Add(actor);
			}

			return position;
		}

		void ILevelEntity.Move(Entity entity, Int2 from, Int2 to)
		{
			Debug.Assert(Grid[from] != null, "Grid[from] != null");
			Debug.Assert(Grid[to] != null, "Grid[to] != null");

			Grid[from].Remove(entity);
			Grid[to].Add(entity);
		}

		void ILevelEntity.Despawn(Entity entity)
		{
			Debug.Assert(entity.Position != null, "entity.Position != null");
			Debug.Assert(entity.Level == this, "entity.Level == this");
			Debug.Assert(Grid[entity.Position.Value] != null, "Grid[entity.Position.Value] != null");

			Grid[entity.Position.Value].Remove(entity);

			if (entity is Creature actor)
			{
				Scheduler.Remove(actor);
			}
		}

		private Int2 FindSpawnPoint()
		{
			return new Int2();
		}

		public void Tick(float dt)
		{
			Scheduler.Advance();
			// TODO timers
		}

		[NotNull]
		public override string ToString()
		{
			return $"{Name}({Width}, {Height})";
		}
	}
}
