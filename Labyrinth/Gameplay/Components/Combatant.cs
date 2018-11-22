using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	// this entity can attack others
	[DataContract]
	public sealed class Combatant : IEntityComponent
	{
		public int AttackSpeed { get; set; }
	}
}
