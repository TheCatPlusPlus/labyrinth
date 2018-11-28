using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace Labyrinth.ECS
{
	public sealed class PrefabRegistry : IEnumerable<KeyValuePair<PrefabID, Prefab>>
	{
		private readonly Dictionary<PrefabID, Prefab> _prefabs;

		public Prefab this[PrefabID id] => _prefabs[id];

		public PrefabRegistry()
		{
			_prefabs = new Dictionary<PrefabID, Prefab>();
		}

		public void Add(string id, Prefab prefab)
		{
			prefab.ID = new PrefabID(id);
			_prefabs.Add(prefab.ID, prefab);
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
