using BearLib;

using JetBrains.Annotations;

namespace Labyrinth.UI
{
	public abstract class Dialog
	{
		protected Game Game { get; }

		protected Dialog(Game game)
		{
			Game = game;
		}

		public virtual DialogResult React(Code code, UI ui)
		{
			return DialogResult.StayOpen;
		}

		public virtual void Draw(UI ui)
		{
		}

		[NotNull]
		public override string ToString()
		{
			return $"{nameof(Dialog)}: {GetType().Name}";
		}
	}
}
