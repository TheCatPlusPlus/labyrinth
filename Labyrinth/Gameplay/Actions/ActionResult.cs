using JetBrains.Annotations;

namespace Labyrinth.Gameplay.Actions
{
	public struct ActionResult
	{
		public readonly int UsedEnergy;
		public readonly bool IsDone;
		[CanBeNull]
		public readonly ActionBase Alternate;

		public ActionResult(int usedEnergy, bool done)
		{
			UsedEnergy = usedEnergy;
			IsDone = done;
			Alternate = null;
		}

		public ActionResult(ActionBase alternate)
		{
			UsedEnergy = 0;
			IsDone = true;
			Alternate = alternate;
		}

		[NotNull]
		public override string ToString()
		{
			if (Alternate != null)
			{
				return $"do {Alternate} instead";
			}

			var done = IsDone ? "done" : "continues";
			return $"used {UsedEnergy} energy, {done}";
		}
	}
}
