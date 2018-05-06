using Labyrinth.Geometry;
using Labyrinth.Utils;

namespace Labyrinth.Map.Gen
{
	public sealed class TestLevelGen : LevelGen
	{
		public TestLevelGen(Game game)
			: base(game)
		{
		}

		protected override void Fill(LevelBuilder builder)
		{
			const int margin = 5;

			var room = new Rect(margin, margin, builder.Width - margin * 2, builder.Height - margin * 2);

			builder.Fill(TileType.Wall);
			builder.Box(TileType.Floor, room);

			var types = new[]
			{
				TileType.Wall,
				TileType.DoorOpen,
				TileType.DoorClosed,
				TileType.Water,
				TileType.DeepWater,
				TileType.Lava,
				TileType.GlassWall,
				TileType.StairsUp,
				TileType.StairsDown
			};

			for (var idx = 0; idx < 50; ++idx)
			{
				var tile = builder.Level.FindSpawnTile(TileFlag.Walkable);
				tile.Type = Game.RNG.Pick(types);
			}
		}
	}
}
