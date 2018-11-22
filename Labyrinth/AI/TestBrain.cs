using System.Diagnostics;
using System.Linq;

using Labyrinth.Entities;
using Labyrinth.Geometry.Paths;
using Labyrinth.Utils;

using NLog;

namespace Labyrinth.AI
{
	public sealed class TestBrain : Brain
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public TestBrain(GamePrev game)
			: base(game)
		{
		}

		public override void Act(Mob mob)
		{
			if ((mob.Position == null) || (mob.Level == null))
			{
				return;
			}

			var position = mob.Position.Value;
			if (Game.Player.FOV.Visible.Any(tile => position == tile.Position))
			{
				ChasePlayer(mob);
				return;
			}

			Wander(mob);
		}

		private void Wander(Mob mob)
		{
			Debug.Assert(mob.Position != null, "mob.Position != null");
			Debug.Assert(mob.Level != null, "mob.Level != null");

			var position = mob.Position.Value;
			var tile = mob.Level.Grid[position];
			Debug.Assert(tile != null, "tile != null");

			var candidates = tile.Neighbours.Where(mob.CanGetThrough).ToList();
			var next = Game.RNG.Pick(candidates);
			var delta = next.Position - tile.Position;

			Log.Debug($"{mob}: AI: Wander by {delta} (from {tile.Position} to {next.Position})");
			mob.TryMove(delta);
		}

		private void ChasePlayer(Mob mob)
		{
			// TODO horribly broken

			Debug.Assert(Game.Player.Position != null, "Game.Player.Position != null");
			Debug.Assert(mob.Position != null, "mob.Position != null");

			var path = AStar.Find(mob, Game.Player.Position.Value);
			if (path == null)
			{
				Wander(mob);
				return;
			}

			var next = path.Value.Points[0];
			var position = mob.Position.Value;
			var delta = next - position;

			Log.Debug($"{mob}: AI: Chase by {delta} (from {position} to {next})");
			mob.TryMove(delta);
		}
	}
}
