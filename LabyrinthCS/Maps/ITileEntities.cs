using Labyrinth.Entities;

namespace Labyrinth.Maps
{
    // these should only be called from Entity internals
    // explicit implementation in Tile is used
    // (so that they're not normally visible)
    public interface ITileEntities
    {
        bool AddEntity(Entity entity);
        void RemoveEntity(Entity entity);
    }
}