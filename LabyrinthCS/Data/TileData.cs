using JetBrains.Annotations;

using Labyrinth.Data.Ids;
using Labyrinth.Maps;
using Labyrinth.Utils;

namespace Labyrinth.Data
{
    public sealed class TileData : HasId<Tile>
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
        public bool CanWalkThrough { get; private set; }
        public bool CanFlyOver { get; private set; }
        public bool CanSeeThrough { get; private set; }
        public decimal CostFactor { get; private set; }

        public bool IsSolid
        {
            get => !CanWalkThrough && !CanFlyOver && !CanSeeThrough;
            private set => CanWalkThrough = CanFlyOver = CanSeeThrough = !value;
        }

        public TileData([NotNull] Id<Tile> id)
            : base(id)
        {
            Name = new Name("unknown tile", thing: true);
            Description = "This tile has not been described yet. It is a bug.";
            IsSolid = false;
            CostFactor = 1.0m;
        }

        public TileData([NotNull] Id<Tile> id, [NotNull] TileData other)
            : base(id)
        {
            Name = other.Name;
            Description = other.Description;
            CanWalkThrough = other.CanWalkThrough;
            CanFlyOver = other.CanFlyOver;
            CanSeeThrough = other.CanSeeThrough;
            CostFactor = other.CostFactor;
        }

        static TileData()
        {
            var wall = new TileData(Wall)
            {
                Name = new Name("wall", thing: true),
                Description =
                    "The walls are made out of solid stone. They feel weird to touch, slightly pulsating with strange energy. " +
                    "Here and there inscriptions in a forgotten language can be found.",
                IsSolid = true
            };

            var door = new TileData(DoorClosed)
            {
                Name = new Name("closed door", thing: true),
                Description =
                    "A heavy, intricately detailed stone door. The mechanism operating it seems to be working flawlessly.",
                IsSolid = true
            };

            Registry = new Registry<Tile, TileData>
            {
                new TileData(Ground)
                {
                    Name = new Name("floor", countable: false, unique: true, thing: true),
                    Description = "The floor is perfectly smooth and cold."
                },

                wall,
                new TileData(WallDeep, wall),
                new TileData(OutOfBounds, wall),

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

                new TileData(WaterShallow)
                {
                    Name = new Name("pool of water", "pools of water", thing: true),
                    Description =
                        "This pool is shallow enough to cross, but the water will hinder your movement and combat ability.",
                    CostFactor = 2.0m
                },

                new TileData(WaterDeep)
                {
                    Name = new Name("pool of deep water", "pools of deep water", thing: true),
                    Description =
                        "The water is murky and deep, it is best to not attempt crossing here.",
                    CanWalkThrough = false
                },

                new TileData(Lava)
                {
                    Name = new Name("pool of lava", "pools of lava", thing: true),
                    Description = "This sizzling pool of molten rock is extremely dangerous, " +
                                  "even coming near it might cause you to get badly burnt.",
                    CanWalkThrough = false
                    // TODO: damage, possible to cross with fire resist
                }
            };
        }

        [NotNull]
        public static TileData For([NotNull] Id<Tile> id)
        {
            return Registry.Get(id);
        }
    }
}
