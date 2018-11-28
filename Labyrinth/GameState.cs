using Labyrinth.ECS;
using Labyrinth.Gameplay.Actions.Player;
using Labyrinth.Gameplay.Database;
using Labyrinth.Utils;

using NLog;

namespace Labyrinth
{
	public sealed class GameState
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public Xoshiro256StarStar RNG { get; }
		public PrefabRegistry Prefabs { get; }
		public World World { get; }
		public EntityID Player { get; }
		public InputBase CurrentInput { get; set; }

		public GameState()
		{
			RNG = new Xoshiro256StarStar();
			Prefabs = PrefabDatabase.Collect();
			World = new World(Prefabs);
			Player = World.Create("Player");
		}
	}
}
