using Labyrinth.Utils;

namespace Labyrinth.Entities
{
	public sealed class Player : Creature
	{
		public static readonly EntityID PlayerID = new EntityID($"{EntityID.Creatures}/Player");

		public Player(string name)
			: base(PlayerID, new Name(name, unique: true, proper: true), "This is you.", 50)
		{
		}
	}
}
