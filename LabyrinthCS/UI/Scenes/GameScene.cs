using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.AI;
using Labyrinth.Data;
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
        private IPathFinder _cursorPath;
        private Tile _lookAt;

        public GameScene()
        {
            const int gaugeX = Const.WidthSidebar - Const.WidthSidebarGauge - 1;

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

            _viewport = new Viewport(
                new Rect(Const.WidthSidebar + 2, 2, Const.WidthViewport, Const.HeightViewport));
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
                    _lookAt = level[map];
                    _cursorPath = new MissilePath(level, State.Game.Player.Position, map);
                }
                catch (OutOfBounds)
                {
                    _cursorPath = null;
                    _lookAt = null;
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
                _lookAt = null; // TODO modal look
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
            var x = Const.WidthSidebar + 2;
            var y = Const.Height - Const.HeightMessages + 1;
            var rect = new Rect(x, y, Const.WidthViewport, Const.HeightMessages);

            if (_lookAt != null)
            {
                DrawLookAt(rect);
            }
        }

        private void DrawLookAt(Rect rect)
        {
            var look = new StringBuilder();

            if (!_lookAt.WasSeen)
            {
                look.Append("[color=white]An unseen tile.");
            }
            else
            {
                look.Append($"[color=white]{_lookAt.Name.Singular().Capitalize()}.");
                var description = _lookAt.Description;

                if (_lookAt.IsLit)
                {
                    if (_lookAt.Monster != null)
                    {
                        look.Append($" [color=cyan]{_lookAt.Monster.Name.Singular().Capitalize()}[/color] is here.");
                    }

                    if (_lookAt.Items.Count > 0)
                    {
                        // TODO store count on Tile maybe
                        var query = from @group in _lookAt.Items.GroupBy(i => i.Id)
                            let item = ItemData.For(@group.Key)
                            let count = @group.Count()
                            select (item, count);
                        var grouped = query.ToList();

                        if (grouped.Count.Within(1, 5))
                        {
                            var items = new List<string>();

                            for (var idx = 0; idx < grouped.Count; ++idx)
                            {
                                (var item, var count) = grouped[idx];
                                var name = count > 1 ? item.Name.Plural(count) : item.Name.Singular();

                                if (idx == 0)
                                {
                                    name = name.Capitalize();
                                }

                                name = $"[color=cyan]{name}[/color]";

                                if ((grouped.Count > 1) && (idx == (grouped.Count - 1)))
                                {
                                    name = $"and {name}";
                                }

                                items.Add(name);
                            }

                            var lies = _lookAt.Items.Count > 1 ? "lie" : "lies";
                            var joined = string.Join(", ", items);
                            look.Append($" {joined} {lies} here.");
                        }
                        else
                        {
                            look.Append(" Many items lie here.");
                        }
                    }

                    if (_lookAt.Monster != null)
                    {
                        description = _lookAt.Monster.Description;
                    }
                    else if (_lookAt.Items.Count == 1)
                    {
                        description = _lookAt.Items[0].Description;
                    }
                }

                look.Append($"\n\n[color=light grey]{description}[/color]");
            }

            Terminal.Print(rect, look.ToString());
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
        }

        private void DrawViewport()
        {
            _viewport.Draw();

            if ((_cursorPath != null) && _cursorPath.Found)
            {
                using (TerminalExt.Composition())
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
}
