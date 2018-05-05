using System;

using JetBrains.Annotations;

namespace Labyrinth.Entities.Attrs
{
	public sealed class Attribute
	{
		private readonly string _name;
		private readonly Func<int, int> _getEffective;

		public int BaseValue { get; set; }
		public int EffectiveValue => _getEffective(BaseValue);

		public Attribute(string name, int baseValue, Func<int, int> getEffective)
		{
			_name = name;
			BaseValue = baseValue;
			_getEffective = getEffective;
		}

		[NotNull]
		public override string ToString()
		{
			return $"{_name}: {EffectiveValue} (base {BaseValue})";
		}
	}
}
