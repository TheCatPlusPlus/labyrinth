using System.Collections.Generic;
using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	[DataContract]
	public sealed class Loot : IEntityComponent
	{
		public List<(PrefabID Item, float Chance)> Drops { get; private set; } = new List<(PrefabID, float)>();
	}
}
