using System.Runtime.Serialization;

using Labyrinth.ECS;
using Labyrinth.Geometry;

namespace Labyrinth.Gameplay.Components
{
	// the entity is part of a level
	[DataContract]
	public sealed class Spawned : IEntityComponent
	{
		public Int2 Position { get; set; }
	}
}
