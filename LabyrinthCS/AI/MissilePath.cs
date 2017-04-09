using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using Labyrinth.Maps;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.AI
{
    public sealed class MissilePath : IPathFinder
    {
        private readonly List<Vector2I> _points;

        public Vector2I Start { get; }
        public Vector2I Goal { get; }
        public bool Found { get; }
        public IReadOnlyList<Vector2I> Points => _points;

        public MissilePath([NotNull] Level level, Vector2I start, Vector2I goal, bool penetrating = false)
        {
            Start = start;
            Goal = goal;

            // missiles will crash or penetrate obstacles, not path around them
            // so path is 'found' regardless of whether we'll actually
            // reach the goal
            Found = true;
            _points = new List<Vector2I>();

            // we need to start closest to the start and move outwards
            // otherwise collisions might be checked from the wrong side
            // and the resulting path will be something like
            //   ...Xxx#...@
            // instead of
            //   ......#xxx@
            // (this is due to coordinate transformations needed in Bresenham's algorithm)

            foreach (var point in Line.Make(start, goal))
            {
                if (point == start)
                {
                    continue;
                }

                if (!level.Rect.Contains(point))
                {
                    break;
                }

                var tile = level[point];
                _points.Add(point);

                if (!tile.CanFlyOver)
                {
                    var monster = tile.Actor != null;
                    var goThrough = monster && penetrating;

                    // only go through monsters
                    if (!goThrough)
                    {
                        break;
                    }
                }
            }
        }

        public IEnumerator<Vector2I> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
