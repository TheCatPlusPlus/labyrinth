using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Labyrinth.Utils.Geometry
{
    public sealed class Line : IEnumerable<Vector2I>
    {
        private readonly List<Vector2I> _points;

        public Vector2I Start { get; }
        public Vector2I End { get; }
        public IReadOnlyList<Vector2I> Points => _points;

        public Line(Vector2I start, Vector2I end)
        {
            Start = start;
            End = end;
            _points = new List<Vector2I>(Make(start, end));
        }

        [NotNull]
        public static IEnumerable<Vector2I> Make(Vector2I start, Vector2I end)
        {
            return DoMake(start, end).OrderBy(p => (p - start).NormL1());
        }

        [NotNull]
        private static IEnumerable<Vector2I> DoMake(Vector2I start, Vector2I end)
        {
            var diff = (end - start).Abs();
            var steep = diff.Y > diff.X;

            var x0 = start.X;
            var y0 = start.Y;
            var x1 = end.X;
            var y1 = end.Y;

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
