namespace Labyrinth.Entities
{
	public sealed class Item : Entity
	{
		public Item(EntityID id)
			: base(id)
		{
			id.RequireNamespace(EntityID.Items);
		}
	}
}
