using System;

namespace BearLib.Native
{
	internal sealed class OSXLibrary : LibraryImpl
	{
		public override string Platform => "osx";
		public override string Extension => "dylib";

		public override IntPtr Open(string path)
		{
			throw new NotImplementedException();
		}

		public override void Close(IntPtr handle)
		{
			throw new NotImplementedException();
		}

		public override IntPtr Get(IntPtr handle, string symbol)
		{
			throw new NotImplementedException();
		}
	}
}
