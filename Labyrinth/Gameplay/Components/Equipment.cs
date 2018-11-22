using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	[DataContract]
	public sealed class Equipment : IEntityComponent
	{
		public EntityID? MainHand { get; set; }
		public EntityID? OffHand { get; set; }
		public EntityID? Armor { get; set; }
		public EntityID? Helmet { get; set; }
		public EntityID? Boots { get; set; }
		public EntityID? LeftRing { get; set; }
		public EntityID? RightRing { get; set; }
		public EntityID? Amulet { get; set; }
	}
}
