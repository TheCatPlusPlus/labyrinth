using System.Collections.Generic;

namespace Labyrinth.Maps.DunGen
{
    public sealed class SimpleZoneGenerator : IZoneGenerator
    {
        public IEnumerable<string> Fill(Level level, Zone zone, int depth)
        {
            yield return "placing stairs";
        }
    }
}
