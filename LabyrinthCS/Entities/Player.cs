using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Maps.FOV;
using Labyrinth.Utils;

namespace Labyrinth.Entities
{
    public sealed class Player : Actor
    {
        public override Name Name { get; }
        public override string Description => Data.Description;
        public override float SpeedFactor => 1.0f; // TODO effects

        public Gauge HP { get; }
        public Gauge MP { get; }
        public Gauge Stamina { get; }
        public FieldOfView FieldOfView { get; }

        public Player([NotNull] string name)
            : base(MonsterData.Player)
        {
            Name = new Name(name, unique: true, proper: true);
            HP = new Gauge("HP", 100);
            MP = new Gauge("MP", 100);
            Stamina = new Gauge("Stamina", 50);
            FieldOfView = new FieldOfView(this);
        }
    }
}
