using System.Collections.Generic;
using System.Diagnostics;

namespace Labyrinth.Utils
{
	public static class RandomExt
	{
		public static bool NextBool(this Xoshiro256StarStar rng)
		{
			var x = rng.NextULong();
			return (x & (1UL << 63)) != 0;
		}

		public static long NextLong(this Xoshiro256StarStar rng)
		{
			return (long)rng.NextULong();
		}

		public static int NextInt(this Xoshiro256StarStar rng)
		{
			return (int)rng.NextLong();
		}

		public static uint NextUInt(this Xoshiro256StarStar rng)
		{
			return (uint)rng.NextULong();
		}

		public static int NextIntRange(this Xoshiro256StarStar rng, int min, int max)
		{
			Debug.Assert(min < max, "min < max");
			return min + rng.NextIntRange(max);
		}

		public static int NextIntRange(this Xoshiro256StarStar rng, int max)
		{
			if (max <= 0)
			{
				return 0;
			}

			var threshold = (0x7fffffff - max + 1) % max;
			while (true)
			{
				var result = (int)(rng.NextLong() & 0x7fffffff);
				if (result >= threshold)
				{
					return result % max;
				}
			}
		}

		public static long NextLongRange(this Xoshiro256StarStar rng, long min, long max)
		{
			Debug.Assert(min < max, "min < max");
			return min + rng.NextLongRange(max);
		}

		public static long NextLongRange(this Xoshiro256StarStar rng, long max)
		{
			if (max <= 0)
			{
				return 0;
			}

			var threshold = (0x7fffffffffffffffL - max + 1) % max;
			while (true)
			{
				var result = rng.NextLong() & 0x7fffffffffffffffL;
				if (result >= threshold)
				{
					return result % max;
				}
			}
		}

		public static double NextDoubleRange(this Xoshiro256StarStar rng, double min, double max)
		{
			Debug.Assert(min < max, "min < max");
			return rng.NextDouble() * (max - min) + min;
		}

		public static T Pick<T>(this Xoshiro256StarStar rng, IList<T> choices)
		{
			var index = rng.NextIntRange(choices.Count);
			return choices[index];
		}

		public static void Shuffle<T>(this Xoshiro256StarStar rng, IList<T> items)
		{
			var n = items.Count;
			while (n > 1)
			{
				n--;
				var k = rng.NextIntRange(n + 1);
				items.Swap(k, n);
			}
		}
	}
}
