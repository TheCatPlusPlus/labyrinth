using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	[DataContract]
	public sealed class Killable : IEntityComponent
	{
		public int Health { get; set; }
		public int MaxHealth { get; set; }

		public bool IsAlive => Health > 0 && MaxHealth > 0;
	}
}
