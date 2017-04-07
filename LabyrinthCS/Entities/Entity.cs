using System;
using System.Drawing;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Maps;
using Labyrinth.Utils;

namespace Labyrinth.Entities
{
    public abstract class Entity : IGridItem
    {
        public Point Position { get; private set; }
        public Level Level { get; private set; }

        protected Entity()
        {
            Position = PointExt.Invalid;
        }

        public bool Move(Point to)
        {
            if (Level == null)
            {
                throw new InvalidOperationException("Attempting to move an unspawned entity");
            }

            if (!Level.Rect.Contains(to))
            {
                return false;
            }

            var current = Level[Position];
            var next = Level[to];

            if (!next.IsWalkable)
            {
                if (next.IsDoor)
                {
                    next.Id = TileData.DoorOpen;
                }

                return false;
            }

            ((IEntityList)current).RemoveEntity(this);
            ((IEntityList)next).AddEntity(this);
            Position = to;
            return true;
        }

        public void Despawn()
        {
            if (Level == null)
            {
                throw new InvalidOperationException("Attempting to despawn an unspawned entity");
            }

            ((IEntityList)Level[Position]).RemoveEntity(this);
            ((IEntityList)Level).RemoveEntity(this);
            Level = null;
            Position = PointExt.Invalid;
        }

        public void Spawn([NotNull] Level level, Point position)
        {
            if (Level != null)
            {
                Despawn();
            }

            IEntityList tile;
            try
            {
                tile = level[position];
            }
            catch (OutOfBounds e)
            {
                throw new ArgumentException("Trying to spawn entity outside of level bounds", nameof(position), e);
            }

            if (!tile.AddEntity(this))
            {
                throw new ArgumentException("Trying to spawn entity in a taken spot", nameof(position));
            }

            Level = level;
            Position = position;
            ((IEntityList)Level).AddEntity(this);
        }

        public void Spawn([NotNull] Level level)
        {
            var position = level.RandomWalkable();
            if (position.IsInvalid())
            {
                throw new InvalidOperationException("Could not find a viable spawn point");
            }

            Spawn(level, position);
        }
    }
}
