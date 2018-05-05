using Labyrinth.Utils;

namespace Labyrinth.Entities
{
	public sealed class Mob : Creature
	{
		public Mob(Game game, EntityID id, Name name, string desc, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(game, id, name, desc, maxHP, speed)
		{
			id.RequireNamespace(EntityID.Mobs);
		}

		public void Act()
		{
		}
	}
}
