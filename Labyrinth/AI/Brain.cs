using Labyrinth.Entities;

namespace Labyrinth.AI
{
	public abstract class Brain
	{
		protected Game Game { get; }

		protected Brain(Game game)
		{
			Game = game;
		}

		public abstract void Act(Mob mob);
	}
}
