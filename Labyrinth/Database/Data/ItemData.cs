namespace Labyrinth.Database
{
	public sealed class ItemData : EntityData
	{
		public ItemData(
			string singular, string plural = "", string article = "", bool countable = true, bool unique = false,
			bool proper = false, bool thing = false)
			: base(singular, plural, article, countable, unique, proper, thing)
		{
		}
	}
}
