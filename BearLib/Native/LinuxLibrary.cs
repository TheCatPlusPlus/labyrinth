using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace BearLib.Native
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal sealed class LinuxLibrary : LibraryImpl
	{
		private const string LibDL = "kernel32.dll";
		private const int RTLD_NOW = 2;

		public override string Platform => "linux";
		public override string Extension => "so";

		[DllImport(LibDL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr dlopen(string filename, int flags);

		[DllImport(LibDL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr dlsym(IntPtr handle, string symbol);

		[DllImport(LibDL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr dlclose(IntPtr handle);

		[DllImport(LibDL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[CanBeNull]
		private static extern string dlerror();

		public override IntPtr Open(string path)
		{
			var result = dlopen(path, RTLD_NOW);
			if (result == default)
			{
				var error = dlerror() ?? "unknown error";
				throw new LibraryException($"dlopen({path}) failed: {error}");
			}

			return result;
		}

		public override void Close(IntPtr handle)
		{
			dlclose(handle);
		}

		public override IntPtr Get(IntPtr handle, string symbol)
		{
			var result = dlsym(handle, symbol);
			if (result == default)
			{
				var error = dlerror() ?? "unknown error";
				throw new LibraryException($"dlsym({symbol}) failed: {error}");
			}

			return result;
		}
	}
}
