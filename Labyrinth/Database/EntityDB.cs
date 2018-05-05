using System.Collections.Generic;

using Labyrinth.Entities;

namespace Labyrinth.Database
{
	public sealed class EntityDB
	{
		private readonly Dictionary<string, EntityData> _entities = new Dictionary<string, EntityData>
		{
			{ Player.PlayerID.Value, new EntityData("Player", unique: true, proper: true) }
		};

		public EntityData Get(EntityID id)
		{
			return _entities[id.Value];
		}
	}
}
