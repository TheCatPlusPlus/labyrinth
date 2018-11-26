using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Labyrinth.ECS
{
	public sealed class PrefabRegistry : IEnumerable<KeyValuePair<PrefabID, Prefab>>
	{
		private readonly Dictionary<PrefabID, Prefab> _prefabs;

		public PrefabRegistry()
		{
			_prefabs = new Dictionary<PrefabID, Prefab>();
		}

		public void Add(string id, Prefab prefab)
		{
			_prefabs.Add(new PrefabID(id), prefab);
		}

		[NotNull]
		public IEnumerator<KeyValuePair<PrefabID, Prefab>> GetEnumerator()
		{
			return _prefabs.GetEnumerator();
		}

		[NotNull]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
