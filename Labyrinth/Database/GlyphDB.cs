using System.Collections.Generic;
using System.Drawing;

using Labyrinth.Entities;
using Labyrinth.Map;

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
