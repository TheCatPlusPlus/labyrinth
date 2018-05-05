using Labyrinth.Utils;

namespace Labyrinth.Entities
{
	public sealed class Mob : Creature
	{
		public Mob(EntityID id, Name name, string desc, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(id, name, desc, maxHP, speed)
		{
			id.RequireNamespace(EntityID.Mobs);
		}

		public void Act()
		{
		}
	}
}
