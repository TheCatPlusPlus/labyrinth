using JetBrains.Annotations;

namespace Labyrinth.Utils
{
	public struct Dice
	{
		// (<Multiplier> * <Times>d<Sides>) + <Offset>
		public readonly int Times;
		public readonly int Sides;
		public readonly int Offset;
		public readonly int Multiplier;

		public Dice(int times, int sides, int multiplier = 1, int offset = 0)
		{
			Times = times;
			Sides = sides;
			Offset = offset;
			Multiplier = multiplier;
		}

		public int Roll(Xoshiro256StarStar rng)
		{
			var result = Offset;

			for (var roll = 0; roll < Times; ++roll)
			{
				result += rng.NextIntRange(1, Sides + 1);
			}

			return result * Multiplier;
		}

		[NotNull]
		public override string ToString()
		{
			var repr = $"{Times}d{Sides}";

			if (Multiplier != 1)
			{
				repr = $"{Multiplier}x{repr}";
			}

			if (Offset != 0)
			{
				repr = $"{repr}+{Offset}";
			}

			return repr;
		}
	}
}
