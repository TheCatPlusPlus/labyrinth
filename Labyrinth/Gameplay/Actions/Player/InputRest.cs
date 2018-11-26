using Labyrinth.Gameplay.Components;

namespace Labyrinth.Gameplay.Actions.Player
{
	public sealed class InputRest : InputBase
	{
		public InputRest(GameState state)
			: base(state)
		{
		}

		public override bool CanPerform()
		{
			var health = State.World.Get<Killable>(State.Player);
			return health.Health < health.MaxHealth;
		}

		public override ActionBase NextAction()
		{
			return new ActionWait(State, State.Player);
		}

		public override string ToString()
		{
			return "wait until interrupted";
		}
	}
}
