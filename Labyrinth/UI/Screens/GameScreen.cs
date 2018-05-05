using System.Diagnostics;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Geometry;
using Labyrinth.Map;
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
		private readonly LookAt _lookAt;

		[CanBeNull]
		private Tile _cursorTile;

		public GameScreen(Game game, UI ui)
			: base(game, ui)
		{
			var messageLog = new Rect(0, 0, ui.Width, 4);
			var statusBar = new Rect(0, ui.Height - 3, ui.Width, 3);
			var viewport = new Rect(0, messageLog.Height + 1, ui.Width, ui.Height - messageLog.Height - statusBar.Height - 2);

			_viewport = new Viewport(game, viewport);
			_statusBar = new StatusBar(game, statusBar);
			_lookAt = new LookAt(statusBar);
			_messageLog = new MessageLog(game, messageLog);
		}

		public override void React(Code code)
		{
			if (code == Code.MouseMove)
			{
				ReactMouse();
			}
			else if (Terminal.Check(Code.Shift))
			{
				ReactShift(code);
			}
			else
			{
				ReactNonShift(code);
			}
		}

		private void ReactMouse()
		{
			Debug.Assert(Game.Player.Level != null, "Game.Player.Level != null");

			var x = Terminal.State(Code.MouseX);
			var y = Terminal.State(Code.MouseY);

			_viewport.Cursor = new Int2(x, y);
			_cursorTile = null;

			if (_viewport.Cursor != null)
			{
				var map = _viewport.ScreenToMap(_viewport.Cursor.Value);
				if (map != null)
				{
					_cursorTile = Game.Player.Level.Grid[map.Value];
				}
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
			TerminalExt.HLine(new Int2(0, _messageLog.Rect.Height), UI.Width);
			TerminalExt.HLine(new Int2(0, UI.Height - _statusBar.Rect.Height - 1), UI.Width);
			_viewport.Draw();
			_messageLog.Draw();

			if (_cursorTile != null)
			{
				_lookAt.Draw(_cursorTile);
			}
			else
			{
				_statusBar.Draw();
			}
		}
	}
}
