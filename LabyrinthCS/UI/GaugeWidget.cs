using System.Drawing;

using Labyrinth.Utils;

namespace Labyrinth.UI
{
    public class GaugeWidget
    {
        private readonly Point _origin;
        private readonly int _width;
        private readonly Color _color;
        private readonly Color _lossColor;
        private readonly Color _gainColor;

        public GaugeWidget(Point origin, int width, Color color, Color? lossColor = null, Color? gainColor = null)
        {
            _origin = origin;
            _width = width;
            _color = color;
            _lossColor = lossColor ?? color;
            _gainColor = gainColor ?? color;
        }

        public void Draw(Gauge gauge)
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
