using System.Drawing;

using BearLib;

namespace Labyrinth.UI
{
    public class SidebarGaugeWidget
    {
        private readonly GaugeWidget _gauge;
        private readonly string _label;
        private readonly Point _origin;

        public SidebarGaugeWidget(
            string label,
            Point origin,
            Color color,
            Color? lossColor = null,
            Color? gainColor = null)
        {
            _label = label;
            _origin = new Point(1, origin.Y);
            _gauge = new GaugeWidget(origin, Const.WidthSidebarGauge, color, lossColor, gainColor);
        }

        public void Draw(Gauge gauge)
        {
            string fg;

            if (gauge.CurrentPercent < 0.25f)
            {
                fg = "red";
            }
            else if (gauge.CurrentPercent < 0.5f)
            {
                fg = "yellow";
            }
            else
            {
                fg = "white";
            }

            var label = $"{_label}: [color={fg}]{gauge.Value}[/color] / {gauge.MaxValue}";
            Terminal.Print(_origin, label);
            _gauge.Draw(gauge);
        }
    }
}
