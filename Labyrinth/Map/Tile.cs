using System;
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

		public TileType Type { get; set; }
		public Int2 Position { get; }
		public TileFlag ManualFlags { get; set; }
		public TileFlag EffectiveFlags => GetEffectiveFlags();

		[CanBeNull]
		public Creature Creature { get; private set; }
		public IReadOnlyList<Item> Items => _items;

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

		public TileFlag GetEffectiveFlags()
		{
			var flags = ManualFlags;

			// 1. tile type flags
			switch (Type)
			{
				case TileType.Floor:
					flags |= TileFlag.Transparent;
					if (Creature != null)
					{
						flags |= TileFlag.Solid;
					}
					break;
				case TileType.Wall:
					flags |= TileFlag.Solid;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			flags |= TileFlag.Lit;

			return flags;
		}
	}
}
