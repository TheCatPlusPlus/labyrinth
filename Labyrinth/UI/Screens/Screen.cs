using BearLib;

using JetBrains.Annotations;

namespace Labyrinth.UI
{
	public abstract class Screen
	{
		protected Game Game { get; }

		protected Screen(Game game)
		{
			Game = game;
		}

		public virtual void React(Code code, UI ui)
		{
		}

		public virtual void Draw(UI ui)
		{
		}

		[NotNull]
		public override string ToString()
		{
			return $"{nameof(Screen)}: {GetType().Name}";
		}
	}
}
