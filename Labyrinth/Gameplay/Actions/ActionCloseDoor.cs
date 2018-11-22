using JetBrains.Annotations;

using Labyrinth.ECS;
using Labyrinth.Geometry;

namespace Labyrinth.Gameplay.Actions
{
	public sealed class ActionCloseDoor : ActionBase
	{
		public Int2 Position { get; }

		public override ActionResult Perform(Entity self)
		{
			return base.Perform(self);
		}

		public override string ToString()
		{
			return $"close door at {Position}";
		}
	}
}
