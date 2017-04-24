using System;
using System.Drawing;

using BearLib;

using Labyrinth.UI.Input;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI
{
    // TODO multiline
    public sealed class ConfirmModal : Modal<bool>
    {
        private const string YesNo = "[[[color=green]Y[/color]]]es / [[[color=green]N[/color]]]o";

        private readonly string _message;
        private readonly Vector2I _origin;
        private readonly int _width;

        public ConfirmModal(string message)
        {
            _message = message;

            var yesNoSize = Terminal.Measure(YesNo);
            var messageSize = Terminal.Measure(_message);

            _width = Math.Max(yesNoSize.Width, messageSize.Width) + 3;
            var x = Const.Width / 2 - _width / 2;
            var y = Const.Height / 2 - 1;
            _origin = new Vector2I(x, y);
        }

        protected override void DrawModal()
        {
            var first = _origin + new Vector2I(1, 1);
            var second = _origin + new Vector2I(1, 2);
            var bbox = new Vector2I(_width - 2, 1);

            Terminal.Print(first, ' '.Repeat(_width));
            Terminal.Print(second, ' '.Repeat(_width));
            TerminalExt.Box(_origin, new Vector2I(_width, 2));
            Terminal.Print(new Rect(first, bbox), ContentAlignment.MiddleCenter, _message);
            Terminal.Print(new Rect(second, bbox), ContentAlignment.MiddleCenter, YesNo);
        }

        protected override void ReactModal(KeyEvent @event)
        {
            switch (@event.Key.Code)
            {
                case Terminal.TK_Y:
                    Close(true);
                    break;
                case Terminal.TK_N:
                case Terminal.TK_ESCAPE:
                    Close(false);
                    break;
            }
        }

        public static bool Show(string message)
        {
            var modal = new ConfirmModal(message);
            modal.Run();
            return modal.Result;
        }
    }
}
