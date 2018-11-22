using System.Collections.Generic;

namespace Labyrinth.Gameplay.Time
{
	public struct TickSummary
	{
		public readonly List<EventBase> Events;

		public TickSummary(List<EventBase> events)
		{
			Events = events;
		}
	}
}
