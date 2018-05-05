using System.Collections.Generic;

using Labyrinth.Map;

namespace Labyrinth.Database
{
	public sealed class TileDB
	{
		private readonly Dictionary<TileType, TileData> _tiles = new Dictionary<TileType, TileData>
		{
			{
				TileType.Floor, new TileData("floor", countable: false, unique: true)
				{
					Flags = TileFlag.SpawnCandidate | TileFlag.Transparent
				}
			},
			{
				TileType.Wall, new TileData("wall")
				{
					Flags = TileFlag.Solid
				}
			}
		};

		public TileData Get(TileType type)
		{
			return _tiles[type];
		}
	}
}
