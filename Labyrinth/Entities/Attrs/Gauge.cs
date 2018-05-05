using JetBrains.Annotations;

using Labyrinth.Utils;

namespace Labyrinth.Entities.Attrs
{
	public sealed class Gauge
	{
		private readonly string _name;
		private readonly Attribute _maxValue;
		private int _current;
		private int _previous;

		public int MaxValue => _maxValue.EffectiveValue;
		public bool Changed => _current != _previous;
		public float CurrentPercent => _current / (float)MaxValue;
		public float PreviousPercent => _previous / (float)MaxValue;

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
			_current = _previous = MaxValue;
		}

		public void Settle()
		{
			_previous = _current;
		}

		[NotNull]
		public override string ToString()
		{
			return $"{_name}: {_current} / {MaxValue} ({_previous})";
		}
	}
}
