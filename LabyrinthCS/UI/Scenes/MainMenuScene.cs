using System.Drawing;

using BearLib;

using Labyrinth.UI.Input;

namespace Labyrinth.UI.Scenes
{
    public class MainMenuScene : Scene
    {
        private readonly MenuWidget _menu;

        public MainMenuScene()
        {
            _menu = new MenuWidget
            {
                { "New game", OnNewGame },
                { "Load game", OnLoadGame, SavedGame.Exists },
                { "Quit", State.SignalExit }
            };
        }

        public override void React(Event @event)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_menu.React(@event))
            {
                case MenuWidget.Result.Confirm:
                    _menu.Current.Action();
                    break;
                case MenuWidget.Result.Cancel:
                    State.SignalExit();
                    break;
            }
        }

        public override void Draw()
        {
            Terminal.Print(
                new Rectangle(0, 12, Const.Width, 1),
                ContentAlignment.MiddleCenter,
                "The Labyrinth");
            Terminal.Print(
                new Rectangle(0, 13, Const.Width, 1),
                ContentAlignment.MiddleCenter,
                "[color=dark grey]v0.1.0");

            var x = Const.Width / 2 - _menu.Width / 2;
            _menu.Draw(new Point(x, 17));
        }

        private void OnLoadGame()
        {
            // TODO
        }

        private void OnNewGame()
        {
            State.SetScene<NewGameScene>();
        }
    }
}
