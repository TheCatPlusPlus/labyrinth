using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	// this entity can move
	[DataContract]
	public sealed class Mobile : IEntityComponent
	{
		public int MoveSpeed { get; set; }
		public bool IsFlying { get; set; }
	}
}
