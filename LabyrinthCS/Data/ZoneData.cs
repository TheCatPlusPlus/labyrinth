using System.Drawing;

using Labyrinth.Data.Ids;
using Labyrinth.Maps;

namespace Labyrinth.Data
{
    public class ZoneData : HasId<Zone>
    {
        private static readonly Registry<Zone, ZoneData> Registry;

        public static readonly Id<Zone> Test = new Id<Zone>("z:test");

        public string Name { get; private set; }
        public int MaxDepth { get; private set; }
        public Size MaxLevelSize { get; private set; }

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
                    MaxLevelSize = new Size(80, 20)
                }
            };
        }

        public static ZoneData For(Id<Zone> id)
        {
            return Registry.Get(id);
        }
    }
}
