using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using Labyrinth.Data.Ids;

namespace Labyrinth.Data
{
    public sealed class Registry<TData> : IEnumerable<TData>
        where TData : class, IHasId
    {
        private readonly Dictionary<string, TData> _items;

        public Registry()
        {
            _items = new Dictionary<string, TData>();
        }

        [NotNull]
        public TData Get([NotNull] IId id)
        {
            return _items[id.Value];
        }

        [CanBeNull]
        public TData GetOrDefault([NotNull] IId id)
        {
            return _items.TryGetValue(id.Value, out TData value) ? value : null;
        }

        public void Add([NotNull] TData item)
        {
            _items.Add(item.Id.Value, item);
        }

        public IEnumerator<TData> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class Registry<T, TData> : IEnumerable<TData>
        where T : class, IHasId<T>
        where TData : class, IHasId<T>
    {
        private readonly Registry<TData> _items;

        public Registry()
        {
            _items = new Registry<TData>();
        }

        [NotNull]
        public TData Get([NotNull] Id<T> id)
        {
            return _items.Get(id);
        }

        [CanBeNull]
        public TData GetOrDefault([NotNull] Id<T> id)
        {
            return _items.GetOrDefault(id);
        }

        public void Add([NotNull] TData item)
        {
            _items.Add(item);
        }

        public IEnumerator<TData> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
