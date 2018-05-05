using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Labyrinth
{
	public static class Data
	{
		public static string ExecPath { get; }
		public static string AssetsPath { get; }
		public static string UserPath { get; }

		static Data()
		{
			ExecPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
			AssetsPath = Path.Combine(ExecPath, "Assets");
			UserPath = FindUserPath();

			Directory.CreateDirectory(UserPath);
		}

		private static string FindUserPath()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return FindUserPath(Environment.SpecialFolder.ApplicationData);
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				// this corresponds to XDG_DATA_HOME
				return FindUserPath(Environment.SpecialFolder.LocalApplicationData);
			}

			throw new PlatformNotSupportedException();
		}

		private static string FindUserPath(Environment.SpecialFolder dataFolder)
		{
			var basePath = Environment.GetFolderPath(dataFolder);
			return Path.Combine(basePath, "CatPlusPlus", "Labyrinth");
		}
	}
}
