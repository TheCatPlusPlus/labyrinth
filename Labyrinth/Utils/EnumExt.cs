using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Labyrinth.Utils
{
	public static class EnumExt
	{
		public static IEnumerable<T> GetValues<T>()
			where T : struct//, Enum // R# doesn't support this yet
		{
			Debug.Assert(typeof(T).IsEnum, $"typeof({typeof(T).Name}).IsEnum");
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}
