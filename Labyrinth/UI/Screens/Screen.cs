using BearLib;

using JetBrains.Annotations;

namespace Labyrinth.UI
{
	public abstract class Screen : Element
	{
		protected Screen(GamePrev game, UI ui)
			: base(game, ui)
		{
		}

		public virtual void React(Code code)
		{
		}

		public virtual void Draw()
		{
		}

		[NotNull]
		public override string ToString()
		{
			return $"{nameof(Screen)}: {GetType().Name}";
		}
	}
}
