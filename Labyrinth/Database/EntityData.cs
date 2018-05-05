using Labyrinth.Utils;

namespace Labyrinth.Database
{
	public sealed class EntityData
	{
		public Name Name { get; set; }

		public EntityData(
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
