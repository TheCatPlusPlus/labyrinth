using JetBrains.Annotations;

namespace Labyrinth.Entities.Attrs
{
	public sealed class Energy
	{
		private readonly Attribute _speed;

		public int Speed => _speed.EffectiveValue;
		public int Current { get; private set; }

		public Energy(Attribute speed)
		{
			_speed = speed;
		}

		public void Recharge()
		{
			if (Current > 0)
			{
				return;
			}

			Current += Speed;
		}

		public void Drain(int cost)
		{
			Current -= cost;
		}

		public void Freeze(int turns)
		{
			Current = -Speed * turns;
		}

		[NotNull]
		public override string ToString()
		{
			return $"{nameof(Energy)}: {Current} ({Speed}/turn)";
		}
	}
}
