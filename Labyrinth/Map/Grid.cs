using JetBrains.Annotations;

using Labyrinth.Geometry;

namespace Labyrinth.Map
{
	public sealed class Grid
	{
		private readonly Tile[,] _tiles;
		private readonly Rect _rect;

		[CanBeNull]
		public Tile this[Int2 position] => IsInBounds(position) ? _tiles[position.X, position.Y] : null;

		public Grid(int width, int height)
		{
			_rect = new Rect(0, 0, width, height);
			_tiles = new Tile[width, height];

			foreach (var point in _rect.Points)
			{
				_tiles[point.X, point.Y] = new Tile();
			}
		}

		public bool IsInBounds(Int2 position)
		{
			return _rect.Contains(position);
		}
	}
}
