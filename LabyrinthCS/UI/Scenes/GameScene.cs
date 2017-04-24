using System.Drawing;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.AI;
using Labyrinth.Maps;
using Labyrinth.UI.Input;
using Labyrinth.UI.Widgets;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI.Scenes
{
    public sealed class GameScene : Scene
    {
        private const string ConfirmQuit =
            "Quit without saving? [color=red]NOTE[/color]: this will discard existing save! (not implemented yet)";
        private const string ConfirmSave =
            "Save and quit? (not implemented yet)";

        private readonly SidebarGaugeWidget _hp;
        private readonly SidebarGaugeWidget _mp;
        private readonly SidebarGaugeWidget _stamina;
        private readonly Viewport _viewport;
        private readonly LookAtWidget _lookAt;
        private readonly MessagesWidget _messages;

        private IPathFinder _cursorPath;
        private Tile _cursorTile;

        public GameScene()
        {
            const int gaugeX = Const.WidthSidebar - Const.WidthSidebarGauge - 1;

            const int messagesX = Const.WidthSidebar + 2;
            const int messagesY = Const.Height - Const.HeightMessages + 1;
            var messagesRect = new Rect(messagesX, messagesY, Const.WidthViewport, Const.HeightMessages);

            var viewportRect = new Rect(Const.WidthSidebar + 2, 2, Const.WidthViewport, Const.HeightViewport);

            _hp = new SidebarGaugeWidget(
                "HP",
                new Vector2I(gaugeX, 3),
                Color.DarkGreen,
                Color.Red,
                Color.LawnGreen);

            _mp = new SidebarGaugeWidget(
                "MP",
                new Vector2I(gaugeX, 4),
                Color.Blue,
                Color.LightSkyBlue,
                Color.LightSkyBlue);

            _stamina = new SidebarGaugeWidget(
                "ST",
                new Vector2I(gaugeX, 5),
                Color.DarkOrange,
                Color.Orange,
                Color.Orange);

            _viewport = new Viewport(viewportRect);
            _lookAt = new LookAtWidget(messagesRect);
            _messages = new MessagesWidget(messagesRect);
        }

        public override void OnEnter()
        {
            State.Game.MessageListeners.Add(_messages);
            State.Game.Start();
        }

        public override void OnExit()
        {
            State.Game.MessageListeners.Remove(_messages);
        }

        public override void React(Event @event)
        {
            if (@event is KeyEvent key)
            {
                ReactKey(key.Key);
            }
            else if (@event is MouseMoveEvent mouse)
            {
                _viewport.Cursor = mouse.Cursor;

                try
                {
                    var level = State.Game.Level;
                    var map = _viewport.ScreenToMap(_viewport.Cursor);
                    _cursorTile = level[map];
                    _cursorPath = new MissilePath(level, State.Game.Player.Position, map);
                }
                catch (OutOfBounds)
                {
                    _cursorPath = null;
                    _cursorTile = null;
                }
            }
        }

        private void ReactKey([NotNull] Key key)
        {
            var action = KeyMap.Game[key];
            if ((action == UserAction.Quit) && ConfirmModal.Show(ConfirmQuit))
            {
                State.DiscardGame();
                State.SignalExit();
            }
            else if ((action == UserAction.SaveQuit) && ConfirmModal.Show(ConfirmSave))
            {
                State.SaveGame();
            }
            else if (action == UserAction.MoveTo)
            {
                // TODO
            }
            else if (action != null)
            {
                _cursorTile = null; // TODO modal look
                _cursorPath = null;
                State.Game.React(action.Value);
            }
        }

        public override void Draw()
        {
            TerminalExt.VLine(new Vector2I(Const.WidthSidebar, 0), Const.Height);
            TerminalExt.HLine(
                new Vector2I(Const.WidthSidebar, Const.Height - Const.HeightMessages),
                Const.Width - Const.WidthSidebar);
            Terminal.Put(Const.WidthSidebar, Const.Height - Const.HeightMessages, TerminalExt.BoxVLineLSplit);

            using (TerminalExt.Foreground(Color.Gray))
            {
                DrawSidebar();
                DrawMessages();
                DrawViewport();
            }
        }

        private void DrawMessages()
        {
            if (_cursorTile != null)
            {
                _lookAt.Draw(_cursorTile);
            }
            else
            {
                _messages.Draw();
            }
        }

        private void DrawSidebar()
        {
            var game = State.Game;
            var player = game.Player;

            Terminal.Print(
                new Rect(2, 1, Const.WidthPlayerName, 1),
                ContentAlignment.MiddleCenter,
                player.Name.Singular(true));

            _hp.Draw(player.HP);
            _mp.Draw(player.MP);
            _stamina.Draw(player.Stamina);

            var turn = game.TotalCost / (float)Const.SpeedBase;
            var last = MathExt.Clamp(game.LastCost / (float)Const.SpeedBase, 0.0f, float.MaxValue);

            Terminal.Print(2, 6, $"T: {turn:F1} ({last:F1})");
        }

        private void DrawViewport()
        {
            _viewport.Draw();

            if (_cursorPath?.Found != true)
            {
                return;
            }

            using (TerminalExt.Layer(3))
            using (TerminalExt.Foreground(Color.Red))
            {
                foreach (var map in _cursorPath.Points)
                {
                    var screen = _viewport.MapToScreen(map);
                    Terminal.Put(screen, '\u00B7');
                }
            }
        }
    }
}
