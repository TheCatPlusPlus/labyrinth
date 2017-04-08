using BearLib;

using Labyrinth.UI.Input;
using Labyrinth.UI.Widgets;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI.Scenes
{
    public class NewGameScene : Scene
    {
        private readonly InputWidget _playerName;
        private readonly MenuWidget _menu;

        public NewGameScene()
        {
            _playerName = new InputWidget(new Vector2I(25, 1), Const.WidthPlayerName, "Bob");
            _menu = new MenuWidget
            {
                { "Start the game", OnStartGame },
                { "Change the name", _playerName.Focus },
                { "Go back", OnBack }
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
                    OnBack();
                    break;
            }
        }

        public override void Draw()
        {
            Terminal.Print(new Vector2I(4, 1), "Character name:");
            _playerName.Draw();
            _menu.Draw(new Vector2I(1, 10));
        }

        private void OnBack()
        {
            State.SetScene<MainMenuScene>();
        }

        private void OnStartGame()
        {
            State.NewGame(_playerName.Value);
            State.SetScene<GameScene>();
        }
    }
}
