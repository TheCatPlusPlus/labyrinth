using System.Diagnostics;

using Labyrinth.Entities;
using Labyrinth.Geometry;
using Labyrinth.Map;

using NLog;

namespace Labyrinth
{
	public sealed class Game
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public Player Player { get; }
		public int Round { get; private set; }
		public int TotalCost { get; private set; }
		public int LastCost { get; private set; }

		public Game()
		{
			Player = new Player("Player");
			var level = new Level("Test", 200, 200);

			Player.Spawn(level);
		}

		public void Move(Direction direction)
		{
			var cost = Player.TryMove(direction);
			if (cost > 0)
			{
				AdvanceRound(cost);
			}
		}

		public void MoveDown()
		{
			Log.Debug("MoveDown");
		}

		public void MoveUp()
		{
			Log.Debug("MoveUp");
		}

		public void Wait()
		{
			Log.Debug("Wait");
			AdvanceRound();
		}

		public void Discard()
		{
			Log.Debug("Discard");
		}

		public void Save()
		{
			Log.Debug("Save");
		}

		private void AdvanceRound(int? cost = null)
		{
			Debug.Assert(Player.Position != null, "Player.Position != null");
			Debug.Assert(Player.Level != null, "Player.Level != null");

			var actualCost = cost ?? Player.Speed.EffectiveValue;
			Log.Debug($"AdvanceRound: spent {actualCost} energy");

			++Round;
			TotalCost += actualCost;
			LastCost = actualCost;

			Player.Energy.Drain(actualCost);
			// timers (regen etc) tick at the base speed so doing an action
			// that takes 3x base time will advance them by 3 ticks
			Player.Level.Tick(actualCost / (float)Scheduler.BaseSpeed);
		}
	}
}
