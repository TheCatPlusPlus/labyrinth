using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Labyrinth.Data;
using Labyrinth.Data.Ids;
using Labyrinth.Entities;
using Labyrinth.Utils;

namespace Labyrinth.Maps
{
    public class Tile : IHasId<Tile>, IGridItem, ITileEntities
    {
        private readonly List<Item> _items;
        private bool _isLit;

        private Id<Tile> _id;
        private TileData _data;

        public Id<Tile> Id
        {
            get => _id;
            set
            {
                _id = value;
                _data = TileData.For(_id);
            }
        }

        IId IHasId.Id => Id;
        public Point Position { get; }
        public Level Level { get; }

        public IReadOnlyList<Item> Items => _items;
        public Monster Monster { get; private set; }
        public bool WasSeen { get; private set; }
        public object Tag { get; set; }
        public IEnumerable<Point> Neighbours => GetNeighbours();

        public Name Name => _data.Name;
        public string Description => _data.Description;
        public bool IsWalkable => Monster == null && _data.IsWalkable;
        public bool IsTransparent => _data.IsTransparent;
        public bool IsWall => TileData.AllWalls.Contains(Id);
        public bool IsDoor => TileData.AllDoors.Contains(Id);
        public bool IsExit => TileData.AllExits.Contains(Id);
        public int BaseMoveCost => IsWalkable ? _data.BaseMoveCost : int.MaxValue;

        public bool IsLit
        {
            get => _isLit;
            set
            {
                _isLit = value;
                if (value)
                {
                    WasSeen = true;
                }
            }
        }

        public Tile(Level level, Point position, Id<Tile> id = null)
        {
            Id = id ?? TileData.WallDeep;
            Position = position;
            Level = level;

            _items = new List<Item>();
        }

        bool ITileEntities.AddEntity(Entity entity)
        {
            if (!IsWalkable)
            {
                return false;
            }

            if (entity is Monster monster)
            {
                Monster = monster;
            }
            else if (entity is Item item)
            {
                _items.Add(item);
            }

            return true;
        }

        void ITileEntities.RemoveEntity(Entity entity)
        {
            if (entity.Position != Position)
            {
                throw new InvalidOperationException("Entity's position doesn't match the tile's");
            }

            if (entity is Monster)
            {
                Monster = null;
            }
            else if (entity is Item item)
            {
                _items.Remove(item);
            }
        }

        private IEnumerable<Point> GetNeighbours()
        {
            for (var dx = -1; dx <= 1; ++dx)
            {
                for (var dy = -1; dy <= 1; ++dy)
                {
                    var p = Position + new Size(dx, dy);

                    if (p == Position)
                    {
                        continue;
                    }

                    if (!Level.Rect.Contains(p))
                    {
                        continue;
                    }

                    yield return p;
                }
            }
        }

        public override string ToString()
        {
            return $"{Position}: {Id}";
        }
    }
}
