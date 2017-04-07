using System.Collections.Generic;

namespace Labyrinth.Maps.DunGen
{
    public interface ILevelGenerator
    {
        // yields generation step
        IEnumerable<string> Fill(Level level);
    }
}