using System.Drawing;

using BearLib;

using JetBrains.Annotations;

using NLog;

namespace Labyrinth.UI
{
	public sealed class UI
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		private readonly Game _game;

		private Screen _screen;
		[CanBeNull]
		private Dialog _dialog;
		private bool _isRunning;

		public int Width { get; }
		public int Height { get; }

		public UI(Game game, int width, int height)
		{
			_game = game;
			Width = width;
			Height = height;

			Terminal.Open();
			Terminal.Set(
				$"window: title=Labyrinth, size={Width}x{Height};" +
				"input: precise-mouse=false, filter=[keyboard, mouse, system], alt-functions=false;" +
				"log: level=fatal;"
			);
			Terminal.Color(Color.White);
			Terminal.BkColor(Color.Black);
			Terminal.Refresh();

			_screen = new GameScreen(_game, this);
		}

		public void Open(Screen screen)
		{
			Log.Debug($"Open: {screen}");
			_screen = screen;
		}

		public void Open(Dialog dialog)
		{
			Log.Debug($"Open: {dialog}");
			_dialog = dialog;
		}

		public void Run()
		{
			_isRunning = true;

			while (_isRunning)
			{
				Terminal.Clear();
				_screen.Draw();
				_dialog?.Draw();
				Terminal.Refresh();

				var code = Terminal.Read();
				if (code == Code.Close)
				{
					return;
				}

				if (_dialog != null)
				{
					if (_dialog.React(code) == DialogResult.Close)
					{
						Log.Debug($"Close: {_dialog}");
						_dialog = null;
					}
				}
				else
				{
					_screen.React(code);
				}
			}
		}

		public void Quit()
		{
			_isRunning = false;
		}
	}
}
