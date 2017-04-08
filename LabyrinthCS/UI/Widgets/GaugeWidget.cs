using System.Drawing;

using JetBrains.Annotations;

using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI.Widgets
{
    public sealed class GaugeWidget
    {
        private readonly Vector2I _origin;
        private readonly int _width;
        private readonly Color _color;
        private readonly Color _lossColor;
        private readonly Color _gainColor;

        public GaugeWidget(Vector2I origin, int width, Color color, Color? lossColor = null, Color? gainColor = null)
        {
            _origin = origin;
            _width = width;
            _color = color;
            _lossColor = lossColor ?? color;
            _gainColor = gainColor ?? color;
        }

        public void Draw([NotNull] Gauge gauge)
        {
            var previous = gauge.PreviousPercent;
            var current = gauge.CurrentPercent;

            var changeColor = previous > current ? _lossColor : _gainColor;
            var previousWidth = MathExt.RoundInt(previous * _width);
            var currentWidth = MathExt.RoundInt(current * _width);

            if (previousWidth < currentWidth)
            {
                MiscExt.Swap(ref previousWidth, ref currentWidth);
            }

            TerminalExt.HBar(_origin, _width, Color.DarkGray);
            TerminalExt.HBar(_origin, previousWidth, changeColor);
            TerminalExt.HBar(_origin, currentWidth, _color);
        }
    }
}
