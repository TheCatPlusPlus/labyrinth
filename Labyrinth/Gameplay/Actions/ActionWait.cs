using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Actions
{
	public sealed class ActionWait : ActionBase
	{
		public ActionWait(GameState state, EntityID self)
			: base(state, self)
		{
		}

		public override string ToString()
		{
			return "wait a single turn";
		}
	}
}
