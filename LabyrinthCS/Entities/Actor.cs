using System;
using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Data.Ids;
using Labyrinth.Entities.Time;
using Labyrinth.Utils;

namespace Labyrinth.Entities
{
    public abstract class Actor : Entity, IHasId<Monster>, IComparable<Actor>, IEnergy
    {
        protected readonly MonsterData Data;
        public Id<Monster> Id { get; }
        IId IHasId.Id => Id;

        public abstract Name Name { get; }
        public abstract string Description { get; }
        public virtual decimal SpeedFactor => Data.SpeedFactor;

        public int Speed => MoveCost.ApplyFactor(SpeedFactor);
        public int Energy { get; private set; }
        public bool IsAlive => true;

        protected Actor([NotNull] Id<Monster> id)
        {
            Id = id;
            Data = MonsterData.For(Id);
            Energy = Speed;
        }

        public int CompareTo(Actor other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return Energy.CompareTo(other.Energy);
        }

        public override string ToString()
        {
            return $"{Name.Singular()} ({Id})";
        }

        void IEnergy.Recharge()
        {
            if (Energy > 0)
            {
                return;
            }

            Energy += Speed;
            Log.Verbose(Log.Category.Scheduler, $"{this}: Energy.Recharge() = {Energy}");
        }

        void IEnergy.Use(int cost)
        {
            Debug.Assert(cost >= 0);

            Energy -= cost;
            Log.Verbose(Log.Category.Scheduler, $"{this}: Energy.Use({cost}) = {Energy}");
        }
    }
}
