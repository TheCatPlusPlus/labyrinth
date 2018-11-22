using System.Linq;

namespace Labyrinth.Map.Gen
{
	public abstract class LevelGen
	{
		protected GamePrev Game { get; }

		protected LevelGen(GamePrev game)
		{
			Game = game;
		}

		public Level Create(string name, int width, int height, int depth)
		{
			width |= 1;
			height |= 1;

			var level = new Level(Game, name, width, height, depth);
			var builder = new LevelBuilder(level);
			Fill(builder);

			void FixWalls(Tile tile)
			{
				// every wall that's surrounded by other walls should be a deep wall
				if (tile.Type.IsOpaqueWall() && tile.Neighbours.All(t => t.Type.IsOpaqueWall()))
				{
					tile.Type = TileType.DeepWall;
				}
			}

			builder.ForEach(FixWalls);
			return level;
		}

		protected abstract void Fill(LevelBuilder builder);
	}
}
