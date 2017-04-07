using System.Collections.Generic;

namespace Labyrinth.Maps.DunGen
{
    public interface IZoneGenerator
    {
        IEnumerable<string> Fill(Level level, Zone zone, int depth);
    }
}
