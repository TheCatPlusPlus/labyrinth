using Labyrinth.Database;
using Labyrinth.Entities.Damage;
using Labyrinth.Map;

namespace Labyrinth.Entities
{
	public sealed class Player : Creature
	{
		public override bool CanOpenDoors => true;
		public int VisionRange => 5;
		public FieldOfView FOV { get; }

		public Player(Game game)
			: base(game, DB.CreaturePlayer, 50)
		{
			FOV = new FieldOfView(this);
		}

		public override void Attack(Creature target)
		{
			var damage = new DamageSpec();
			damage.Inflict(DamageType.Blunt, 5);
			target.TakeDamage(this, damage);
		}
	}
}
