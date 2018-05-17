using Labyrinth.Database;
using Labyrinth.Entities.Damage;

namespace Labyrinth.Entities
{
	public sealed class Player : Creature
	{
		public override bool CanOpenDoors => true;

		public Player(Game game)
			: base(game, DB.CreaturePlayer, 50)
		{
		}

		public override void Attack(Creature target)
		{
			var damage = new DamageSpec();
			damage.Inflict(DamageType.Blunt, 5);
			target.TakeDamage(this, damage);
		}
	}
}
