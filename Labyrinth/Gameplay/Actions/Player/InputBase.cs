using JetBrains.Annotations;

namespace Labyrinth.Gameplay.Actions.Player
{
	// player input results in one or more game actions
	public abstract class InputBase
	{
		protected readonly GameState State;

		protected InputBase(GameState state)
		{
			State = state;
		}

		public virtual bool CanPerform()
		{
			return false;
		}

		public abstract ActionBase NextAction();

		[NotNull]
		public override string ToString()
		{
			return GetType().Name;
		}
	}
}
