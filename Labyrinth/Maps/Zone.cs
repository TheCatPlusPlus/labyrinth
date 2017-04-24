﻿using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Data.Ids;
using Labyrinth.Maps.DunGen;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.Maps
{
    public sealed class Zone : HasId<Zone>
    {
        private readonly ZoneData _data;

        public Zone([NotNull] Id<Zone> id)
            : base(id)
        {
            _data = ZoneData.For(id);
        }

        [NotNull]
        public Level CreateLevel(int depth)
        {
            var size = _data.MaxLevelSize;
            size = new Vector2I(size.Width | 1, size.Height | 1);

            // TODO
            var levelGen = new SimpleLevelGenerator();
            var zoneGen = new SimpleZoneGenerator();

            var level = new Level($"{_data.Name}:{depth + 1}", size);

            foreach (var status in levelGen.Fill(level))
            {
                Log.Info($"Generating {level.Name}: level structure: {status}");
            }

            foreach (var status in zoneGen.Fill(level, this, depth))
            {
                Log.Info($"Generating {level.Name}: zone specifics: {status}");
            }

            Log.Info($"Generating {level.Name}: fixing wall types");
            // every wall that's surrounded by other walls should be a deep wall
            foreach (var tile in level.Tiles.Where(t => t.IsWall))
            {
                tile.Id = tile.Neighbours.All(p => level[p].IsWall)
                    ? TileData.WallDeep
                    : TileData.Wall;
            }

            return level;
        }
    }
}