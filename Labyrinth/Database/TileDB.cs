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
					Flags = TileFlag.SpawnCandidate | TileFlag.Walkable
				}
			},
			{
				TileType.Wall, new TileData("wall")
				{
					Flags = TileFlag.Solid
				}
			},
			{
				TileType.DeepWall, new TileData("wall")
				{
					Flags = TileFlag.Solid
				}
			},
			{
				TileType.GlassWall, new TileData("glass wall")
				{
					Flags = TileFlag.Solid | TileFlag.Transparent
				}
			},
			{
				TileType.DoorOpen, new TileData("open door")
				{
					Flags = TileFlag.Walkable
				}
			},
			{
				TileType.DoorClosed, new TileData("closed door")
				{
					Flags = TileFlag.Solid | TileFlag.Door
				}
			},
			{
				TileType.StairsUp, new TileData("staircase leading up", "staircases leading up")
				{
					Flags = TileFlag.Walkable
				}
			},
			{
				TileType.StairsDown, new TileData("staircase leading down", "staircases leading down")
				{
					Flags = TileFlag.Walkable
				}
			},
			{
				TileType.Water, new TileData("water", countable: false)
				{
					Flags = TileFlag.Walkable,
					CostMultiplier = 2
				}
			},
			{
				TileType.DeepWater, new TileData("deep water", countable: false)
				{
					CostMultiplier = 3
				}
			},
			{
				TileType.Lava, new TileData("lava", countable: false)
				{
					CostMultiplier = 3
				}
			}
		};

		public TileData Get(TileType type)
		{
			return _tiles[type];
		}
	}
}
