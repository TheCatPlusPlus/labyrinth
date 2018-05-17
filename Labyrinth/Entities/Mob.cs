using Labyrinth.AI;
using Labyrinth.Database;
using Labyrinth.Entities.Damage;

namespace Labyrinth.Entities
{
	public sealed class Mob : Creature
	{
		public Brain Brain { get; }

		public Mob(Game game, EntityID id, Brain brain, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(game, id, maxHP, speed)
		{
			Brain = brain;
			id.RequireNamespace(EntityID.Mobs);
		}

		public void Act()
		{
			Brain.Act(this);
		}

		private DamageSpec CalcMeleeDamage()
		{
			return new DamageSpec();
		}
	}
}
