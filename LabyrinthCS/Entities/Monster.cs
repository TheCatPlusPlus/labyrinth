using Labyrinth.Data;
using Labyrinth.Data.Ids;
using Labyrinth.Utils;

namespace Labyrinth.Entities
{
    public class Monster : Entity, IHasId<Monster>
    {
        private readonly MonsterData _data;

        public Id<Monster> Id { get; }
        IId IHasId.Id => Id;

        public virtual Name Name => _data.Name;
        public string Description => _data.Description;

        public Monster(Id<Monster> id)
        {
            Id = id;
            _data = MonsterData.For(Id);
        }
    }
}
