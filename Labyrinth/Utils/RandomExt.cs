using System.Collections.Generic;

using JetBrains.Annotations;

using Pcg;

namespace Labyrinth.Utils
{
    public static class RandomExt
    {
        public static T Pick<T>([NotNull] this PcgRandom rng, [NotNull] IList<T> choices)
        {
            var index = rng.Next(choices.Count);
            return choices[index];
        }
    }
}
