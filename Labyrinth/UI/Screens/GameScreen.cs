using BearLib;

using Labyrinth.Geometry;
using Labyrinth.UI.HUD;

using NLog;

namespace Labyrinth.UI
{
	public sealed class GameScreen : Screen
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		private readonly Viewport _viewport;
		private readonly StatusBar _statusBar;
		private readonly MessageLog _messageLog;

		public GameScreen(Game game, UI ui)
			: base(game, ui)
		{
			var messageLog = new Rect(0, 0, ui.Width, 4);
			var statusBar = new Rect(0, ui.Height - 4, ui.Width, 3);
			var viewport = new Rect(0, messageLog.Height + 1, ui.Width, ui.Height - messageLog.Height - statusBar.Height - 2);

			_viewport = new Viewport(game, viewport);
			_statusBar = new StatusBar(game, statusBar);
			_messageLog = new MessageLog(game, messageLog);
		}

		public override void React(Code code)
		{
			if (Terminal.Check(Code.Shift))
			{
				ReactShift(code);
			}
			else
			{
				ReactNonShift(code);
			}
		}

		private void ReactShift(Code code)
		{
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (code)
			{
				case Code.Period:
					Game.MoveDown();
					break;
				case Code.Comma:
					Game.MoveUp();
					break;
				case Code.Q:
					AskQuit(true);
					break;
			}
		}

		private void ReactNonShift(Code code)
		{
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (code)
			{
				case Code.Num8:
				case Code.Up:
					Game.Move(Direction.North);
					break;
				case Code.Num2:
				case Code.Down:
					Game.Move(Direction.South);
					break;
				case Code.Num6:
				case Code.Right:
					Game.Move(Direction.East);
					break;
				case Code.Num4:
				case Code.Left:
					Game.Move(Direction.West);
					break;
				case Code.Num9:
					Game.Move(Direction.NorthEast);
					break;
				case Code.Num7:
					Game.Move(Direction.NorthWest);
					break;
				case Code.Num3:
					Game.Move(Direction.SouthEast);
					break;
				case Code.Num1:
					Game.Move(Direction.SouthWest);
					break;
				case Code.Period:
				case Code.Num5:
					Game.Wait();
					break;
				case Code.I:
					// ui.Open(new InventoryScreen(Game));
					break;
				case Code.T:
					// ui.Open(new PickItemScreen(Game, Throwable));
					break;
				case Code.U:
					// ui.Open(new PickItemScreen(Game, Usable));
					break;
				case Code.Q:
					AskQuit(false);
					break;
			}
		}

		private void AskQuit(bool discard)
		{
			Log.Debug($"AskQuit({discard})");

			var message = discard
				? "Quit without saving? [color=red]NOTE[/color]: this will discard existing save! (not implemented yet)"
				: "Save and quit? (not implemented yet)";
			var confirm = new ConfirmDialog(Game, UI, message);

			confirm.Yes += () =>
			{
				if (discard)
				{
					Game.Discard();
				}
				else
				{
					Game.Save();
				}

				UI.Quit();
			};

			UI.Open(confirm);
		}

		public override void Draw()
		{
			Terminal.Put(0, UI.Height - 3, '3');
			Terminal.Put(0, UI.Height - 2, '2');
			Terminal.Put(0, UI.Height - 1, '1');

			TerminalExt.HLine(new Int2(0, _messageLog.Rect.Height), UI.Width);
			TerminalExt.HLine(new Int2(0, UI.Height - _statusBar.Rect.Height - 1), UI.Width);
			_viewport.Draw();
			_statusBar.Draw();
			_messageLog.Draw();
		}
	}
}
