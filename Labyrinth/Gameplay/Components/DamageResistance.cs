using System.Runtime.Serialization;

using Labyrinth.ECS;
using Labyrinth.Entities.Damage;

namespace Labyrinth.Gameplay.Components
{
	[DataContract]
	public sealed class DamageResistance : IEntityComponent
	{
		public DamageType Type { get; set; }
		public int Degree { get; set; }
	}
}
