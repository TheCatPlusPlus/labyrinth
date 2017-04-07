using Labyrinth.Data.Ids;
using Labyrinth.Maps;

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

        public string Singular { get; private set; }
        public string Plural { get; private set; }
        public string Description { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsTransparent { get; private set; }
        public int BaseMoveCost { get; private set; }

        public TileData(Id<Tile> id)
            : base(id)
        {
            IsWalkable = true;
            IsTransparent = true;
            BaseMoveCost = Const.MoveCostBase;
        }

        static TileData()
        {
            var wall = new TileData(Wall)
            {
                Singular = "a wall",
                Plural = "walls",
                Description =
                    "The walls are made out of solid stone. They feel weird to touch, slightly pulsating with strange energy. " +
                    "Here and there inscriptions in a forgotten language can be found.",
                IsWalkable = false,
                IsTransparent = false
            };

            var door = new TileData(DoorClosed)
            {
                Singular = "a closed door",
                Plural = "closed doors",
                Description =
                    "A heavy, intricately detailed stone door. The mechanism operating it seems to be working flawlessly.",
                IsWalkable = false,
                IsTransparent = false
            };

            Registry = new Registry<Tile, TileData>
            {
                new TileData(Ground)
                {
                    Singular = "the floor",
                    Plural = "the floor",
                    Description = "The floor is perfectly smooth and cold."
                },

                wall,

                new TileData(WallDeep)
                {
                    Singular = wall.Singular,
                    Plural = wall.Plural,
                    Description = wall.Description,
                    IsWalkable = false,
                    IsTransparent = false
                },

                door,

                new TileData(DoorOpen)
                {
                    Singular = "an open door",
                    Plural = "open doors",
                    Description = door.Description
                },

                new TileData(StairsDown)
                {
                    Singular = "a staircase leading down",
                    Plural = "staircases leading down",
                    Description = "A long staircase leading further into the Labyrinth."
                },

                new TileData(OutOfBounds),

                new TileData(WaterShallow)
                {
                    Singular = "pool of water",
                    Plural = "pool of water",
                    Description =
                        "This pool is shallow enough to cross, but the water will hinder your movement and combat ability.",
                    BaseMoveCost = Const.MoveCostBase * 2
                },

                new TileData(WaterDeep)
                {
                    Singular = "deep pool of water",
                    Plural = "deep pool of water",
                    Description =
                        "The water is murky and deep, it is best to not attempt crossing here.",
                    IsWalkable = false
                },

                new TileData(Lava)
                {
                    Singular = "pool of lava",
                    Plural = "pool of lava",
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
