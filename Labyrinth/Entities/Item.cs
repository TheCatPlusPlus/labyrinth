namespace Labyrinth.Entities
{
	public sealed class Item : Entity
	{
		public Item(Game game, EntityID id)
			: base(game, id)
		{
			id.RequireNamespace(EntityID.Items);
		}
	}
}
