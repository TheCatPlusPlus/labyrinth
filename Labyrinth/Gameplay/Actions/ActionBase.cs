using JetBrains.Annotations;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Actions
{
	public abstract class ActionBase
	{
		public virtual ActionResult Perform(Entity self)
		{
			return Failure();
		}

		protected ActionResult Failure()
		{
			return new ActionResult(0, true);
		}

		protected ActionResult Success(int cost)
		{
			return new ActionResult(cost, true);
		}

		protected ActionResult Instead(ActionBase action)
		{
			return new ActionResult(action);
		}

		[NotNull]
		public override string ToString()
		{
			return GetType().Name;
		}
	}
}
