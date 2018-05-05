using JetBrains.Annotations;

using Labyrinth.Utils;

namespace Labyrinth.Entities.Attrs
{
	public sealed class Gauge
	{
		private readonly string _name;
		private readonly Attribute _maxValue;
		private int _current;

		public int MaxValue => _maxValue.EffectiveValue;
		public bool HasChanged => _current != PreviousValue;
		public float CurrentPercent => _current / (float)MaxValue;
		public float PreviousPercent => PreviousValue / (float)MaxValue;
		public int PreviousValue { get; private set; }

		public int Value
		{
			get => _current;
			set
			{
				Settle();
				_current = MathExt.Clamp(value, 0, MaxValue);
			}
		}

		public Gauge(string name, Attribute maxValue)
		{
			_name = name;
			_maxValue = maxValue;
			_current = PreviousValue = MaxValue;
		}

		public void Settle()
		{
			PreviousValue = _current;
		}

		[NotNull]
		public override string ToString()
		{
			return $"{_name}: {_current} / {MaxValue} ({PreviousValue})";
		}
	}
}
