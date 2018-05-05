using JetBrains.Annotations;

using Labyrinth.Entities;

namespace Labyrinth.Map
{
	public sealed class Tile
	{
		public TileType Type { get; set; }
		[CanBeNull]
		public Creature Creature { get; private set; }

		public void Add(Entity entity)
		{

		}

		public void Remove(Entity entity)
		{

		}

		public bool Contains(Entity entity)
		{
			return true;
		}
	}
}
