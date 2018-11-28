using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Utils;

namespace Labyrinth.ECS
{
	public sealed class World : IEnumerable<EntityID>
	{
		private sealed class Entity
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

			public void Add(IEntityComponent component)
			{
				_components[component.GetType()] = component;
			}

			public IEntityComponent Get(Type type)
			{
				var component = TryGet(type);
				if (component == null)
				{
					throw new Exception($"Entity {ID} {Name} doesn't have a {type.Name} component");
				}

				return component;
			}

			[CanBeNull]
			public IEntityComponent TryGet(Type type)
			{
				return !_components.TryGetValue(type, out var component) ? default : component;
			}

			public void Remove(Type type)
			{
				_components.Remove(type);
			}

			public Entity Clone(EntityID id)
			{
				var components = new Dictionary<Type, IEntityComponent>();

				foreach (var (type, data) in _components)
				{
					components[type] = data.DeepClone();
				}

				return new Entity(id, Name, components);
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

		private readonly Dictionary<EntityID, Entity> _entities;
		private readonly PrefabRegistry _prefabs;
		private int _nextID;

		public World(PrefabRegistry prefabs)
		{
			_prefabs = prefabs;
			_entities = new Dictionary<EntityID, Entity>();
			_nextID = 1;
		}

		public EntityID Create(string name)
		{
			var id = Allocate();
			_entities[id] = new Entity(id, name);
			return id;
		}

		public EntityID Create(PrefabID prefab)
		{
			var id = Create(prefab.Name);
			var entity = _entities[id];

			foreach (var component in _prefabs[prefab])
			{
				entity.Add(component.DeepClone());
			}

			return id;
		}

		public void Destroy(EntityID id)
		{
			_entities.Remove(id);
		}

		public EntityID Clone(EntityID id)
		{
			var cloneID = Allocate();
			_entities[cloneID] = _entities[id].Clone(cloneID);
			return cloneID;
		}

		public T Add<T>(EntityID id, T component)
			where T : class, IEntityComponent
		{
			_entities[id].Add(component);
			return component;
		}

		public void Remove<T>(EntityID id)
			where T : class, IEntityComponent
		{
			_entities[id].Remove(typeof(T));
		}

		public T Get<T>(EntityID id)
			where T : class, IEntityComponent
		{
			return (T)_entities[id].Get(typeof(T));
		}

		[CanBeNull]
		public T TryGet<T>(EntityID id)
			where T : class, IEntityComponent
		{
			return (T)_entities[id].TryGet(typeof(T));
		}

		public string Describe(EntityID id)
		{
			var entity = _entities[id];
			return $"{entity.ID} {entity.Name}";
		}

		private EntityID Allocate()
		{
			return new EntityID(_nextID++);
		}

		[NotNull]
		public IEnumerator<EntityID> GetEnumerator()
		{
			return _entities.Keys.GetEnumerator();
		}

		[NotNull]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
