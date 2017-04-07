using Labyrinth.Entities;

namespace Labyrinth.Maps
{
    // these should only be called from Entity internals
    // explicit implementation is used
    // (so that they're not normally visible)
    public interface IEntityList
    {
        bool AddEntity(Entity entity);
        void RemoveEntity(Entity entity);
    }
}