using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Utils;

namespace Labyrinth.Geometry.Paths
{
	// simple line with no regard to level structure
	public static class Line
	{
		public static Path Create(Int2 start, Int2 end)
		{
			var points = DoCreate(start, end).OrderBy(p => (p - start).NormL1()).ToArray();
			return new Path(points, start, end);
		}

		[NotNull]
		private static IEnumerable<Int2> DoCreate(Int2 start, Int2 end)
		{
			var diff = (end - start).Abs();
			var steep = diff.Y > diff.X;

			var x0 = start.X;
			var y0 = start.Y;
			var x1 = end.X;
			var y1 = end.Y;

			if (steep)
			{
				MiscExt.Swap(ref x0, ref y0);
				MiscExt.Swap(ref x1, ref y1);
			}

			if (x0 > x1)
			{
				MiscExt.Swap(ref x0, ref x1);
				MiscExt.Swap(ref y0, ref y1);
			}

			var dx = x1 - x0;
			var dy = Math.Abs(y1 - y0);
			var error = dx / 2;
			var step = y0 < y1 ? 1 : -1;

			var y = y0;
			for (var x = x0; x <= x1; ++x)
			{
				var point = new Int2(x, y);

				if (steep)
				{
					point = point.Swap();
				}

				yield return point;

				error -= dy;
				if (error < 0)
				{
					y += step;
					error += dx;
				}
			}
		}
	}
}
