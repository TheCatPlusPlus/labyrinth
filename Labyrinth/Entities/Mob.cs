namespace Labyrinth.Entities
{
	public sealed class Mob : Creature
	{
		public Mob(Game game, EntityID id, int maxHP, int speed = Scheduler.BaseSpeed)
			: base(game, id, maxHP, speed)
		{
			id.RequireNamespace(EntityID.Mobs);
		}

		public void Act()
		{
		}
	}
}
