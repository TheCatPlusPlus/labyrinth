using System;

namespace BearLib.Native
{
	internal abstract class LibraryImpl
	{
		public abstract string Platform { get; }
		public abstract string Extension { get; }

		public abstract IntPtr Open(string path);
		public abstract void Close(IntPtr handle);
		public abstract IntPtr Get(IntPtr handle, string symbol);
	}
}
