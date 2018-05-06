using System.Collections.Generic;

using JetBrains.Annotations;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedMember.Global

namespace Labyrinth.Utils
{
	public static partial class DictExt
	{
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2, IDictionary<TKey, TValue> d3)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d3.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2, IDictionary<TKey, TValue> d3, IDictionary<TKey, TValue> d4)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d3.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d4.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2, IDictionary<TKey, TValue> d3, IDictionary<TKey, TValue> d4, IDictionary<TKey, TValue> d5)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d3.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d4.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d5.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2, IDictionary<TKey, TValue> d3, IDictionary<TKey, TValue> d4, IDictionary<TKey, TValue> d5, IDictionary<TKey, TValue> d6)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d3.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d4.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d5.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d6.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2, IDictionary<TKey, TValue> d3, IDictionary<TKey, TValue> d4, IDictionary<TKey, TValue> d5, IDictionary<TKey, TValue> d6, IDictionary<TKey, TValue> d7)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d3.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d4.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d5.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d6.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d7.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2, IDictionary<TKey, TValue> d3, IDictionary<TKey, TValue> d4, IDictionary<TKey, TValue> d5, IDictionary<TKey, TValue> d6, IDictionary<TKey, TValue> d7, IDictionary<TKey, TValue> d8)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d3.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d4.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d5.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d6.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d7.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d8.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
		[CanBeNull]
		public static TValue TryGetValue<TKey, TValue>(TKey key, IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2, IDictionary<TKey, TValue> d3, IDictionary<TKey, TValue> d4, IDictionary<TKey, TValue> d5, IDictionary<TKey, TValue> d6, IDictionary<TKey, TValue> d7, IDictionary<TKey, TValue> d8, IDictionary<TKey, TValue> d9)
		{
			{
				if (d1.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d2.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d3.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d4.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d5.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d6.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d7.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d8.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			{
				if (d9.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return default;
		}
	}
}
