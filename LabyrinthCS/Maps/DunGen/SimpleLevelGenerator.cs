using System.Collections.Generic;
using System.Drawing;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Entities;
using Labyrinth.Utils;

namespace Labyrinth.Maps.DunGen
{
    public class SimpleLevelGenerator : ILevelGenerator
    {
        [ItemNotNull]
        public IEnumerable<string> Fill(Level level)
        {
            yield return "creating simple room";

            const int margin = 3;
            var room = new Rectangle(margin, margin, level.Rect.Width - margin * 2, level.Rect.Height - margin * 2);
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
                if (point.IsInvalid())
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
            new Item(ItemData.Sword).Spawn(level, oneItem.Position);
            new Item(ItemData.Sword).Spawn(level, manyItems.Position);
            new Item(ItemData.Sword).Spawn(level, monsterItem.Position);
            new Item(ItemData.Sword).Spawn(level, monsterManyItems.Position);
            for (var i = 0; i < 10; ++i)
            {
                new Item(ItemData.Rock).Spawn(level, manyItems.Position);
                new Item(ItemData.Rock).Spawn(level, monsterManyItems.Position);
            }
            new Monster(MonsterData.Bat).Spawn(level, monsterItem.Position);
            new Monster(MonsterData.Rodent).Spawn(level, monsterManyItems.Position);
        }
    }
}
