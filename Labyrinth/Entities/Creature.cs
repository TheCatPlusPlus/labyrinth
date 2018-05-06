using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Database;
using Labyrinth.Entities.Attrs;
using Labyrinth.Geometry;
using Labyrinth.Map;

using NLog;

namespace Labyrinth.Entities
{
	public abstract class Creature : Entity
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public Attribute MaxHP { get; }
		public Attribute Speed { get; }
		public Attribute AttackSpeed { get; }

		public Energy Energy { get; }
		public Gauge HP { get; }

		public bool IsAlive => HP.Value > 0;
		public virtual bool CanOpenDoors => false;

		protected Creature(Game game, EntityID id, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(game, id)
		{
			id.RequireNamespace(EntityID.Creatures);

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
		public virtual int GetMoveCost(Tile to)
		{
			var tile = DB.Tiles.Get(to.Type);
			return tile.CostMultiplier;
		}

		public virtual bool CanWalkOn(Tile from, Tile to, out int multiplier)
		{
			multiplier = GetMoveCost(from);
			return CanWalkOn(to);
		}

		public virtual bool CanWalkOn(Tile tile)
		{
			return tile.EffectiveFlags.Contains(TileFlag.Walkable);
		}

		public virtual bool CanGetThrough(Tile tile)
		{
			if (tile.EffectiveFlags.Contains(TileFlag.Door))
			{
				return CanOpenDoors;
			}

			return CanWalkOn(tile);
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

			// 1. if target is not in bounds, do nothing
			if (toTile == null)
			{
				Log.Debug($"TryMove({direction}): from {from} to {to}: target out of bounds");
				return 0;
			}

			// 2. if tile has another creature, fight
			if (toTile.Creature != null)
			{
				Log.Debug($"TryMove({direction}): from {from} to {to}: fighting {toTile.Creature}");
				Attack(toTile.Creature);
				return AttackSpeed.EffectiveValue;
			}

			// 3. if tile is a door and we're capable of opening doors, open it
			if (toTile.EffectiveFlags.Contains(TileFlag.Door) && CanOpenDoors)
			{
				Log.Debug($"TryMove({direction}): from {from} to {to}: opening the door");
				toTile.Type = toTile.Type.GetOpened();
				return Speed.EffectiveValue;
			}

			// 4. if tile is walkable (via Player to allow for flight effects etc), walk
			// the cost is determined by the tile we're leaving
			Debug.Assert(fromTile != null, "fromTile != null");
			if (CanWalkOn(fromTile, toTile, out var multiplier))
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
			return $"{base.ToString()}: {HP}, {Energy}";
		}
	}
}
