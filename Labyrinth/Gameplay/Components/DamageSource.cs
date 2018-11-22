using System.Runtime.Serialization;

using Labyrinth.ECS;
using Labyrinth.Entities.Damage;
using Labyrinth.Utils;

namespace Labyrinth.Gameplay.Components
{
	[DataContract]
	public sealed class DamageSource : IEntityComponent
	{
		public DamageType Type { get; set; }
		public Die Roll { get; set; }
	}
}
