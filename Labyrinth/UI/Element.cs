namespace Labyrinth.UI
{
	public abstract class Element
	{
		protected GamePrev Game { get; }
		protected UI UI { get; }

		protected Element(GamePrev game, UI ui)
		{
			Game = game;
			UI = ui;
		}
	}
}
