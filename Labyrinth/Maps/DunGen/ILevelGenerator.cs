using System.Collections.Generic;

using JetBrains.Annotations;

namespace Labyrinth.Maps.DunGen
{
    public interface ILevelGenerator
    {
        // yields generation step
        [ItemNotNull]
        [NotNull]
        IEnumerable<string> Fill([NotNull] Level level);
    }
}
