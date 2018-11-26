using JetBrains.Annotations;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Actions
{
	public abstract class ActionBase
	{
		protected readonly GameState State;
		protected readonly EntityID Self;

		protected World World => State.World;

		protected ActionBase(GameState state, EntityID self)
		{
			State = state;
			Self = self;
		}

		public virtual ActionResult Perform()
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
