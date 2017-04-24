using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Labyrinth.Utils.Geometry;

namespace Labyrinth.Maps
{
    public sealed class Grid<T> : IEnumerable<T>
        where T : class, IGridItem
    {
        private readonly T[,] _cells;

        public Vector2I Size { get; }
        public Rect Rect { get; }
        public T this[int x, int y] => Get(x, y);
        public T this[Vector2I p] => this[p.X, p.Y];

        public Grid(Vector2I size, Func<Vector2I, T> makeCell)
        {
            Size = size;
            Rect = new Rect(0, 0, Size);
            _cells = new T[Size.Width, Size.Height];

            foreach (var p in Rect.Points)
            {
                _cells[p.X, p.Y] = makeCell(p);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Rect
                .Points
                .Select(p => this[p])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private T Get(int x, int y)
        {
            try
            {
                return _cells[x, y];
            }
            catch (IndexOutOfRangeException)
            {
                throw new OutOfBounds(new Vector2I(x, y));
            }
        }
    }
}
