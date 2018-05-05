namespace Labyrinth.UI
{
	public abstract class Element
	{
		protected Game Game { get; }
		protected UI UI { get; }

		protected Element(Game game, UI ui)
		{
			Game = game;
			UI = ui;
		}
	}
}
