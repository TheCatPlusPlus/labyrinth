using System;
using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Entities;
using Labyrinth.Geometry;
using Labyrinth.Utils;

namespace Labyrinth.Map
{
	public sealed class Level : ILevelEntity
	{
		private readonly Game _game;

		public string Name { get; }
		public int Width { get; }
		public int Height { get; }
		public int Depth { get; }

		public Scheduler Scheduler { get; }
		public Grid Grid { get; }

		public Level(Game game, string name, int width, int height, int depth)
		{
			_game = game;
			Name = name;
			Width = width;
			Height = height;
			Depth = depth;
			Scheduler = new Scheduler();
			Grid = new Grid(this, width, height);
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

		public Int2 FindSpawnPoint(TileFlag flags = TileFlag.SpawnCandidate | TileFlag.Walkable)
		{
			var attempts = 5000;
			while (attempts-- > 0)
			{
				var x = _game.RNG.NextIntRange(Width);
				var y = _game.RNG.NextIntRange(Height);
				var p = new Int2(x, y);
				var tile = Grid[p];

				Debug.Assert(tile != null, "tile != null");
				if (tile.EffectiveFlags.Contains(flags))
				{
					return p;
				}
			}

			throw new InvalidOperationException("Could not find a suitable spawn point");
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
