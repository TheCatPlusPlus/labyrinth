using Labyrinth.Entities;

namespace Labyrinth.AI
{
	public abstract class Brain
	{
		protected GamePrev Game { get; }

		protected Brain(GamePrev game)
		{
			Game = game;
		}

		public abstract void Act(Mob mob);
	}
}
