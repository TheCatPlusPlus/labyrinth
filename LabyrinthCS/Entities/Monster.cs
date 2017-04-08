using JetBrains.Annotations;

using Labyrinth.Data.Ids;
using Labyrinth.Utils;

namespace Labyrinth.Entities
{
    public sealed class Monster : Actor
    {
        public override Name Name => Data.Name;
        public override string Description => Data.Description;

        public Monster([NotNull] Id<Monster> id)
            : base(id)
        {
        }

        public int Act()
        {
            return Const.SpeedBase;
        }
    }
}
