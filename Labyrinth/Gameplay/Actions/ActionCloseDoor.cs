using JetBrains.Annotations;

using Labyrinth.ECS;
using Labyrinth.Geometry;

namespace Labyrinth.Gameplay.Actions
{
	public sealed class ActionCloseDoor : ActionBase
	{
		private readonly Int2 _position;

		public ActionCloseDoor(GameState state, EntityID self, Int2 position)
			: base(state, self)
		{
			_position = position;
		}

		public override ActionResult Perform()
		{
			return base.Perform();
		}

		public override string ToString()
		{
			return $"close door at {_position}";
		}
	}
}
