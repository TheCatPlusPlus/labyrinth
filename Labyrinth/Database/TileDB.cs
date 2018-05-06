using System.Collections.Generic;
using System.Drawing;

using Labyrinth.Map;
using Labyrinth.Utils;

namespace Labyrinth.Database
{
	public sealed class TileDB
	{
		public readonly GlyphData OutOfBounds = new GlyphData(' ');
		public readonly GlyphData MultipleItems = new GlyphData('%')
		{
			Fore = Color.White
		};

		private readonly Dictionary<TileType, TileData> _tiles = new Dictionary<TileType, TileData>
		{
			{
				TileType.Floor, new TileData("floor", countable: false, unique: true)
				{
					Flags = TileFlag.SpawnCandidate | TileFlag.Walkable,
					Glyph = new GlyphData('.')
					{
						Fore = Color.FromArgb(0x55, 0x55, 0x55)
					}
				}
			},
			{
				TileType.Wall, new TileData("wall")
				{
					Flags = TileFlag.Solid,
					Glyph = new GlyphData(' ')
					{
						Back = Color.FromArgb(0x50, 0x50, 0x50)
					}
				}
			},
			{
				TileType.DeepWall, new TileData("wall")
				{
					Flags = TileFlag.Solid,
					Glyph = new GlyphData(' ')
				}
			},
			{
				TileType.GlassWall, new TileData("glass wall")
				{
					Flags = TileFlag.Solid | TileFlag.Transparent,
					Glyph = new GlyphData(' ')
					{
						Back = Color.SteelBlue
					}
				}
			},
			{
				TileType.DoorOpen, new TileData("open door")
				{
					Flags = TileFlag.Walkable,
					Glyph = new GlyphData('/')
					{
						Fore = Color.FromArgb(0xC9, 0x76, 0x00)
					}
				}
			},
			{
				TileType.DoorClosed, new TileData("closed door")
				{
					Flags = TileFlag.Solid | TileFlag.Door,
					Glyph = new GlyphData('+')
					{
						Fore = Color.FromArgb(0xC9, 0x76, 0x00)
					}
				}
			},
			{
				TileType.StairsUp, new TileData("staircase leading up", "staircases leading up")
				{
					Flags = TileFlag.Walkable,
					Glyph = new GlyphData('<')
					{
						Fore = Color.White
					}
				}
			},
			{
				TileType.StairsDown, new TileData("staircase leading down", "staircases leading down")
				{
					Flags = TileFlag.Walkable,
					Glyph = new GlyphData('>')
					{
						Fore = Color.White
					}
				}
			},
			{
				TileType.Water, new TileData("water", countable: false)
				{
					Flags = TileFlag.Walkable,
					CostMultiplier = 2,
					Glyph = new GlyphData('~')
					{
						Fore = Color.SteelBlue.Lighten(0.25f),
						AlwaysDrawBack = true
					}
				}
			},
			{
				TileType.DeepWater, new TileData("deep water", countable: false)
				{
					CostMultiplier = 3,
					Glyph = new GlyphData('~')
					{
						Fore = Color.SteelBlue.Darken(0.25f),
						AlwaysDrawBack = true
					}
				}
			},
			{
				TileType.Lava, new TileData("lava", countable: false)
				{
					CostMultiplier = 3,
					Glyph = new GlyphData('~')
					{
						Fore = Color.DarkRed,
						AlwaysDrawBack = true
					}
				}
			}
		};

		public TileData Get(TileType type)
		{
			return _tiles[type];
		}
	}
}
