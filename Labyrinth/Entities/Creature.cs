using System;
using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Entities.Attrs;
using Labyrinth.Geometry;
using Labyrinth.Map;
using Labyrinth.Utils;

using NLog;

using Attribute = Labyrinth.Entities.Attrs.Attribute;

namespace Labyrinth.Entities
{
	public abstract class Creature : Entity
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public Name Name { get; }
		public string Description { get; }

		public Attribute MaxHP { get; }
		public Attribute Speed { get; }
		public Attribute AttackSpeed { get; }

		public Energy Energy { get; }
		public Gauge HP { get; }

		public bool IsAlive => HP.Value > 0;

		protected Creature(Game game, EntityID id, Name name, string desc, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(game, id)
		{
			id.RequireNamespace(EntityID.Creatures);

			Name = name;
			Description = desc;

			MaxHP = new Attribute("Max HP", maxHP, CalcMaxHP);
			// general speed is used for most actions, attacks are separate to allow
			// weapon mastery effects etc
			Speed = new Attribute("Speed", speed, CalcSpeed);
			AttackSpeed = new Attribute("Attack speed", speed, CalcAttackSpeed);

			Energy = new Energy(Speed);
			HP = new Gauge("HP", MaxHP);
		}

		protected virtual int CalcMaxHP(int baseValue) => baseValue;
		protected virtual int CalcSpeed(int baseValue) => baseValue;
		protected virtual int CalcAttackSpeed(int baseValue) => baseValue;

		// the result is multiplied by the creature speed
		// if the cost is <= 0 then the tile is not walkable
		public virtual int GetMoveCost(TileType target)
		{
			switch (target)
			{
				case TileType.Floor:
					return 1;
				case TileType.Wall:
					return 0;
				default:
					throw new ArgumentOutOfRangeException(nameof(target), target, null);
			}
		}

		public void Attack(Creature target)
		{
		}

		public void TakeDamage([CanBeNull] Creature attacker)
		{
			if (!IsAlive)
			{
				Despawn();
			}
		}

		public int TryMove(Direction direction)
		{
			Debug.Assert(Position != null, "Position != null");
			Debug.Assert(Level != null, "Level != null");

			var from = Position.Value;
			var to = from + Int2.Movement[direction];
			var fromTile = Level.Grid[from];
			var toTile = Level.Grid[to];

			// 0. if target is not in bounds, do nothing
			if (toTile == null)
			{
				Log.Debug($"TryMove({direction}): from {from} to {to}: target out of bounds");
				return 0;
			}

			// 1. if tile has another creature, fight
			if (toTile.Creature != null)
			{
				Log.Debug($"TryMove({direction}): from {from} to {to}: fighting {toTile.Creature}");
				Attack(toTile.Creature);
				return AttackSpeed.EffectiveValue;
			}

			// 2. if tile is walkable (via Player to allow for flight effects etc), walk
			// the cost is determined by the tile we're leaving
			Debug.Assert(fromTile != null, "fromTile != null");
			var multiplier = GetMoveCost(fromTile.Type);
			if (multiplier > 0)
			{
				Log.Debug($"TryMove({direction}): from {from} to {to}: {multiplier}x");
				Move(to);
				return Speed.EffectiveValue * multiplier;
			}

			// 3. otherwise, do nothing
			Log.Debug($"TryMove({direction}): from {from} to {to}: target not walkable");
			return 0;
		}

		public override string ToString()
		{
			return $"{base.ToString()}: {Name} ({HP}, {Energy})";
		}
	}
}
