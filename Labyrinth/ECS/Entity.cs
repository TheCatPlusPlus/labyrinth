using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Utils;

namespace Labyrinth.ECS
{
	public sealed class Entity
	{
		private readonly Dictionary<Type, IEntityComponent> _components;

		public string Name { get; }
		public EntityID ID { get; }

		public Entity(EntityID id, string name)
			: this(id, name, new Dictionary<Type, IEntityComponent>())
		{
		}

		private Entity(EntityID id, string name, Dictionary<Type, IEntityComponent> components)
		{
			Name = name;
			ID = id;
			_components = components;
		}

		public void Add<T>(T component)
			where T : class, IEntityComponent
		{
			_components[typeof(T)] = component;
		}

		public T Get<T>()
			where T : class, IEntityComponent
		{
			var component = TryGet<T>();
			if (component == null)
			{
				throw new Exception($"Entity {ID} {Name} doesn't have a {typeof(T).Name} component");
			}

			return component;
		}

		[CanBeNull]
		public T TryGet<T>()
			where T : class, IEntityComponent
		{
			return !_components.TryGetValue(typeof(T), out var component) ? default : (T)component;
		}

		public void Remove<T>()
			where T : class, IEntityComponent
		{
			_components.Remove(typeof(T));
		}

		public Entity Clone(EntityID id)
		{
			return new Entity(id, Name, _components.DeepClone());
		}

		[NotNull]
		public override string ToString()
		{
			if (_components.Count == 0)
			{
				return $"{ID} {Name} (0)";
			}

			var components = _components.Values.Select(c => c.ToString()).ToArray();
			var joined = string.Join(" | ", components);
			return $"{ID} {Name} ({components.Length}): {joined}";
		}
	}
}
