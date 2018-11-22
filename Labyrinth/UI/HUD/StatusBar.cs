using System.Drawing;

using BearLib;

using Labyrinth.Entities;
using Labyrinth.Entities.Attrs;
using Labyrinth.Geometry;
using Labyrinth.Utils;

namespace Labyrinth.UI.HUD
{
	public sealed class StatusBar
	{
		private readonly GamePrev _game;

		public Rect Rect { get; }

		public StatusBar(GamePrev game, Rect rect)
		{
			_game = game;
			Rect = rect;
		}

		public void Draw()
		{
			var x = 0;
			var y = 0;
			var player = _game.Player;

			void NextLine()
			{
				++y;
				x = 0;
			}

			void Space(int s)
			{
				x += s;
			}

			void DrawGauge(string l, Gauge g, int w = 15)
			{
				this.DrawGauge(l, g, x, y, w);
				Space(w);
			}

			void DrawStr(string l, string v, int w = 15)
			{
				DrawValue(l, v, x, y, w);
				Space(w);
			}

			var time = _game.TotalCost / (float)Scheduler.BaseSpeed;
			var timeDiff = _game.LastCost / (float)Scheduler.BaseSpeed;

			DrawGauge("HP", player.HP);
			DrawStr("T", $"{time:F1} ({timeDiff:F1})", 20);
			NextLine();
		}

		private void DrawGauge(string label, Gauge gauge, int x, int y, int width)
		{
			var value = $"{gauge.Value}/{gauge.MaxValue}";
			if (gauge.HasChanged)
			{
				var diff = gauge.Value - gauge.PreviousValue;
				var color = diff < 0 ? Color.DarkRed : Color.Chartreuse;
				value = $"{value} ([color={color.ToHex()}]{diff}[/color])";
			}

			DrawValue(label, value, x, y, width);
		}

		private void DrawValue(string label, string value, int x, int y, int width)
		{
			var bbox = MakeRect(x, y, width);
			var labelWidth = Terminal.Print(bbox, $"{label}: ").Width;
			bbox = MakeRect(x + labelWidth, y, width - labelWidth);
			Terminal.Print(bbox, value);
		}

		private Rect MakeRect(int x, int y, int width, int height = 1)
		{
			return new Rect(Rect.X + x, Rect.Y + y, width, height);
		}
	}
}
