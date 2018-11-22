using Labyrinth.ECS;
using Labyrinth.Geometry;

namespace Labyrinth.Gameplay.Actions
{
	public sealed class ActionOpenDoor : ActionBase
	{
		public Int2 Position { get; }

		public override ActionResult Perform(Entity self)
		{
			return base.Perform(self);
		}

		public override string ToString()
		{
			return $"open door at {Position}";
		}
	}
}
