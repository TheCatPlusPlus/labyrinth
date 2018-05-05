using System;

using JetBrains.Annotations;

namespace BearLib
{
	[Flags]
	[PublicAPI]
	public enum Alignment
	{
		Default = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8,

		Center = Left | Right,
		Middle = Top | Bottom,

		TopLeft = Top | Left,
		TopCenter = Top | Center,
		TopRight = Top | Right,
		MiddleLeft = Middle | Left,
		MiddleCenter = Middle | Center,
		MiddleRight = Middle | Right,
		BottomLeft = Bottom | Left,
		BottomCenter = Bottom | Center,
		BottomRight = Bottom | Right
	}
}
