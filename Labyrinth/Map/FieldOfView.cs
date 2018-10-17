using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using Labyrinth.Entities;
using Labyrinth.Geometry;
using Labyrinth.Geometry.Paths;

using NLog;

namespace Labyrinth.Map
{
	public sealed class FieldOfView
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		private readonly Player _player;
		private readonly List<Tile> _visible;
		public IReadOnlyList<Tile> Visible => _visible;

		public FieldOfView(Player player)
		{
			_player = player;
			_visible = new List<Tile>();
		}

		public void Recalculate()
		{
			foreach (var tile in _visible)
			{
				tile.EnabledFlags &= ~TileFlag.Lit;
			}

			_visible.Clear();

			if (_player.Position == null || _player.Level == null)
			{
				return;
			}

			var level = _player.Level;
			var position = _player.Position.Value;
			var fov = new Rect(position, 1, 1).Inflated(_player.VisionRange);

			foreach (var goal in fov.EdgePoints)
			foreach (var point in Line.Create(position, goal))
			{
				var tile = level.Grid[point];
				if (tile == null)
				{
					continue;
				}

				Visit(tile);

				if (!tile.EffectiveFlags.Contains(TileFlag.Transparent) && point != position)
				{
					break;
				}
			}

			Visit(level, position);
		}

		private void Visit([NotNull] Level level, Int2 point)
		{
			var tile = level.Grid[point];
			if (tile != null)
			{
				Visit(tile);
			}
		}

		private void Visit([NotNull] Tile tile)
		{
			tile.EnabledFlags |= TileFlag.Lit | TileFlag.Seen;
			_visible.Add(tile);
		}
	}
}
