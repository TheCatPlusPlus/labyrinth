using JetBrains.Annotations;

namespace BearLib
{
	[PublicAPI]
	public enum Code
	{
		None = 0,

		A = 0x04,
		B = 0x05,
		C = 0x06,
		D = 0x07,
		E = 0x08,
		F = 0x09,
		G = 0x0A,
		H = 0x0B,
		I = 0x0C,
		J = 0x0D,
		K = 0x0E,
		L = 0x0F,
		M = 0x10,
		N = 0x11,
		O = 0x12,
		P = 0x13,
		Q = 0x14,
		R = 0x15,
		S = 0x16,
		T = 0x17,
		U = 0x18,
		V = 0x19,
		W = 0x1A,
		X = 0x1B,
		Y = 0x1C,
		Z = 0x1D,
		Alpha1 = 0x1E,
		Alpha2 = 0x1F,
		Alpha3 = 0x20,
		Alpha4 = 0x21,
		Alpha5 = 0x22,
		Alpha6 = 0x23,
		Alpha7 = 0x24,
		Alpha8 = 0x25,
		Alpha9 = 0x26,
		Alpha0 = 0x27,
		Return = 0x28,
		Enter = 0x28,
		Escape = 0x29,
		Backspace = 0x2A,
		Tab = 0x2B,
		Space = 0x2C,
		Minus = 0x2D,
		Equals = 0x2E,
		LBracket = 0x2F,
		RBracket = 0x30,
		Backslash = 0x31,
		Semicolon = 0x33,
		Apostrophe = 0x34,
		Grave = 0x35,
		Comma = 0x36,
		Period = 0x37,
		Slash = 0x38,
		F1 = 0x3A,
		F2 = 0x3B,
		F3 = 0x3C,
		F4 = 0x3D,
		F5 = 0x3E,
		F6 = 0x3F,
		F7 = 0x40,
		F8 = 0x41,
		F9 = 0x42,
		F10 = 0x43,
		F11 = 0x44,
		F12 = 0x45,
		Pause = 0x48,
		Insert = 0x49,
		Home = 0x4A,
		PageUp = 0x4B,
		Delete = 0x4C,
		End = 0x4D,
		PageDown = 0x4E,
		Right = 0x4F,
		Left = 0x50,
		Down = 0x51,
		Up = 0x52,
		NumDivide = 0x54,
		NumMultiply = 0x55,
		NumMinus = 0x56,
		NumPlus = 0x57,
		NumEnter = 0x58,
		Num1 = 0x59,
		Num2 = 0x5A,
		Num3 = 0x5B,
		Num4 = 0x5C,
		Num5 = 0x5D,
		Num6 = 0x5E,
		Num7 = 0x5F,
		Num8 = 0x60,
		Num9 = 0x61,
		Num0 = 0x62,
		NumPeriod = 0x63,
		Shift = 0x70,
		Control = 0x71,
		Alt = 0x72,

		// Mouse events/states
		MouseLeft = 0x80, // Buttons
		MouseRight = 0x81,
		MouseMiddle = 0x82,
		MouseX1 = 0x83,
		MouseX2 = 0x84,
		MouseMove = 0x85, // Movement event
		MouseScroll = 0x86, // Mouse scroll event
		MouseX = 0x87, // Cusor position in cells
		MouseY = 0x88,
		MousePixelX = 0x89, // Cursor position in pixels
		MousePixelY = 0x8A,
		MouseWheel = 0x8B, // Scroll direction and amount
		MouseClicks = 0x8C, // Number of consecutive clicks

		//If key was released instead of pressed, it's code will be OR'ed withKEY_RELEASED
		KeyReleased = 0x100,

		// Virtual key-codes for internal terminal states/variables.
		// These can be accessed via terminal_state function.
		Width = 0xC0, // Terminal width in cells
		Height = 0xC1, // Terminal height in cells
		CellWidth = 0xC2, // Cell width in pixels
		CellHeight = 0xC3, // Cell height in pixels
		Color = 0xC4, // Current foregroung color
		BkColor = 0xC5, // Current background color
		Layer = 0xC6, // Current layer
		Composition = 0xC7, // Current composition state
		Char = 0xC8, // Translated ANSI code of last produced character
		WChar = 0xC9, // Unicode codepoint of last produced character
		Event = 0xCA, // Last dequeued event
		FullScreen = 0xCB, // Fullscreen state

		// Other events
		Close = 0xE0,
		Resized = 0xE1,

		// Input result codes for terminal_read function.
		InputNone = 0,
		InputCancelled = -1
	}
}
