using Labyrinth.Utils;

namespace Labyrinth
{
    public class Gauge
    {
        private readonly string _name;
        private int _current;
        private int _previous;

        public int Value
        {
            get => _current;
            set
            {
                Settle();
                _current = MathExt.Clamp(value, 0, MaxValue);
            }
        }

        public int MaxValue { get; }
        public bool Changed => _current != _previous;
        public float CurrentPercent => _current / (float)MaxValue;
        public float PreviousPercent => _previous / (float)MaxValue;

        public Gauge(string name, int max)
        {
            _name = name;
            MaxValue = max;
            _current = max;
            _previous = max;
        }

        public void Settle()
        {
            _previous = _current;
        }

        public override string ToString()
        {
            return $"{_name}: {_current} / {MaxValue} ({_previous})";
        }
    }
}
