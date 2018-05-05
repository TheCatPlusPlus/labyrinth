using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using NLog;

namespace Labyrinth.Entities
{
	public sealed class Scheduler
	{
		[SuppressMessage("ReSharper", "UnusedMember.Local")]
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public const int BaseSpeed = 10;

		private readonly LinkedList<Creature> _creatures;
		private bool _hasPlayer;

		public Scheduler()
		{
			_creatures = new LinkedList<Creature>();
		}

		public void Advance()
		{
			while (_hasPlayer)
			{
				var creature = _creatures.First.Value;
				var energy = creature.Energy;

//				Log.Debug($"Turn: {creature} ({energy})");

				switch (creature)
				{
					case Player player:
					{
						if (!player.IsAlive)
						{
							return;
						}

						if (energy.Current > 0)
						{
							return;
						}

						break;
					}
					case Mob mob:
						while ((energy.Current > 0) && creature.IsAlive)
						{
							var speed = energy.Speed;
							mob.Act();
							energy.Drain(speed);
						}

						break;
				}

				energy.Recharge();

				_creatures.RemoveFirst();
				_creatures.AddLast(creature);
			}
		}

		public void Add(Creature creature)
		{
			_creatures.AddLast(creature);

			if (creature is Player)
			{
				_hasPlayer = true;
			}
		}

		public void Remove(Creature creature)
		{
			_creatures.Remove(creature);

			if (creature is Player)
			{
				_hasPlayer = false;
			}
		}
	}
}
