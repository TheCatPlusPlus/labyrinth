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

		public Grid(int width, int height)
		{
			Rect = new Rect(0, 0, width, height);
			_tiles = new Tile[width, height];

			foreach (var point in Rect.Points)
			{
				_tiles[point.X, point.Y] = new Tile(point);
			}
		}

		public bool IsInBounds(Int2 position)
		{
			return Rect.Contains(position);
		}
	}
}
