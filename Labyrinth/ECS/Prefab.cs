using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Labyrinth.ECS
{
	public sealed class Prefab : IEnumerable<IEntityComponent>
	{
		private readonly List<IEntityComponent> _components;

		public Prefab()
		{
			_components = new List<IEntityComponent>();
		}

		public void Add(IEntityComponent component)
		{
			_components.Add(component);
		}

		[NotNull]
		public IEnumerator<IEntityComponent> GetEnumerator()
		{
			return _components.GetEnumerator();
		}

		[NotNull]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
