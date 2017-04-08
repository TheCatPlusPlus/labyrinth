using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Data.Ids;
using Labyrinth.Entities;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.Maps
{
    public sealed class Level : IEntityList
    {
        private readonly Grid<Tile> _tiles;
        private readonly HashSet<Monster> _monsters;

        public string Name { get; }
        public Rect Rect => _tiles.Rect;
        public Tile this[int x, int y] => _tiles[x, y];
        public Tile this[Vector2I p] => _tiles[p];

        [ItemNotNull]
        [NotNull]
        public IEnumerable<Tile> WalkableTiles => _tiles.Where(t => t.CanWalkThrough);
        [ItemNotNull]
        [NotNull]
        public IEnumerable<Tile> Tiles => _tiles;
        [ItemNotNull]
        [NotNull]
        public IEnumerable<Monster> Monsters => _monsters;

        public Level(string name, Vector2I size)
        {
            Name = name;
            _tiles = new Grid<Tile>(size, p => new Tile(this, p));
            _monsters = new HashSet<Monster>();
        }

        public Vector2I RandomWalkable()
        {
            // TODO inefficient
            var tiles = WalkableTiles.ToArray();
            if (tiles.Length == 0)
            {
                return GridPoint.Invalid;
            }

            var index = State.Rng.Next(tiles.Length);
            return WalkableTiles.ElementAt(index).Position;
        }

        public void Fill(Rect rect, Id<Tile> id)
        {
            foreach (var p in rect.Points)
            {
                this[p].Id = id;
            }
        }

        bool IEntityList.AddEntity(Entity entity)
        {
            if (entity is Monster monster)
            {
                _monsters.Add(monster);
            }

            return true;
        }

        void IEntityList.RemoveEntity(Entity entity)
        {
            if (entity is Monster monster)
            {
                _monsters.Remove(monster);
            }
        }
    }
}
