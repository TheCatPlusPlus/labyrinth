using Labyrinth.Database;

namespace Labyrinth.Entities
{
	public sealed class Player : Creature
	{
		public override bool CanOpenDoors => true;

		public Player(Game game)
			: base(game, DB.CreaturePlayer, 50)
		{
		}
	}
}
