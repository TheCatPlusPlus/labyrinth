using System.Collections.Generic;
using System.Drawing;

using Labyrinth.Entities;
using Labyrinth.Map;
using Labyrinth.Utils;

namespace Labyrinth.Database
{
	public sealed class GlyphDB
	{
		private readonly Dictionary<TileType, GlyphData> _tiles = new Dictionary<TileType, GlyphData>
		{
			{
				TileType.Floor, new GlyphData('.')
				{
					Fore = Color.FromArgb(0x55, 0x55, 0x55)
				}
			},
			{
				TileType.Wall, new GlyphData(' ')
				{
					Back = Color.FromArgb(0x50, 0x50, 0x50)
				}
			},
			{
				TileType.DeepWall, new GlyphData(' ')
			},
			{
				TileType.DoorOpen, new GlyphData('/')
				{
					Fore = Color.FromArgb(0xC9, 0x76, 0x00)
				}
			},
			{
				TileType.DoorClosed, new GlyphData('+')
				{
					Fore = Color.FromArgb(0xC9, 0x76, 0x00)
				}
			},
			{
				TileType.Water, new GlyphData('~')
				{
					Fore = Color.SteelBlue.Lighten(0.25f),
					AlwaysDrawBack = true
				}
			},
			{
				TileType.DeepWater, new GlyphData('~')
				{
					Fore = Color.SteelBlue.Darken(0.25f),
					AlwaysDrawBack = true
				}
			},
			{
				TileType.Lava, new GlyphData('~')
				{
					Fore = Color.DarkRed,
					AlwaysDrawBack = true
				}
			},
			{
				TileType.GlassWall, new GlyphData(' ')
				{
					Back = Color.SteelBlue
				}
			},
			{
				TileType.StairsUp, new GlyphData('<')
				{
					Fore = Color.White
				}
			},
			{
				TileType.StairsDown, new GlyphData('>')
				{
					Fore = Color.White
				}
			}
		};

		private readonly Dictionary<string, GlyphData> _entities = new Dictionary<string, GlyphData>
		{
			{
				Player.PlayerID.Value, new GlyphData('@')
				{
					Fore = Color.White
				}
			}
		};

		public readonly GlyphData OutOfBounds = new GlyphData(' ');
		public readonly GlyphData MultipleItems = new GlyphData('%')
		{
			Fore = Color.White
		};

		public GlyphData Get(EntityID id)
		{
			return _entities[id.Value];
		}

		public GlyphData Get(TileType id)
		{
			return _tiles[id];
		}
	}
}
