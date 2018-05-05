using System.Drawing;

namespace Labyrinth.UI
{
	public sealed class Glyph
	{
		private static readonly Color DefaultUnlitFore = Color.FromArgb(0x20, 0x20, 0x20);
		private static readonly Color DefaultUnlitBack = DefaultUnlitFore;

		private Color? _unlitFore;

		public char Code { get; }
		public Color Fore { get; set; }
		public Color? Back { get; set; }
		public bool AlwaysDrawBack { get; set; }

		public Color UnlitFore
		{
			get => _unlitFore ?? DefaultUnlitFore;
			set => _unlitFore = value;
		}

		public Color UnlitBack => Back.HasValue && (Back != Color.Black)
			? DefaultUnlitBack
			: Color.Black;

		public Glyph(char code)
		{
			Code = code;
			Fore = Color.Black;
		}
	}
}
