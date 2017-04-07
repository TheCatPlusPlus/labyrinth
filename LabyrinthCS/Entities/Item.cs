using Labyrinth.Data;
using Labyrinth.Data.Ids;

namespace Labyrinth.Entities
{
    public class Item : Entity, IHasId<Item>
    {
        private readonly ItemData _data;

        public Id<Item> Id { get; }
        IId IHasId.Id => Id;

        public string Singular => _data.Singular;
        public string Plural => _data.Plural;
        public string Description => _data.Description;

        public Item(Id<Item> id)
        {
            Id = id;
            _data = ItemData.For(Id);
        }
    }
}
