using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Maps;
using Labyrinth.Utils;
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

            var line = MakeLine(start, goal).OrderBy(p => (p - start).NormL1());
            foreach (var point in line)
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

                if (!tile.CanFlyOver)
                {
                    // only go through monsters
                    // (and include them as final point)

                    if (tile.Actor != null)
                    {
                        _points.Add(point);
                        if (!penetrating)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                _points.Add(point);
            }
        }

        private static IEnumerable<Vector2I> MakeLine(Vector2I start, Vector2I goal)
        {
            var diff = (goal - start).Abs();
            var steep = diff.Y > diff.X;

            var x0 = start.X;
            var y0 = start.Y;
            var x1 = goal.X;
            var y1 = goal.Y;

            if (steep)
            {
                MiscExt.Swap(ref x0, ref y0);
                MiscExt.Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                MiscExt.Swap(ref x0, ref x1);
                MiscExt.Swap(ref y0, ref y1);
            }

            var dx = x1 - x0;
            var dy = Math.Abs(y1 - y0);
            var error = dx / 2;
            var step = y0 < y1 ? 1 : -1;

            var y = y0;
            for (var x = x0; x <= x1; ++x)
            {
                var point = new Vector2I(x, y);

                if (steep)
                {
                    point = point.Swap();
                }

                yield return point;

                error -= dy;
                if (error < 0)
                {
                    y += step;
                    error += dx;
                }
            }
        }
    }
}
