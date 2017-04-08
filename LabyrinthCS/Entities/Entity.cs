﻿using System;
using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Maps;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.Entities
{
    public abstract class Entity : IGridItem
    {
        public Vector2I Position { get; private set; }
        public Level Level { get; private set; }

        protected Entity()
        {
            Position = GridPoint.Invalid;
        }

        public bool Move(Vector2I to)
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

            if (!next.CanWalkThrough)
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
            Position = GridPoint.Invalid;
        }

        public void Spawn([NotNull] Level level, [NotNull] Tile tile)
        {
            Debug.Assert(level == tile.Level);
            Spawn(level, tile.Position);
        }

        public void Spawn([NotNull] Level level, Vector2I position)
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
            if (position.IsInvalidPoint())
            {
                throw new InvalidOperationException("Could not find a viable spawn point");
            }

            Spawn(level, position);
        }
    }
}
