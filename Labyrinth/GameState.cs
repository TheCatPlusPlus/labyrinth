using Labyrinth.ECS;
using Labyrinth.Gameplay.Actions.Player;
using Labyrinth.Utils;

using NLog;

namespace Labyrinth
{
	public sealed class GameState
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public Xoshiro256StarStar RNG { get; }
		public World World { get; }
		public Entity Player { get; }
		public InputBase CurrentInput { get; set; }

		public GameState()
		{
			RNG = new Xoshiro256StarStar();
			World = new World();
		}
	}
}
