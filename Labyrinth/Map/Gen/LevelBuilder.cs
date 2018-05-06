using System;
using System.Collections.Generic;

using Labyrinth.Geometry;

namespace Labyrinth.Map.Gen
{
	public sealed class LevelBuilder
	{
		public Level Level { get; }
		public string Name => Level.Name;
		public int Depth => Level.Depth;
		public int Width => Level.Width;
		public int Height => Level.Height;
		public Grid Grid => Level.Grid;

		public LevelBuilder(Level level)
		{
			Level = level;
		}

		public void Fill(TileType type, Rect? rect = null)
		{
			rect = rect ?? Grid.Rect;
			Box(type, type, rect.Value);
		}

		public void Box(TileType fill, TileType border, Rect rect)
		{
			ForEach(t => t.Type = fill, rect);
			ForEach(t => t.Type = border, rect.EdgePoints);
		}

		public void Box(TileType fill, Rect rect)
		{
			Box(fill, TileType.Wall, rect);
		}

		public void Box(Rect rect)
		{
			Box(TileType.Floor, rect);
		}

		public void ForEach(Action<Tile> action, Rect? rect = null)
		{
			rect = rect ?? Grid.Rect;
			ForEach(action, rect.Value.Points);
		}

		public void ForEach(Action<Tile> action, IEnumerable<Int2> points)
		{
			foreach (var point in points)
			{
				var tile = Grid[point];
				if (tile != null)
				{
					action(tile);
				}
			}
		}
	}
}
