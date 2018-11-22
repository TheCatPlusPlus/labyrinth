using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	// this entity participates in turn processing
	[DataContract]
	public sealed class Schedulable : IEntityComponent
	{
		public int Energy { get; set; }
	}
}
