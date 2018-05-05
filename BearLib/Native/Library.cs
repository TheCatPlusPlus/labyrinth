using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace BearLib.Native
{
	internal sealed class Library : IDisposable
	{
		private readonly LibraryImpl _impl;
		private IntPtr _handle;

		public bool IsOpen => _handle != IntPtr.Zero;

		public Library()
		{
			switch (GetPlatform())
			{
				case Platform.Windows:
					_impl = new WindowsLibrary();
					break;
				case Platform.Linux:
					_impl = new LinuxLibrary();
					break;
				case Platform.OSX:
					_impl = new OSXLibrary();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		~Library()
		{
			Close();
		}

		public void Open(string name)
		{
			var path = Find(name);

			try
			{
				_handle = _impl.Open(path);
			}
			catch (Exception e)
			{
				throw new DllNotFoundException($"Failed to load {name} from {path}", e);
			}

			if (!IsOpen)
			{
				throw new DllNotFoundException($"Failed to load {name} from {path}");
			}
		}

		public void Close()
		{
			if (_handle != IntPtr.Zero)
			{
				_impl.Close(_handle);
				_handle = IntPtr.Zero;
			}
		}

		public void Get<T>([NotNull] out T function, string name)
		{
			Check();

			IntPtr ptr;
			try
			{
				ptr = _impl.Get(_handle, name);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException($"Failed to get symbol {name}", e);
			}

			if (ptr == IntPtr.Zero)
			{
				throw new InvalidOperationException($"Failed to get symbol {name}");
			}

			var @delegate = (object)Marshal.GetDelegateForFunctionPointer(ptr, typeof(T));
			function = (T)@delegate;
		}

		[Conditional("DEBUG")]
		public void Check()
		{
			if (!IsOpen)
			{
				throw new InvalidOperationException("The library has not been loaded yet");
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Close();
		}

		private string Find(string name)
		{
			var here = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Debug.Assert(here != null, "GetDirectoryName(GetExecutingAssembly().Location) != null");

			var bits = Environment.Is64BitProcess ? "64" : "32";
			var basePath = Path.Combine(here, "Libs");
			var dllPath = Path.Combine(basePath, $"{_impl.Platform}{bits}", $"{name}.{_impl.Extension}");

			if (!File.Exists(dllPath))
			{
				throw new PlatformNotSupportedException($"No DLL for this platform, tried: {dllPath}");
			}

			return dllPath;
		}

		private static Platform GetPlatform()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return Platform.Windows;
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return Platform.Linux;
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return Platform.OSX;
			}

			throw new PlatformNotSupportedException();
		}
	}
}
