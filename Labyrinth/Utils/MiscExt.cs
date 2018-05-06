using System.Collections.Generic;

namespace Labyrinth.Utils
{
	public static class MiscExt
	{
		public static void Swap<T>(ref T left, ref T right)
		{
			var temp = left;
			left = right;
			right = temp;
		}

		public static void Swap<T>(this IList<T> items, int left, int right)
		{
			var temp = items[left];
			items[left] = items[right];
			items[right] = temp;
		}
	}
}
