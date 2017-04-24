namespace Labyrinth.Data.Ids
{
    public interface IHasId
    {
        IId Id { get; }
    }

    public interface IHasId<T> : IHasId
        where T : IHasId
    {
        new Id<T> Id { get; }
    }
}