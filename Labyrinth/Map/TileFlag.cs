using System;

namespace Labyrinth.Map
{
	[Flags]
	public enum TileFlag : ulong
	{
		None = 0,

		// currently lit tile
		Lit = 1 << 0,
		// previously seen tile
		Seen = 1 << 1,
		// can be seen through
		Transparent = 1 << 2,

		// something solid (wall or creature)
		Solid = 1 << 3
	}

	public static class TileFlagExt
	{
		public static bool Contains(this TileFlag flags, TileFlag value)
		{
			return (flags & value) != 0;
		}
	}
}
