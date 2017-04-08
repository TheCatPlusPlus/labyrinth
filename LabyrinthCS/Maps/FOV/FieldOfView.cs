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

            // TODO real FOV
            var fov = new Rect(_player.Position, 1, 1).Inflated(Const.FovRadius);
            foreach (var point in fov.Points)
            {
                if (level.Rect.Contains(point))
                {
                    Visit(level, point);
                }
            }
        }

        private void Visit([NotNull] Level level, Vector2I point)
        {
            var tile = level[point];
            tile.IsLit = true;
            _visible.Add(tile);
        }
    }
}
