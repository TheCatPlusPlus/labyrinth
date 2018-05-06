using Labyrinth.Utils;

namespace Labyrinth.Database
{
	public abstract class EntityData
	{
		public Name Name { get; set; }
		public GlyphData Glyph { get; set; } = new GlyphData('\uFFFC');

		protected EntityData(
			string singular,
			string plural = "",
			string article = "",
			bool countable = true,
			bool unique = false,
			bool proper = false,
			bool thing = false)
		{
			Name = new Name(singular, plural, article, countable, unique, proper, thing);
		}
	}
}
