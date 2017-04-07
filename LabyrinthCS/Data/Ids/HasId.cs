namespace Labyrinth.Data.Ids
{
    public class HasId<T> : IHasId<T>
        where T : IHasId
    {
        public Id<T> Id { get; }
        IId IHasId.Id => Id;

        public HasId(Id<T> id)
        {
            Id = id;
        }
    }
}