using Labyrinth.Geometry;

using NLog;

namespace Labyrinth
{
	public sealed class Game
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public void Move(Direction direction)
		{
			Log.Debug($"Move: {direction}");
		}

		public void Wait()
		{
			Log.Debug("Wait");
		}

		public void Rest()
		{
			Log.Debug("Rest");
		}

		public void Discard()
		{
		}

		public void Save()
		{
		}
	}
}
