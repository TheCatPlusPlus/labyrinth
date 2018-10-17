using Labyrinth.AI;
using Labyrinth.Database;
using Labyrinth.Entities.Damage;

namespace Labyrinth.Entities
{
	public sealed class Mob : Creature
	{
		public Brain Brain { get; }

		private Mob(Game game, EntityID id, Brain brain, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(game, id, maxHP, speed)
		{
			id.RequireNamespace(EntityID.Mobs);
			Brain = brain;
		}

		public static Mob Create(Game game, EntityID id)
		{
			var data = DB.Entities.GetCreature(id);
			return new Mob(game, id, new TestBrain(game), data.HP, data.Speed);
		}

		public void Act()
		{
			Brain.Act(this);
		}

		protected override void AddMeleeDamage(DamageSpec damage)
		{
			var data = DB.Entities.Get(this);
			damage.Inflict(data.MeleeDamageType, data.MeleeDamage.Roll(Game.RNG));
		}
	}
}
