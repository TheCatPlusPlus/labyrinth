using Labyrinth.Data.Ids;
using Labyrinth.Entities;
using Labyrinth.Utils;

namespace Labyrinth.Data
{
    public class MonsterData : HasId<Monster>
    {
        private static readonly Registry<Monster, MonsterData> Registry;

        public static readonly Id<Monster> Player = new Id<Monster>("m:player");
        public static readonly Id<Monster> Rodent = new Id<Monster>("m:rodent");
        public static readonly Id<Monster> Bat = new Id<Monster>("m:bat");

        public Name Name { get; private set; }
        public string Description { get; private set; }

        public float SpeedFactor { get; private set; }

        public MonsterData(Id<Monster> id)
            : base(id)
        {
            SpeedFactor = 1.0f;
        }

        static MonsterData()
        {
            Registry = new Registry<Monster, MonsterData>
            {
                new MonsterData(Player)
                {
                    Name = new Name("player", unique: true),
                    Description = "No time for introspection."
                },

                new MonsterData(Rodent)
                {
                    Name = new Name("rodent of unusual size", "rodents of unusual size"),
                    Description = "A giant rodent that will eat anything it can get its paws on."
                },

                new MonsterData(Bat)
                {
                    // TODO flying
                    Name = new Name("bat"),
                    Description = "An unusually aggressive kind of bat.",
                    SpeedFactor = 2.0f
                }
            };
        }

        public static MonsterData For(Id<Monster> id)
        {
            return Registry.Get(id);
        }
    }
}
