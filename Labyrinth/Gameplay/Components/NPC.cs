using System.Runtime.Serialization;

using Labyrinth.ECS;
using Labyrinth.Gameplay.AI;

namespace Labyrinth.Gameplay.Components
{
	// this entity is driven by an AI
	[DataContract]
	public sealed class NPC : IEntityComponent
	{
		public AIType AI { get; set; }
	}
}
