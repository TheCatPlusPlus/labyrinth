using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Data.Ids;
using Labyrinth.Entities;
using Labyrinth.Entities.Time;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.Maps
{
    public sealed class Tile : IHasId<Tile>, IGridItem, IEntityList
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
        public Vector2I Position { get; }
        public Level Level { get; }

        public IReadOnlyList<Item> Items => _items;
        public Actor Actor { get; private set; }
        public bool WasSeen { get; private set; }
        public object Tag { get; set; }
        public IEnumerable<Vector2I> Neighbours => GetNeighbours();

        public Name Name => _data.Name;
        public string Description => _data.Description;
        public bool CanWalkThrough => (Actor == null) && _data.CanWalkThrough;
        public bool CanFlyOver => (Actor == null) && _data.CanFlyOver;
        public bool CanSeeThrough => _data.CanSeeThrough;
        public bool IsWall => TileData.AllWalls.Contains(Id);
        public bool IsDoor => TileData.AllDoors.Contains(Id);
        public bool IsExit => TileData.AllExits.Contains(Id);

        public float CostFactor => _data.CostFactor;
        public int BaseMoveCost => CanWalkThrough ? MoveCost.ApplyFactor(CostFactor) : int.MaxValue;

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

        public Tile([NotNull] Level level, Vector2I position, [CanBeNull] Id<Tile> id = null)
        {
            Id = id ?? TileData.WallDeep;
            Position = position;
            Level = level;

            _items = new List<Item>();
        }

        bool IEntityList.AddEntity(Entity entity)
        {
            if (!CanWalkThrough)
            {
                return false;
            }

            if (entity is Actor monster)
            {
                Actor = monster;
            }
            else if (entity is Item item)
            {
                _items.Add(item);
            }

            return true;
        }

        void IEntityList.RemoveEntity(Entity entity)
        {
            if (entity.Position != Position)
            {
                throw new InvalidOperationException("Entity's position doesn't match the tile's");
            }

            if (entity is Actor)
            {
                Actor = null;
            }
            else if (entity is Item item)
            {
                _items.Remove(item);
            }
        }

        private IEnumerable<Vector2I> GetNeighbours()
        {
            for (var dx = -1; dx <= 1; ++dx)
            for (var dy = -1; dy <= 1; ++dy)
            {
                var p = Position + new Vector2I(dx, dy);

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

        public override string ToString()
        {
            return $"{Position}: {Id}";
        }
    }
}
