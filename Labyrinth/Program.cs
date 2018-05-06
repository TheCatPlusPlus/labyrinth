using System;
using System.Globalization;

using NLog;
using NLog.Config;
using NLog.Targets;

namespace Labyrinth
{
	internal static class Program
	{
		private static void SetupLogging()
		{
			var console = new ColoredConsoleTarget("console")
			{
				Layout = @"[${logger}] [${level}] ${message} ${exception:format=toString,Data:maxInnerExceptionLevel=10}"
			};

			var config = new LoggingConfiguration();
			config.AddTarget(console);
			config.AddRule(LogLevel.Trace, LogLevel.Fatal, console);

			LogManager.Configuration = config;
		}

		private static void Main()
		{
			Environment.CurrentDirectory = Data.ExecPath;

			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

			SetupLogging();

			var game = new Game();
			game.Start();
			var ui = new UI.UI(game, 120, 45);
			ui.Run();
		}
	}
}
