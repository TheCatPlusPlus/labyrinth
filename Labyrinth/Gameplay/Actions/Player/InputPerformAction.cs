namespace Labyrinth.Gameplay.Actions.Player
{
	public sealed class InputPerformAction : InputBase
	{
		private readonly ActionBase _action;

		public InputPerformAction(GameState state, ActionBase action)
			: base(state)
		{
			_action = action;
		}

		public override ActionBase NextAction()
		{
			State.CurrentInput = null;
			return _action;
		}

		public override string ToString()
		{
			return $"perform a single action: {_action}";
		}
	}
}
