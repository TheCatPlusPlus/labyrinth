using System.Collections.Generic;
using System.Diagnostics;

using Labyrinth.AI;
using Labyrinth.Database;
using Labyrinth.Entities;
using Labyrinth.Geometry;
using Labyrinth.Journal;
using Labyrinth.Map;
using Labyrinth.Map.Gen;
using Labyrinth.Utils;

using NLog;

namespace Labyrinth
{
	public sealed class Game
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		private ulong _nextMessage;

		public Xoshiro256StarStar RNG { get; }
		public Player Player { get; }
		public ulong Round { get; private set; }
		public long TotalCost { get; private set; }
		public int LastCost { get; private set; }
		public LinkedList<Message> Messages { get; }

		public Level Level
		{
			get
			{
				Debug.Assert(Player.Level != null, "Player.Level != null");
				Debug.Assert(Player.Position != null, "Player.Position != null");
				return Player.Level;
			}
		}

		public Game()
		{
			RNG = new Xoshiro256StarStar();
			Messages = new LinkedList<Message>();
			Player = new Player(this);
		}

		public void Start()
		{
			var gen = new TestLevelGen(this);
			var level = gen.Create("Test", 66, 23, 0);

			Player.Spawn(level);
			Player.FOV.Recalculate();

			var rat1 = Mob.Create(this, DB.CreatureRat);
			var rat2 = Mob.Create(this, DB.CreatureRat);

			rat1.Spawn(level);
			rat2.Spawn(level);

			Message("Welcome to [color=yellow]the Labyrinth[/color].");
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

		public void Message(string text, MessageType type = MessageType.Normal)
		{
			var id = ++_nextMessage;
			var message = new Message(id, Round, text, type);
			Messages.AddLast(message);
			Log.Debug($"Message: {text}");
		}

		private void AdvanceRound(int? cost = null)
		{
			var actualCost = cost ?? Player.Speed.EffectiveValue;
			Log.Debug($"AdvanceRound: spent {actualCost} energy");

			++Round;
			TotalCost += actualCost;
			LastCost = actualCost;

			Player.Energy.Drain(actualCost);
			// timers (regen etc) tick at the base speed so doing an action
			// that takes 3x base time will advance them by 3 ticks
			Level.Tick(actualCost / (float)Scheduler.BaseSpeed);

			Player.FOV.Recalculate();
		}
	}
}
