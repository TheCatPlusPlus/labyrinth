using Labyrinth.Data.Ids;
using Labyrinth.Entities;

namespace Labyrinth.Data
{
    public class ItemData : HasId<Item>
    {
        private static readonly Registry<Item, ItemData> Registry;

        public static readonly Id<Item> Multiple = new Id<Item>("i:multiple-items");
        public static readonly Id<Item> Sword = new Id<Item>("i:sword");
        public static readonly Id<Item> Rock = new Id<Item>("i:rock");

        public string Singular { get; private set; }
        public string Plural { get; private set; }
        public string Description { get; private set; }

        public ItemData(Id<Item> id)
            : base(id)
        {
        }

        static ItemData()
        {
            Registry = new Registry<Item, ItemData>
            {
                new ItemData(Sword)
                {
                    Singular = "a sword",
                    Plural = "swords",
                    Description = "An ordinary steel blade."
                },

                new ItemData(Rock)
                {
                    Singular = "a rock",
                    Plural = "rocks",
                    Description = "This rock might be fairly small, but it can still be a potent thrown weapon."
                }
            };
        }

        public static ItemData For(Id<Item> id)
        {
            return Registry.Get(id);
        }
    }
}