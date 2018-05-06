using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Labyrinth.Geometry
{
	public struct Path : IEnumerable<Int2>
	{
		public IReadOnlyList<Int2> Points { get; }
		public Int2 Start { get; }
		public Int2 End { get; }

		public Path(IReadOnlyList<Int2> points, Int2 start, Int2 end)
		{
			Points = points;
			Start = start;
			End = end;
		}

		[NotNull]
		public IEnumerator<Int2> GetEnumerator()
		{
			return Points.GetEnumerator();
		}

		[NotNull]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
