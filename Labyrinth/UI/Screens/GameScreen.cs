using BearLib;

using Labyrinth.Geometry;

using NLog;

namespace Labyrinth.UI
{
	public sealed class GameScreen : Screen
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public GameScreen(Game game)
			: base(game)
		{
		}

		public override void React(Code code, UI ui)
		{
			if (Terminal.Check(Code.Shift))
			{
				ReactShift(code, ui);
			}
			else
			{
				ReactNonShift(code, ui);
			}
		}

		private void ReactShift(Code code, UI ui)
		{
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (code)
			{
				case Code.Period:
					Game.Move(Direction.Down);
					break;
				case Code.Comma:
					Game.Move(Direction.Up);
					break;
				case Code.Q:
					AskQuit(true, ui);
					break;
			}
		}

		private void ReactNonShift(Code code, UI ui)
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
				case Code.R:
					Game.Rest();
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
					AskQuit(false, ui);
					break;
			}
		}

		private void AskQuit(bool discard, UI ui)
		{
			Log.Debug($"AskQuit({discard})");

			var message = discard
				? "Quit without saving? [color=red]NOTE[/color]: this will discard existing save! (not implemented yet)"
				: "Save and quit? (not implemented yet)";
			var confirm = new ConfirmDialog(Game, message);

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

				ui.Quit();
			};

			ui.Open(confirm);
		}

		public override void Draw(UI ui)
		{
		}
	}
}
