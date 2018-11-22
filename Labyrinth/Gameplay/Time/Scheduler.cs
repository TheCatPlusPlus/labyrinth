using Labyrinth.Gameplay.Map;

namespace Labyrinth.Gameplay.Time
{
	public sealed class Scheduler
	{
		private readonly Level _level;
		private int _current;

		public Scheduler(Level level)
		{
			_level = level;
		}

		public TickSummary Tick()
		{
			var summary = new TickSummary();

			return summary;
		}
	}
}
