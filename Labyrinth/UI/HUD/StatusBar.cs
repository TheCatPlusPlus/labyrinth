using Labyrinth.Geometry;

namespace Labyrinth.UI.HUD
{
	public sealed class StatusBar
	{
		private readonly Game _game;

		public Rect Rect { get; }

		public StatusBar(Game game, Rect rect)
		{
			_game = game;
			Rect = rect;
		}

		public void Draw()
		{
		}
	}
}
