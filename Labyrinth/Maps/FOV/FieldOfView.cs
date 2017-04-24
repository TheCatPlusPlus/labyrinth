using System.Collections.Generic;

using JetBrains.Annotations;

using Labyrinth.Entities;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.Maps.FOV
{
    public sealed class FieldOfView
    {
        private readonly Player _player;
        private readonly List<Tile> _visible;

        public FieldOfView(Player player)
        {
            _player = player;
            _visible = new List<Tile>();
        }

        public void Update(Level level)
        {
            foreach (var tile in _visible)
            {
                tile.IsLit = false;
            }

            _visible.Clear();

            var fov = new Rect(_player.Position, 1, 1).Inflated(Const.FovRadius);
            foreach (var goal in fov.EdgePoints)
            foreach (var point in Line.Make(_player.Position, goal))
            {
                if (!level.Rect.Contains(point))
                {
                    continue;
                }

                var tile = level[point];
                Visit(tile);

                if (!tile.CanSeeThrough)
                {
                    break;
                }
            }

            Visit(level, _player.Position);
        }

        private void Visit([NotNull] Level level, Vector2I point)
        {
            var tile = level[point];
            Visit(tile);
        }

        private void Visit([NotNull] Tile tile)
        {
            tile.IsLit = true;
            _visible.Add(tile);
        }
    }
}
