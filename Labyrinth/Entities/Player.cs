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

		public Player(GamePrev game)
			: base(game, DB.CreaturePlayer, 50)
		{
			FOV = new FieldOfView(this);
		}

		protected override void AddMeleeDamage(DamageSpec damage)
		{
			damage.Inflict(DamageType.Blunt, 5);
		}
	}
}
