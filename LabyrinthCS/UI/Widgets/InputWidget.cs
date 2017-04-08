using System.Diagnostics;
using System.Drawing;
using System.Text;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI.Widgets
{
    public sealed class InputWidget
    {
        private readonly Vector2I _point;
        private readonly int _maxLength;

        public string Value { get; private set; }

        public InputWidget(Vector2I point, int maxLength, string @default = "")
        {
            _point = point;
            _maxLength = maxLength;
            Value = @default;

            Debug.Assert(Value.Length < _maxLength);
        }

        public void Focus()
        {
            Clear();
            using (Colors())
            {
                var builder = new StringBuilder();
                var result = Terminal.ReadStr(_point, builder, _maxLength);
                var newValue = builder.ToString().Trim();

                if (result != Terminal.TK_INPUT_CANCELLED && newValue.Length > 0)
                {
                    Value = newValue;
                }
            }
        }

        public void Draw()
        {
            Clear();
            using (Colors())
            {
                Terminal.Print(_point, Value);
            }
        }

        private void Clear()
        {
            using (Colors())
            {
                Terminal.Print(_point, new string(' ', _maxLength));
            }
        }

        [NotNull]
        private Guard Colors()
        {
            return TerminalExt.Colors(Color.White, Color.DarkGray);
        }
    }
}
