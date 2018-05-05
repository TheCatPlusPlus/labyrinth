using Labyrinth.Utils;

namespace Labyrinth.Entities
{
	public sealed class Player : Creature
	{
		public static readonly EntityID PlayerID = new EntityID($"{EntityID.Creatures}/Player");

		public Player(Game game, string name)
			: base(game, PlayerID, new Name(name, unique: true, proper: true), "This is you.", 50)
		{
		}
	}
}
