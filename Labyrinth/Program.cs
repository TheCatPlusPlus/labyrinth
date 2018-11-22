using System;
using System.Globalization;

using BearLib;

using Labyrinth.Geometry;
using Labyrinth.UI;

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

			var state = new GameState();
//
			TerminalExt.Setup("Labyrinth", 80, 35);
			Terminal.Print(1, 1, "@.R.x.A._.#");
			TerminalExt.Box(new Rect(5, 5, 10, 10));
			Terminal.Refresh();
			while(Terminal.Read() != Code.Close);

//			var loop = new Loop();
//			loop.Run();

//			TerminalExt.Setup("Labyrinth", 120, 45);
//			Tileset.Load();
//			Terminal.Refresh();
//
//			Terminal.Print(0, 0, "main font test [font=italic]italic[/font] [font=bold]bold[/font] [font=bold-italic]bold italic[/font]");
//			Terminal.Put(0, 3, Tileset.Get("Player/PlayerTemp"));
//			Terminal.Refresh();
//
//			while (true)Terminal.Read();

//			var game = new Game();
//			game.Start();
//			var ui = new UI.UI(game, 120, 45);
//			ui.Run();
		}
	}
}
