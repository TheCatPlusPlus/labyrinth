using System.Collections.Generic;

using Labyrinth.Map;

namespace Labyrinth.Geometry.Paths
{
	// ray that takes into account blocking/piercing properties of the
	// tiles (and also will bounce in the future)
	//
	// this is used to implement projectiles (spells, ammo, throwables, etc)
	public static class Ray
	{
		public static Path Cast(Level level, Int2 start, Int2 target, bool piercing = false)
		{
			var points = new List<Int2>();

			// we need to start closest to the start and move outwards
			// otherwise collisions might be checked from the wrong side
			// and the resulting path will be something like
			//   ...Xxx#...@
			// instead of
			//   ......#xxx@
			// (this is due to coordinate transformations needed in Bresenham's algorithm)

			foreach (var point in Line.Create(start, target))
			{
				if (point == start)
				{
					continue;
				}

				var tile = level.Grid[point];
				if (tile == null)
				{
					break;
				}

				points.Add(point);

				var flags = tile.EffectiveFlags;
				if (flags.Contains(TileFlag.Solid) && (!flags.Contains(TileFlag.Piercable) || !piercing))
				{
					break;
				}
			}

			return new Path(points, start, target);
		}
	}
}
