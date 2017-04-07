using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Labyrinth.Data.Ids;
using Labyrinth.Utils;

namespace Labyrinth.Maps
{
    public class Level
    {
        private readonly Grid<Tile> _tiles;

        public string Name { get; }
        public Rectangle Rect => _tiles.Rect;
        public Tile this[int x, int y] => _tiles[x, y];
        public Tile this[Point p] => _tiles[p];

        public IEnumerable<Tile> WalkableTiles => _tiles.Where(t => t.IsWalkable);
        public IEnumerable<Tile> Tiles => _tiles;

        public Level(string name, Size size)
        {
            Name = name;
            _tiles = new Grid<Tile>(size, p => new Tile(this, p));
        }

        public Point RandomWalkable()
        {
            // TODO inefficient
            var tiles = WalkableTiles.ToArray();
            if (tiles.Length == 0)
            {
                return new Point(-1, -1);
            }

            var index = State.Rng.Next(tiles.Length);
            return WalkableTiles.ElementAt(index).Position;
        }

        public void Fill(Rectangle rect, Id<Tile> id)
        {
            foreach (var p in rect.Points())
            {
                this[p].Id = id;
            }
        }
    }
}
