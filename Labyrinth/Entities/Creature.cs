using System;

using Labyrinth.Entities.Attrs;
using Labyrinth.Map;
using Labyrinth.Utils;

using Attribute = Labyrinth.Entities.Attrs.Attribute;

namespace Labyrinth.Entities
{
	public abstract class Creature : Entity
	{
		public Name Name { get; }
		public string Description { get; }

		public Attribute MaxHP { get; }
		public Attribute Speed { get; }
		public Attribute AttackSpeed { get; }

		public Energy Energy { get; }
		public Gauge HP { get; }

		public bool IsAlive => HP.Value > 0;

		protected Creature(EntityID id, Name name, string desc, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(id)
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

		public override string ToString()
		{
			return $"{base.ToString()}: {Name} ({HP}, {Energy})";
		}
	}
}
