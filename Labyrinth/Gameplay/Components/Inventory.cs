using System.Collections.Generic;
using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	[DataContract]
	public sealed class Inventory : IEntityComponent
	{
		public List<EntityID> Items { get; private set; } = new List<EntityID>();
	}
}
