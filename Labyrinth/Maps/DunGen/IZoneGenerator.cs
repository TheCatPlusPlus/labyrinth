using System.Collections.Generic;

using JetBrains.Annotations;

namespace Labyrinth.Maps.DunGen
{
    public interface IZoneGenerator
    {
        // yields generation step
        [ItemNotNull]
        [NotNull]
        IEnumerable<string> Fill([NotNull] Level level, [NotNull] Zone zone, int depth);
    }
}
