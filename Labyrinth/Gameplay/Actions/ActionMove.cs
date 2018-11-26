using System.Diagnostics;

using Labyrinth.ECS;
using Labyrinth.Gameplay.Components;
using Labyrinth.Geometry;

namespace Labyrinth.Gameplay.Actions
{
	public sealed class ActionMove : ActionBase
	{
		private readonly Int2 _from;
		private readonly Int2 _to;

		public ActionMove(GameState state, EntityID self, Int2 from, Int2 to)
			: base(state, self)
		{
			_from = from;
			_to = to;
		}

		public override ActionResult Perform()
		{
			var mobile = World.Get<Mobile>(Self);
			var position = World.Get<Spawned>(Self);

			// TODO move in level cache
			// TODO attacking
			// TODO opening doors

			Debug.Assert(position.Position == _from, "position.Position == From");
			position.Position = _to;

			return Success(mobile.MoveSpeed);
		}

		public override string ToString()
		{
			return $"move from {_from} to {_to}";
		}
	}
}
