using System;

namespace Labyrinth.Utils
{
	public sealed class Guard : IDisposable
	{
		private readonly Action _action;

		public Guard(Action action)
		{
			_action = action;
		}

		public void Dispose()
		{
			_action();
		}
	}
}
