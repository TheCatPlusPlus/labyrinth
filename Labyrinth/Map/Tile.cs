using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Database;
using Labyrinth.Entities;
using Labyrinth.Geometry;

namespace Labyrinth.Map
{
	public sealed class Tile
	{
		private readonly List<Item> _items;
		private readonly Dictionary<EntityID, int> _itemCount;

		public TileType Type { get; set; }
		public Int2 Position { get; }
		public Level Level { get; }
		public TileFlag EnabledFlags { get; set; }
		public TileFlag DisabledFlags { get; set; }
		public TileFlag EffectiveFlags => GetEffectiveFlags();
		public IEnumerable<Tile> Neighbours => GetNeighbours();

		[CanBeNull]
		public Creature Creature { get; private set; }
		public IReadOnlyList<Item> Items => _items;
		public IReadOnlyDictionary<EntityID, int> ItemCount => _itemCount;

		public Tile(Level level, Int2 position)
		{
			Position = position;
			Level = level;
			_items = new List<Item>();
			_itemCount = new Dictionary<EntityID, int>();
		}

		public void Add(Entity entity)
		{
			switch (entity)
			{
				case Creature creature:
					Creature = creature;
					break;
				case Item item:
					_items.Add(item);
					_itemCount.TryAdd(item.ID, 0);
					_itemCount[item.ID]++;
					break;
			}
		}

		public void Remove(Entity entity)
		{
			Debug.Assert(entity.Position == Position, "entity.Position == Position");

			switch (entity)
			{
				case Creature creature:
					Debug.Assert(Creature == creature, "Creature == creature");
					Creature = null;
					break;
				case Item item:
					var removed = _items.Remove(item);
					Debug.Assert(removed, "removed");
					Debug.Assert(_itemCount.ContainsKey(item.ID));
					if (--_itemCount[item.ID] == 0)
					{
						_itemCount.Remove(item.ID);
					}

					break;
			}
		}

		public bool Contains(Entity entity)
		{
			switch (entity)
			{
				case Creature creature:
					return Creature == creature;
				case Item item:
					return _items.Contains(item);
			}

			return false;
		}

		private TileFlag GetEffectiveFlags()
		{
			var flags = TileFlag.None;

			// 1. static flags
			var data = DB.Tiles.Get(Type);
			flags |= data.Flags;

			// 2. dynamic flags
			if (Creature != null)
			{
				flags |= TileFlag.Solid;
			}

			if (!flags.Contains(TileFlag.Solid))
			{
				flags |= TileFlag.Transparent;
			}
			else
			{
				flags &= ~TileFlag.Walkable;
			}

			// 3. force enabled flags (FOV, terraforming, etc)
			flags |= EnabledFlags;

			// 4. force disabled flags (magic effects etc)
			flags &= ~DisabledFlags;

			// TODO for testing
			flags |= TileFlag.Lit | TileFlag.Seen;
			return flags;
		}

		private IEnumerable<Tile> GetNeighbours()
		{
			for (var dx = -1; dx <= 1; ++dx)
			for (var dy = -1; dy <= 1; ++dy)
			{
				var p = Position + new Int2(dx, dy);

				if (p == Position)
				{
					continue;
				}

				var tile = Level.Grid[p];
				if (tile != null)
				{
					yield return tile;
				}
			}
		}
	}
}
