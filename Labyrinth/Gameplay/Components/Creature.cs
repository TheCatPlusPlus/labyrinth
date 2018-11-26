using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	public sealed class Creature : IEntityComponent
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsUnique { get; set; }
		public bool IsChampion { get; set; }
	}
}
