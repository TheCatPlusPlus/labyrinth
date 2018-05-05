using BearLib;

using JetBrains.Annotations;

namespace Labyrinth.UI
{
	public abstract class Dialog : Element
	{
		protected Dialog(Game game, UI ui)
			: base(game, ui)
		{
		}

		public virtual DialogResult React(Code code)
		{
			return DialogResult.StayOpen;
		}

		public virtual void Draw()
		{
		}

		[NotNull]
		public override string ToString()
		{
			return $"{nameof(Dialog)}: {GetType().Name}";
		}
	}
}
