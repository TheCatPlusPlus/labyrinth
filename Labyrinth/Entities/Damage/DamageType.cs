using System;

namespace Labyrinth.Entities.Damage
{
	public enum DamageType
	{
		Blunt,
		Slashing,
		Piercing,
		Fire,
		Ice
	}

	public static class DamageTypeExt
	{
		public static string GetLabel(this DamageType type)
		{
			switch (type)
			{
				case DamageType.Blunt:
					return "blunt";
				case DamageType.Slashing:
					return "slashing";
				case DamageType.Piercing:
					return "piercing";
				case DamageType.Fire:
					return "fire";
				case DamageType.Ice:
					return "ice";
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}
