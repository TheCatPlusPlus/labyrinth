using Labyrinth.Entities;
using Labyrinth.Entities.Damage;
using Labyrinth.Utils;

namespace Labyrinth.Database
{
	public sealed class CreatureData : EntityData
	{
		public DamageType MeleeDamageType { get;set; }
		public Die MeleeDamage { get; set; }
		public int HP { get; set; }
		public int Speed { get; set; } = Scheduler.BaseSpeed;

		public CreatureData(
			string singular, string plural = "", string article = "", bool countable = true, bool unique = false,
			bool proper = false, bool thing = false)
			: base(singular, plural, article, countable, unique, proper, thing)
		{
		}
	}
}
