using System.Collections.Generic;
using System.Drawing;

using Labyrinth.Entities;
using Labyrinth.Map;

namespace Labyrinth.UI
{
	public sealed class GlyphDB
	{
		public static readonly Glyph OutOfBounds = new Glyph(' ');
		public static readonly Glyph MultipleItems = new Glyph('%')
		{
			Fore = Color.White
		};

		private static readonly Dictionary<TileType, Glyph> Tiles = new Dictionary<TileType, Glyph>
		{
			{
				TileType.Floor, new Glyph('.')
				{
					Fore = Color.FromArgb(0x55, 0x55, 0x55)
				}
			},
			{
				TileType.Wall, new Glyph(' ')
				{
					Back = Color.FromArgb(0x50, 0x50, 0x50)
				}
			}
		};

		private static readonly Dictionary<string, Glyph> Entities = new Dictionary<string, Glyph>
		{
			{
				Player.PlayerID.Value, new Glyph('@')
				{
					Fore = Color.White
				}
			}
		};

		public static Glyph Get(EntityID id)
		{
			return Entities[id.Value];
		}

		public static Glyph Get(TileType id)
		{
			return Tiles[id];
		}
	}
}
