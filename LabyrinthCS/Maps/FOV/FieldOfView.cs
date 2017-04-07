using System.Collections.Generic;
using System.Drawing;

using Labyrinth.Entities;
using Labyrinth.Utils;

namespace Labyrinth.Maps.FOV
{
    public class FieldOfView
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
            var fov = new Rectangle(_player.Position, new Size(1, 1));
            fov.Inflate(Const.FovRadius, Const.FovRadius);
            foreach (var point in fov.Points())
            {
                if (level.Rect.Contains(point))
                {
                    Visit(level, point);
                }
            }
        }

        private void Visit(Level level, Point point)
        {
            var tile = level[point];
            tile.IsLit = true;
            _visible.Add(tile);
        }
    }
}
