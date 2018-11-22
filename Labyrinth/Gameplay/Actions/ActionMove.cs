using System.Diagnostics;

using Labyrinth.ECS;
using Labyrinth.Gameplay.Components;
using Labyrinth.Geometry;

namespace Labyrinth.Gameplay.Actions
{
	public sealed class ActionMove : ActionBase
	{
		public Int2 From { get; }
		public Int2 To { get; }

		public ActionMove(Int2 from, Int2 to)
		{
			From = from;
			To = to;
		}

		public override ActionResult Perform(Entity self)
		{
			var mobile = self.Get<Mobile>();
			var position = self.Get<Spawned>();

			// TODO move in level cache
			// TODO attacking
			// TODO opening doors

			Debug.Assert(position.Position == From, "position.Position == From");
			position.Position = To;

			return Success(mobile.MoveSpeed);
		}

		public override string ToString()
		{
			return $"move from {From} to {To}";
		}
	}
}
