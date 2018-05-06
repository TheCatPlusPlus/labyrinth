namespace Labyrinth.Entities
{
	public sealed class Player : Creature
	{
		public static readonly EntityID PlayerID = new EntityID($"{EntityID.Creatures}/Player");

		public override bool CanOpenDoors => true;

		public Player(Game game)
			: base(game, PlayerID, 50)
		{
		}
	}
}
