using System.Collections.Generic;

using Labyrinth.Data;
using Labyrinth.Entities;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.Maps.DunGen
{
    public sealed class SimpleLevelGenerator : ILevelGenerator
    {
        public IEnumerable<string> Fill(Level level)
        {
            yield return "creating simple room";

            const int margin = 3;
            var room = new Rect(margin, margin, level.Rect.Width - margin * 2, level.Rect.Height - margin * 2);
            level.Fill(room, TileData.Ground);

            yield return "creating some walls and doors";

            var types = new[]
            {
                TileData.Wall,
                TileData.DoorOpen,
                TileData.DoorClosed,
                TileData.WaterDeep,
                TileData.WaterShallow,
                TileData.Lava
            };

            for (var i = 0; i < 50; ++i)
            {
                var point = level.RandomWalkable();
                if (point.IsInvalidPoint())
                {
                    break;
                }

                var tile = level[point];
                tile.Id = State.Rng.Pick(types);
            }

            var oneItem = level[20, 15];
            var manyItems = level[21, 15];
            var monster = level[22, 15];
            var monsterItem = level[23, 15];
            var monsterManyItems = level[24, 15];

            oneItem.Id = manyItems.Id = monster.Id = monsterItem.Id = monsterManyItems.Id = TileData.Ground;
            new Item(ItemData.Sword).Spawn(level, oneItem);
            new Item(ItemData.Sword).Spawn(level, manyItems);
            new Item(ItemData.Sword).Spawn(level, monsterItem);
            new Item(ItemData.Sword).Spawn(level, monsterManyItems);
            for (var i = 0; i < 10; ++i)
            {
                new Item(ItemData.Rock).Spawn(level, manyItems);
                new Item(ItemData.Rock).Spawn(level, monsterManyItems);
            }

            new Monster(MonsterData.Bat).Spawn(level, monsterItem);
            new Monster(MonsterData.Rodent).Spawn(level, monsterManyItems);
        }
    }
}
