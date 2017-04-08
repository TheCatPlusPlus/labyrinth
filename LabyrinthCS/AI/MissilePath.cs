using System.Collections.Generic;

using JetBrains.Annotations;

using Labyrinth.Maps;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.AI
{
    public sealed class MissilePath : IPathFinder
    {
        public bool Found { get; }
        public IEnumerable<Vector2I> Points { get; }

        public MissilePath([NotNull] Level level, Vector2I start, Vector2I goal, bool penetrating = false)
        {
            // missiles will crash or penetrate obstacles, not path around them
            // so path is 'found' regardless of whether we'll actually
            // reach the goal
            Found = true;

//            var direction = goal - (Size)start;
//
//            while (point != goal)
//            {
//                var delta = goal - (Size)point;
//                var distance = delta.X * delta.X + delta.Y * delta.Y;
//
//            }
        }
    }
}
