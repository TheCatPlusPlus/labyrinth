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
		Solid = 1 << 3,
		// suitable for spawning
		SpawnCandidate = 1 << 4,
		// can be walked on without special effects
		Walkable = 1 << 5,
		// can be pierced through (solid but doesn't block shots)
		Piercable = 1 << 6,
		// can be opened by capable creatures
		Door = 1 << 7
	}

	public static class TileFlagExt
	{
		public static bool Contains(this TileFlag flags, TileFlag value)
		{
			return (flags & value) != 0;
		}
	}
}
