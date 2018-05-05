using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Entities;
using Labyrinth.Geometry;

namespace Labyrinth.Map
{
	public sealed class Tile
	{
		private readonly List<Item> _items;
		private bool _isLit;

		public TileType Type { get; set; }
		public Int2 Position { get; }

		[CanBeNull]
		public Creature Creature { get; private set; }
		public IReadOnlyList<Item> Items => _items;

		public bool WasSeen { get; private set; }

		public bool IsLit
		{
			get => _isLit;
			set
			{
				_isLit = value;
				if (value)
				{
					WasSeen = true;
				}
			}
		}

		public Tile(Int2 position)
		{
			Position = position;
			_items = new List<Item>();
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
	}
}
