using Labyrinth.Map;
using Labyrinth.Utils;

namespace Labyrinth.Database
{
	public sealed class TileData
	{
		public Name Name { get; set; }
		public TileFlag Flags { get; set; }
		public int CostMultiplier { get; set; } = 1;
		public GlyphData Glyph { get; set; } = new GlyphData('\uFFFC');

		public TileData(
			string singular,
			string plural = "",
			string article = "",
			bool countable = true,
			bool unique = false)
		{
			Name = new Name(singular, plural, article, countable, unique, false, true);
		}
	}
}
