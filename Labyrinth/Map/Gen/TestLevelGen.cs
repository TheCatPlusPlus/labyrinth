using Labyrinth.Geometry;

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
		}
	}
}
