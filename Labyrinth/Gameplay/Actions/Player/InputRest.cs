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
			var health = State.Player.Get<Killable>();
			return health.Health < health.MaxHealth;
		}

		public override ActionBase NextAction()
		{
			return new ActionWait();
		}

		public override string ToString()
		{
			return "wait until interrupted";
		}
	}
}
