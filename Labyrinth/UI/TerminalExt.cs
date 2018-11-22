using System;
using System.Diagnostics;
using System.Drawing;

using BearLib;

using Labyrinth.Geometry;
using Labyrinth.Utils;

namespace Labyrinth.UI
{
	public static class TerminalExt
	{
		public const char BoxTopLeft = '\u250C';
		public const char BoxTopRight = '\u2510';
		public const char BoxBottomLeft = '\u2514';
		public const char BoxBottomRight = '\u2518';
		public const char BoxTopLeftSoft = '\u256C';
		public const char BoxTopRightSoft = '\u256D';
		public const char BoxBottomLeftSoft = '\u2570';
		public const char BoxBottomRightSoft = '\u256F';
		public const char BoxVLine = '\u2502';
		public const char BoxHLine = '\u2500';
		public const char BoxVLineLSplit = '\u251C';
		public const char BoxVLineRSplit = '\u2524';
		public const char BoxHLineDSplit = '\u252C';
		public const char BoxHLineUSplit = '\u2534';
		public const char BoxCrossSplit = '\u253C';

		public static void Setup(string title, int width, int height)
		{
			var font = System.IO.Path.Combine(Data.AssetsPath, "Fonts", "Inconsolata-Regular.ttf");
			var icon = System.IO.Path.Combine(Data.AssetsPath, "Labyrinth.ico");

			Terminal.Open();
			Terminal.Set(
				$"window: title={title}, size={width}x{height}, icon={icon};" +
				"input: precise-mouse=false, filter=[keyboard, mouse, system], alt-functions=false;" +
				"log: level=debug;" +
				$"font: {font}, size=16, use-box-drawing=false, use-block-elements=false;"
			);
			Terminal.Color(Color.White);
			Terminal.BkColor(Color.Black);
			Terminal.Refresh();
		}

		private static Guard ColorGuard(Color? color, Code state, Action<Color> set)
		{
			var previous = Color.FromArgb(Terminal.State(state));
			if (color.HasValue)
			{
				set(color.Value);
			}

			return new Guard(() => set(previous));
		}

		public static Guard Background(Color? color)
		{
			return ColorGuard(color, Code.BkColor, Terminal.BkColor);
		}

		public static Guard Foreground(Color? color)
		{
			return ColorGuard(color, Code.Color, Terminal.Color);
		}

		public static Guard Colors(Color? fg, Color? bg)
		{
			var fgGuard = Foreground(fg);
			var bgGuard = Background(bg);
			return new Guard(
				() =>
				{
					bgGuard.Dispose();
					fgGuard.Dispose();
				});
		}

		public static void Box(Int2 origin, Int2 size)
		{
			Box(new Rect(origin, size));
		}

		public static void Box(Rect rect)
		{
			VLine(rect.TopLeft, rect.Height);
			VLine(rect.TopRight, rect.Height);
			HLine(rect.TopLeft, rect.Width);
			HLine(rect.BottomLeft, rect.Width);

			Terminal.Put(rect.TopLeft, BoxTopLeft);
			Terminal.Put(rect.TopRight, BoxTopRight);
			Terminal.Put(rect.BottomLeft, BoxBottomLeft);
			Terminal.Put(rect.BottomRight, BoxBottomRight);
		}

		public static void VLine(Int2 point, int height, char ch = BoxVLine)
		{
			for (var y = 0; y < height; ++y)
			{
				Terminal.Put(point.X, point.Y + y, ch);
			}
		}

		public static void HLine(Int2 point, int width, char ch = BoxHLine)
		{
			for (var x = 0; x < width; ++x)
			{
				Terminal.Put(point.X + x, point.Y, ch);
			}
		}

		public static void HBar(Int2 point, int width, Color? color)
		{
			using (Background(color))
			{
				HLine(point, width, ' ');
			}
		}

		public static Guard Composition(bool enabled = true)
		{
			var current = Terminal.State(Code.Composition) != 0;
			Terminal.Composition(enabled);
			return new Guard(() => Terminal.Composition(current));
		}

		public static Guard Layer(int layer)
		{
			Debug.Assert(layer.Within(0, 256));
			var current = Terminal.State(Code.Layer);
			Terminal.Layer(layer);
			return new Guard(() => Terminal.Layer(current));
		}

		public static Guard Crop(Rect rect)
		{
			Terminal.Crop(rect);
			return new Guard(() => Terminal.Crop(0, 0, 0, 0));
		}

		public static void Fill(Rect rect, char ch)
		{
			foreach (var point in rect.Points)
			{
				Terminal.Put(point, ch);
			}
		}

		public static string EscapeTags(this string s)
		{
			return s.Replace("[", "[[").Replace("]", "]]");
		}
	}
}
