using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Labyrinth.Utils;

namespace Labyrinth.Maps
{
    public class Grid<T> : IEnumerable<T>
        where T : class, IGridItem
    {
        private readonly T[,] _cells;

        public Size Size { get; }
        public Rectangle Rect { get; }
        public T this[int x, int y] => Get(x, y);
        public T this[Point p] => this[p.X, p.Y];

        public Grid(Size size, Func<Point, T> makeCell)
        {
            Size = size;
            Rect = new Rectangle(new Point(0, 0), Size);
            _cells = new T[Size.Width, Size.Height];

            foreach (var p in Rect.Points())
            {
                _cells[p.X, p.Y] = makeCell(p);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Rect.Points()
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
                throw new OutOfBounds(new Point(x, y));
            }
        }
    }
}
