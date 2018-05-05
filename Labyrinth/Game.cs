using System.Collections.Generic;
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
		private static readonly Dictionary<Direction, Int2> Movement = new Dictionary<Direction, Int2>
		{
			{ Direction.North, Int2.North },
			{ Direction.South, Int2.South },
			{ Direction.West, Int2.West },
			{ Direction.East, Int2.East },
			{ Direction.NorthEast, Int2.NorthEast },
			{ Direction.NorthWest, Int2.NorthWest },
			{ Direction.SouthEast, Int2.SouthEast },
			{ Direction.SouthWest, Int2.SouthWest }
		};

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
			Debug.Assert(Player.Position != null, "Player.Position != null");
			Debug.Assert(Player.Level != null, "Player.Level != null");

			var from = Player.Position.Value;
			var to = from + Movement[direction];
			var fromTile = Player.Level.Grid[from];
			var toTile = Player.Level.Grid[to];

			// 0. if target is not in bounds, do nothing
			if (toTile == null)
			{
				Log.Debug($"Move({direction}): from {from} to {to}: target out of bounds");
				return;
			}

			// 1. if tile has another creature, fight
			if (toTile.Creature != null)
			{
				Log.Debug($"Move({direction}): from {from} to {to}: fighting {toTile.Creature}");
				Player.Attack(toTile.Creature);
				AdvanceRound(Player.AttackSpeed.EffectiveValue);
				return;
			}

			// 2. if tile is walkable (via Player to allow for flight effects etc), walk
			// the cost is determined by the tile we're leaving
			Debug.Assert(fromTile != null, "fromTile != null");
			var multiplier = Player.GetMoveCost(fromTile.Type);
			if (multiplier > 0)
			{
				Log.Debug($"Move({direction}): from {from} to {to}: {multiplier}x");
				AdvanceRound(Player.Speed.EffectiveValue * multiplier);
				return;
			}

			// 3. otherwise, do nothing
			Log.Debug($"Move({direction}): from {from} to {to}: target not walkable");
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
