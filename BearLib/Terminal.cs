/*
	Based on the orginal wrapper available under the following license:

	BearLibTerminal C# wrapper
	Copyright (C) 2013-2017 Cfyz

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
	of the Software, and to permit persons to whom the Software is furnished to do
	so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
	FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
	COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
	IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
	CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using BearLib.Native;

using JetBrains.Annotations;

namespace BearLib
{
	[PublicAPI]
	public static class Terminal
	{
		private const string LibraryName = "BearLibTerminal";

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool FnOpen();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnClose();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnRefresh();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool FnSet([NotNull] [In] string options);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnClear();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnClearArea(int x, int y, int w, int h);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnCrop(int x, int y, int w, int h);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnColor(int argb);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnBkColor(int argb);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnComposition([MarshalAs(UnmanagedType.Bool)] bool mode);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnLayer(int layer);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private delegate void FnFont([NotNull] [In] string name);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnPut(int x, int y, int code);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnPutExt(
			int x, int y, int dx, int dy, int code,
			[CanBeNull] [MarshalAs(UnmanagedType.LPArray)] [In]
			int[] corners);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int FnPick(int x, int y, int index);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int FnPickColor(int x, int y, int index);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int FnPickBkColor(int x, int y);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private delegate void FnPrintExt(
			int x, int y, int w, int h, Alignment align,
			[NotNull] [In] string text, [Out] out int outW, [Out] out int outH);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private delegate void FnMeasureExt(int w, int h, [NotNull] [In] string text, [Out] out int outW, [Out] out int outH);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool FnHasInput();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int FnState(Code code);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate Code FnRead();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private delegate int FnReadStr(int x, int y, [NotNull] [In] [Out] StringBuilder text, int max);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate Code FnPeek();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void FnDelay(int time);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private delegate IntPtr FnGet([NotNull] [In] string key, [NotNull] [In] string @default);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private delegate int FnColorFromName([NotNull] [In] string name);

		private static readonly Library Lib;
		private static readonly FnOpen DoOpen;
		private static readonly FnClose DoClose;
		private static readonly FnRefresh DoRefresh;
		private static readonly FnSet DoSet;
		private static readonly FnClear DoClear;
		private static readonly FnClearArea DoClearArea;
		private static readonly FnCrop DoCrop;
		private static readonly FnColor DoColor;
		private static readonly FnColor DoBkColor;
		private static readonly FnComposition DoComposition;
		private static readonly FnLayer DoLayer;
		private static readonly FnFont DoFont;
		private static readonly FnPut DoPut;
		private static readonly FnPutExt DoPutExt;
		private static readonly FnPick DoPick;
		private static readonly FnPickColor DoPickColor;
		private static readonly FnPickBkColor DoPickBkColor;
		private static readonly FnPrintExt DoPrintExt;
		private static readonly FnMeasureExt DoMeasureExt;
		private static readonly FnHasInput DoHasInput;
		private static readonly FnState DoState;
		private static readonly FnRead DoRead;
		private static readonly FnReadStr DoReadStr;
		private static readonly FnPeek DoPeek;
		private static readonly FnDelay DoDelay;
		private static readonly FnGet DoGet;
		private static readonly FnColorFromName DoColorFromName;

		static Terminal()
		{
			Lib = new Library();
			Lib.Open(LibraryName);

			Lib.Get(out DoOpen, "terminal_open");
			Lib.Get(out DoClose, "terminal_close");
			Lib.Get(out DoRefresh, "terminal_refresh");
			Lib.Get(out DoSet, "terminal_set16");
			Lib.Get(out DoClear, "terminal_clear");
			Lib.Get(out DoClearArea, "terminal_clear_area");
			Lib.Get(out DoCrop, "terminal_crop");
			Lib.Get(out DoColor, "terminal_color");
			Lib.Get(out DoBkColor, "terminal_bkcolor");
			Lib.Get(out DoComposition, "terminal_composition");
			Lib.Get(out DoLayer, "terminal_layer");
			Lib.Get(out DoFont, "terminal_font16");
			Lib.Get(out DoPut, "terminal_put");
			Lib.Get(out DoPutExt, "terminal_put_ext");
			Lib.Get(out DoPick, "terminal_pick");
			Lib.Get(out DoPickColor, "terminal_pick_color");
			Lib.Get(out DoPickBkColor, "terminal_pick_bkcolor");
			Lib.Get(out DoPrintExt, "terminal_print_ext16");
			Lib.Get(out DoMeasureExt, "terminal_measure_ext16");
			Lib.Get(out DoHasInput, "terminal_has_input");
			Lib.Get(out DoState, "terminal_state");
			Lib.Get(out DoRead, "terminal_read");
			Lib.Get(out DoReadStr, "terminal_read_str16");
			Lib.Get(out DoPeek, "terminal_peek");
			Lib.Get(out DoDelay, "terminal_delay");
			Lib.Get(out DoGet, "terminal_get16");
			Lib.Get(out DoColorFromName, "color_from_name16");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Open()
		{
			Lib.Check();
			return DoOpen();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Close()
		{
			Lib.Check();
			DoClose();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Refresh()
		{
			Lib.Check();
			DoRefresh();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Set(string options)
		{
			Lib.Check();
			return DoSet(options);
		}

		[StringFormatMethod("options")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Set(string options, params object[] args)
		{
			for (var i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case Size size:
						args[i] = $"{size.Width}x{size.Height}";
						break;
					case bool value:
						args[i] = value ? "true" : "false";
						break;
				}
			}

			return Set(string.Format(options, args));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clear()
		{
			Lib.Check();
			DoClear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearArea(int x, int y, int w, int h)
		{
			Lib.Check();
			DoClearArea(x, y, w, h);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearArea(Rectangle area)
		{
			ClearArea(area.X, area.Y, area.Width, area.Height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearArea(Point point, Size size)
		{
			ClearArea(point.X, point.Y, size.Width, size.Height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Crop(int x, int y, int w, int h)
		{
			Lib.Check();
			DoCrop(x, y, w, h);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Crop(Rectangle area)
		{
			Crop(area.X, area.Y, area.Width, area.Height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Crop(Point point, Size size)
		{
			Crop(point.X, point.Y, size.Width, size.Height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Color(Color color)
		{
			Lib.Check();
			DoColor(color.ToArgb());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Color(string name)
		{
			Lib.Check();
			DoColor(DoColorFromName(name));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BkColor(Color color)
		{
			Lib.Check();
			DoBkColor(color.ToArgb());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BkColor(string name)
		{
			Lib.Check();
			DoBkColor(DoColorFromName(name));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Composition(bool enabled)
		{
			Lib.Check();
			DoComposition(enabled);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Layer(int index)
		{
			Lib.Check();
			DoLayer(index);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Font(string name)
		{
			Lib.Check();
			DoFont(name);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Put(int x, int y, int code)
		{
			Lib.Check();
			DoPut(x, y, code);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Put(Point location, int code)
		{
			Put(location.X, location.Y, code);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Put(int x, int y, char code)
		{
			Put(x, y, (int)code);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Put(Point location, char code)
		{
			Put(location.X, location.Y, (int)code);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(int x, int y, int dx, int dy, int code)
		{
			Lib.Check();
			DoPutExt(x, y, dx, dy, code, null);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(Point location, Point offset, int code)
		{
			Lib.Check();
			DoPutExt(location.X, location.Y, offset.X, offset.Y, code, null);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(int x, int y, int dx, int dy, char code)
		{
			Lib.Check();
			DoPutExt(x, y, dx, dy, code, null);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(Point location, Point offset, char code)
		{
			Lib.Check();
			DoPutExt(location.X, location.Y, offset.X, offset.Y, code, null);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(
			int x, int y, int dx, int dy, int code, [NotNull] Color[] corners, [CanBeNull] ref int[] buffer)
		{
			if (buffer == null)
			{
				buffer = new int[4];
			}

			for (var i = 0; i < 4; i++)
			{
				buffer[i] = corners[i].ToArgb();
			}

			Lib.Check();
			DoPutExt(x, y, dx, dy, code, buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(int x, int y, int dx, int dy, int code, [NotNull] Color[] corners)
		{
			int[] buffer = null;
			PutExt(x, y, dx, dy, code, corners, ref buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(
			Point location, Point offset, int code, [NotNull] Color[] corners, [CanBeNull] ref int[] buffer)
		{
			PutExt(location.X, location.Y, offset.X, offset.Y, code, corners, ref buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(
			int x, int y, int dx, int dy, char code, [NotNull] Color[] corners, [CanBeNull] ref int[] buffer)
		{
			PutExt(x, y, dx, dy, (int)code, corners, ref buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(
			Point location, Point offset, char code, [NotNull] Color[] corners, [CanBeNull] ref int[] buffer)
		{
			PutExt(location.X, location.Y, offset.X, offset.Y, (int)code, corners, ref buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(Point location, Point offset, int code, [NotNull] Color[] corners)
		{
			PutExt(location.X, location.Y, offset.X, offset.Y, code, corners);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(int x, int y, int dx, int dy, char code, [NotNull] Color[] corners)
		{
			PutExt(x, y, dx, dy, (int)code, corners);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutExt(Point location, Point offset, char code, [NotNull] Color[] corners)
		{
			PutExt(location.X, location.Y, offset.X, offset.Y, (int)code, corners);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Pick(int x, int y, int index = 0)
		{
			Lib.Check();
			return DoPick(x, y, index);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Pick(Point location, int index = 0)
		{
			return Pick(location.X, location.Y, index);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color PickColor(int x, int y, int index = 0)
		{
			Lib.Check();
			return System.Drawing.Color.FromArgb(DoPickColor(x, y, index));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color PickColor(Point location, int index = 0)
		{
			return PickColor(location.X, location.Y, index);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color PickBkColor(int x, int y)
		{
			Lib.Check();
			return System.Drawing.Color.FromArgb(DoPickBkColor(x, y));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color PickBkColor(Point location)
		{
			return PickBkColor(location.X, location.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Print(int x, int y, int w, int h, Alignment alignment, string text)
		{
			Lib.Check();
			DoPrintExt(x, y, w, h, alignment, text, out var outW, out var outH);
			return new Size(outW, outH);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Print(int x, int y, Alignment alignment, string text)
		{
			return Print(x, y, 0, 0, alignment, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Print(int x, int y, string text)
		{
			return Print(x, y, 0, 0, Alignment.Default, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Print(Rectangle bbox, Alignment alignment, string text)
		{
			return Print(bbox.X, bbox.Y, bbox.Width, bbox.Height, alignment, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Print(Rectangle bbox, string text)
		{
			return Print(bbox, Alignment.Default, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Print(Point location, Alignment alignment, string text)
		{
			return Print(location.X, location.Y, alignment, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Print(Point location, string text)
		{
			return Print(location.X, location.Y, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Measure(Size bbox, string text)
		{
			Lib.Check();
			DoMeasureExt(bbox.Width, bbox.Height, text, out var width, out var height);
			return new Size(width, height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size Measure(string text)
		{
			return Measure(new Size(), text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasInput()
		{
			Lib.Check();
			return DoHasInput();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int State(Code code)
		{
			Lib.Check();
			return DoState(code);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Check(Code code)
		{
			return State(code) > 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Code Read()
		{
			Lib.Check();
			return DoRead();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(int x, int y, [NotNull] StringBuilder text, int max)
		{
			Lib.Check();
			return DoReadStr(x, y, text, max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(int x, int y, [NotNull] StringBuilder text)
		{
			return ReadStr(x, y, text, text.Capacity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(Point location, [NotNull] StringBuilder text, int max)
		{
			return ReadStr(location.X, location.Y, text, max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(Point location, [NotNull] StringBuilder text)
		{
			return ReadStr(location.X, location.Y, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(int x, int y, [NotNull] ref string text, int max)
		{
			var buffer = new StringBuilder(text, max);
			var result = ReadStr(x, y, buffer, max);
			text = buffer.ToString();
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(int x, int y, [NotNull] ref string text)
		{
			return ReadStr(x, y, ref text, text.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(Point location, [NotNull] ref string text, int max)
		{
			return ReadStr(location.X, location.Y, ref text, max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadStr(Point location, [NotNull] ref string text)
		{
			return ReadStr(location.X, location.Y, ref text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Code Peek()
		{
			Lib.Check();
			return DoPeek();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Delay(int time)
		{
			Lib.Check();
			DoDelay(time);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Delay(TimeSpan time)
		{
			Delay((int)time.TotalMilliseconds);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Get(string name, string @default = "")
		{
			Lib.Check();
			return Marshal.PtrToStringUni(DoGet(name, @default)) ?? "";
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static object ParseSize(string s)
		{
			var parts = s.Split('x');
			return new Size(int.Parse(parts[0]), int.Parse(parts[1]));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static object ParseBool(string s)
		{
			return s == "true";
		}

		[CanBeNull]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get<T>(string name, [CanBeNull] T @default = default)
		{
			var result = Get(name);

			if (result.Length == 0)
			{
				return @default;
			}

			try
			{
				var type = typeof(T);

				if (type == typeof(Size))
				{
					return (T)ParseSize(result);
				}

				if (type == typeof(bool))
				{
					return (T)ParseBool(result);
				}

				if (type.IsPrimitive)
				{
					return (T)Convert.ChangeType(result, typeof(T));
				}

				if (type.IsEnum)
				{
					return (T)Enum.Parse(type, result);
				}

				return (T)Activator.CreateInstance(type, result);
			}
			catch
			{
				return @default;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color ColorFromName(string name)
		{
			Lib.Check();
			return System.Drawing.Color.FromArgb(DoColorFromName(name));
		}
	}
}
