using JetBrains.Annotations;

using Labyrinth.Data.Ids;
using Labyrinth.Maps;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.Data
{
    public sealed class ZoneData : HasId<Zone>
    {
        private static readonly Registry<Zone, ZoneData> Registry;

        public static readonly Id<Zone> Test = new Id<Zone>("z:test");

        public string Name { get; private set; }
        public int MaxDepth { get; private set; }
        public Vector2I MaxLevelSize { get; private set; }

        public ZoneData(Id<Zone> id)
            : base(id)
        {
        }

        static ZoneData()
        {
            Registry = new Registry<Zone, ZoneData>
            {
                new ZoneData(Test)
                {
                    Name = "Test Zone",
                    MaxDepth = 3,
                    MaxLevelSize = new Vector2I(80, 20)
                }
            };
        }

        [NotNull]
        public static ZoneData For([NotNull] Id<Zone> id)
        {
            return Registry.Get(id);
        }
    }
}
