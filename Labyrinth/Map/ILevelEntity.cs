using Labyrinth.Entities;
using Labyrinth.Geometry;

namespace Labyrinth.Map
{
	// implemented explicitly to avoid calling Level methods instead of Entity methods
	public interface ILevelEntity
	{
		Int2 Spawn(Entity entity, Int2? target);
		void Move(Entity entity, Int2 from, Int2 to);
		void Despawn(Entity entity);
	}
}
