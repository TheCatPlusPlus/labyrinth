using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

using Labyrinth.Utils;

namespace Labyrinth.Entities.Damage
{
	public sealed class DamageSpec
	{
		private readonly Dictionary<DamageType, int> _inflicted;
		private readonly Dictionary<DamageType, int> _resisted;

		public int Inflicted => _inflicted.Sum(p => p.Value);
		public int Resisted => _resisted.Sum(p => p.Value);
		public int Final => Math.Max(0, Inflicted - Resisted);
		public string Elements => GetElements(false);

		public DamageSpec()
			: this(new Dictionary<DamageType, int>(), new Dictionary<DamageType, int>())
		{
		}

		public DamageSpec(DamageSpec other)
			: this(other._inflicted, other._resisted)
		{
		}

		private DamageSpec(IDictionary<DamageType, int> damage, IDictionary<DamageType, int> resisted)
		{
			_inflicted = damage.ToDictionary(p => p.Key, p => p.Value);
			_resisted = resisted.ToDictionary(p => p.Key, p => p.Value);
		}

		public void Inflict(DamageType type, int value)
		{
			_inflicted.TryAdd(type, 0);
			_inflicted[type] += value;
		}

		public void Resist(DamageType type, double percentage)
		{
			Debug.Assert(percentage.WithinInclusive(0, 1), "percentage.WithinInclusive(0, 1)");

			_resisted.TryAdd(type, 0);
			_resisted[type] = (int)Math.Round(_inflicted[type] * percentage);
		}

		public void Resist(DamageType type, int flat)
		{
			_resisted.TryAdd(type, 0);
			_resisted[type] = Math.Max(0, _resisted[type] - flat);
		}

		public DamageSpec Copy()
		{
			return new DamageSpec(this);
		}

		[NotNull]
		public override string ToString()
		{
			return GetElements(true);
		}

		private string GetElements(bool debug)
		{
			var parts = new List<string>();

			foreach (var type in EnumExt.GetValues<DamageType>())
			{
				var label = type.GetLabel();
				var inflicted = _inflicted.GetValueOrDefault(type);
				var resisted = _resisted.GetValueOrDefault(type);

				if (inflicted == 0 && resisted == 0)
				{
					continue;
				}

				if (debug)
				{
					parts.Add($"{label}: +{inflicted}, -{resisted}");
				}
				else
				{
					var final = inflicted - resisted;
					if (final > 0)
					{
						parts.Add($"{final} {label}");
					}
				}
			}

			return string.Join("; ", parts);
		}
	}
}
