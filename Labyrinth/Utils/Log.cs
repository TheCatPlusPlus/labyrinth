using System;

namespace Labyrinth.Utils
{
    public static class Log
    {
        [Flags]
        public enum Category : ulong
        {
            Scheduler = 1 << 0
        }

        private static void Write(ConsoleColor color, string message)
        {
            var previous = Console.ForegroundColor;
            var now = new DateTime().ToShortTimeString();
            Console.ForegroundColor = color;
            Console.WriteLine($"[{now}] {message}");
            Console.ForegroundColor = previous;
        }

        public static void Verbose(Category category, string message)
        {
            if ((Const.EnabledVerbose & category) != 0)
            {
                Write(ConsoleColor.DarkGray, $"[VERBOSE] {message}");
            }
        }

        public static void Debug(string message)
        {
            Write(ConsoleColor.DarkGray, $"[DEBUG] {message}");
        }

        public static void Info(string message)
        {
            Write(ConsoleColor.Gray, $"[INFO] {message}");
        }

        public static void Warning(string message)
        {
            Write(ConsoleColor.Yellow, $"[WARNING] {message}");
        }

        public static void Error(string message)
        {
            Write(ConsoleColor.Red, $"[ERROR] {message}");
        }
    }
}
