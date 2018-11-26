using Labyrinth.ECS;

using NLog.Targets;

namespace Labyrinth.Gameplay.Actions
{
	public sealed class ActionAttack : ActionBase
	{
		private readonly EntityID _target;

		public ActionAttack(GameState state, EntityID self, EntityID target)
			: base(state, self)
		{
			_target = target;
		}

		public override ActionResult Perform()
		{
			return base.Perform();
		}

		public override string ToString()
		{
			return $"attack {World.Describe(_target)}";
		}
	}
}
