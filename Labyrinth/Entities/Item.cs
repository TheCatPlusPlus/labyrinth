using Labyrinth.Database;

namespace Labyrinth.Entities
{
	public sealed class Item : Entity
	{
		public Item(GamePrev game, EntityID id)
			: base(game, id)
		{
			id.RequireNamespace(EntityID.Items);
		}
	}
}
