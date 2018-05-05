using System;

using BearLib;

using Labyrinth.Geometry;

namespace Labyrinth.UI
{
	public abstract class BoxDialog : Dialog
	{
		protected abstract string Message { get; }
		protected virtual string Footer { get; } = "";

		protected BoxDialog(Game game)
			: base(game)
		{
		}

		public override void Draw(UI ui)
		{
			var footerSize = Terminal.Measure(Footer);
			var messageSize = Terminal.Measure(Message);

			var width = Math.Max(footerSize.Width, messageSize.Width) + 3;
			var x = ui.Width / 2 - width / 2;
			var y = ui.Height / 2 - 1;
			var origin = new Int2(x, y);

			var first = origin + new Int2(1, 1);
			var second = origin + new Int2(1, 2);
			var bbox = new Int2(width - 2, 1);

			TerminalExt.Box(origin, new Int2(width, 2));
			Terminal.Print(new Rect(first, bbox), Alignment.MiddleCenter, Message);
			Terminal.Print(new Rect(second, bbox), Alignment.MiddleCenter, Footer);
		}
	}
}
