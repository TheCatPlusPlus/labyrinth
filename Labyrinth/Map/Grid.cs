using JetBrains.Annotations;

using Labyrinth.Geometry;

namespace Labyrinth.Map
{
	public sealed class Grid
	{
		private readonly Tile[,] _tiles;

		public Rect Rect { get; }

		[CanBeNull]
		public Tile this[Int2 position] => IsInBounds(position) ? _tiles[position.X, position.Y] : null;

		[CanBeNull]
		public Tile this[int x, int y] => this[new Int2(x, y)];

		public Grid(Level level, int width, int height)
		{
			Rect = new Rect(0, 0, width, height);
			_tiles = new Tile[width, height];

			foreach (var point in Rect.Points)
			{
				_tiles[point.X, point.Y] = new Tile(level, point);
			}
		}

		public bool IsInBounds(Int2 position)
		{
			return Rect.Contains(position);
		}
	}
}
