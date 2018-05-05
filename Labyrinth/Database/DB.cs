namespace Labyrinth.Database
{
	public static class DB
	{
		public static EntityDB Entities { get; } = new EntityDB();
		public static GlyphDB Glyphs { get; } = new GlyphDB();
		public static TileDB Tiles { get; } = new TileDB();
	}
}
