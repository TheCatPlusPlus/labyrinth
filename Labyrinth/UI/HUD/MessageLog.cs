using System.Drawing;

using BearLib;

using Labyrinth.Geometry;
using Labyrinth.Utils;

namespace Labyrinth.UI.HUD
{
	public sealed class MessageLog
	{
		private readonly Game _game;

		public Rect Rect { get; }

		public MessageLog(Game game, Rect rect)
		{
			_game = game;
			Rect = rect;
		}

		public void Draw()
		{
			var y = Rect.Bottom;
			var message = _game.Messages.Last;
			var fg = Color.White;

			Rect GetRect(int h) => new Rect(Rect.X, y, Rect.Width, h);

			while ((y > Rect.Top) && (message != null))
			{
				var text = message.Value.Text;
				var height = Terminal.Measure(new Size(Rect.Width, 0), text).Height;
				y -= height;

				using (TerminalExt.Foreground(fg))
				{
					Terminal.Print(GetRect(height), Alignment.TopLeft, text);
				}

				fg = Color.DarkGray.Darken(0.6f);
				message = message.Previous;
			}
		}
	}
}
