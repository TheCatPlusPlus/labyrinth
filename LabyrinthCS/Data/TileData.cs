using Labyrinth.Data.Ids;
using Labyrinth.Maps;
using Labyrinth.Utils;

namespace Labyrinth.Data
{
    public class TileData : HasId<Tile>
    {
        private static readonly Registry<Tile, TileData> Registry;

        public static readonly Id<Tile> Ground = new Id<Tile>("t:ground");
        public static readonly Id<Tile> Wall = new Id<Tile>("t:wall");
        public static readonly Id<Tile> WallDeep = new Id<Tile>("t:wall-deep");
        public static readonly Id<Tile> DoorClosed = new Id<Tile>("t:door-closed");
        public static readonly Id<Tile> DoorOpen = new Id<Tile>("t:door-open");
        public static readonly Id<Tile> StairsDown = new Id<Tile>("t:stairs-down");
        public static readonly Id<Tile> OutOfBounds = new Id<Tile>("t:oob");
        public static readonly Id<Tile> WaterShallow = new Id<Tile>("t:water-shallow");
        public static readonly Id<Tile> WaterDeep = new Id<Tile>("t:water-deep");
        public static readonly Id<Tile> Lava = new Id<Tile>("t:lava");

        public static readonly Id<Tile>[] AllWalls =
        {
            Wall,
            WallDeep
        };

        public static readonly Id<Tile>[] AllDoors =
        {
            DoorClosed,
            DoorOpen
        };

        public static readonly Id<Tile>[] AllExits =
        {
            StairsDown
        };

        public Name Name { get; private set; }
        public string Description { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsTransparent { get; private set; }
        public float CostFactor { get; private set; }

        public TileData(Id<Tile> id)
            : base(id)
        {
            IsWalkable = true;
            IsTransparent = true;
            CostFactor = 1.0f;
        }

        static TileData()
        {
            var wall = new TileData(Wall)
            {
                Name = new Name("wall", thing: true),
                Description =
                    "The walls are made out of solid stone. They feel weird to touch, slightly pulsating with strange energy. " +
                    "Here and there inscriptions in a forgotten language can be found.",
                IsWalkable = false,
                IsTransparent = false
            };

            var door = new TileData(DoorClosed)
            {
                Name = new Name("closed door", thing: true),
                Description =
                    "A heavy, intricately detailed stone door. The mechanism operating it seems to be working flawlessly.",
                IsWalkable = false,
                IsTransparent = false
            };

            Registry = new Registry<Tile, TileData>
            {
                new TileData(Ground)
                {
                    Name = new Name("floor", countable: false, unique: true, thing: true),
                    Description = "The floor is perfectly smooth and cold."
                },

                wall,

                new TileData(WallDeep)
                {
                    Name = wall.Name,
                    Description = wall.Description,
                    IsWalkable = false,
                    IsTransparent = false
                },

                door,

                new TileData(DoorOpen)
                {
                    Name = new Name("open door", thing: true),
                    Description = door.Description
                },

                new TileData(StairsDown)
                {
                    Name = new Name("staircase leading down", "staircases leading down", thing: true),
                    Description = "A long staircase leading further into the Labyrinth."
                },

                new TileData(OutOfBounds),

                new TileData(WaterShallow)
                {
                    Name = new Name("pool of water", "pools of water", thing: true),
                    Description =
                        "This pool is shallow enough to cross, but the water will hinder your movement and combat ability.",
                    CostFactor = 2.0f
                },

                new TileData(WaterDeep)
                {
                    Name = new Name("pool of deep water", "pools of deep water", thing: true),
                    Description =
                        "The water is murky and deep, it is best to not attempt crossing here.",
                    IsWalkable = false
                },

                new TileData(Lava)
                {
                    Name = new Name("pool of lava", "pools of lava", thing: true),
                    Description = "This sizzling pool of molten rock is extremely dangerous, " +
                                  "even coming near it might cause you to get badly burnt.",
                    IsWalkable = false
                    // TODO: damage, possible to cross with fire resist
                }
            };
        }

        public static TileData For(Id<Tile> id)
        {
            return Registry.Get(id);
        }
    }
}
