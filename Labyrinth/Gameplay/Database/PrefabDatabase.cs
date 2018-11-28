using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Labyrinth.ECS;
using Labyrinth.Gameplay.Components;
using Labyrinth.Utils;

namespace Labyrinth.Gameplay.Database
{
	public static class PrefabDatabase
	{
		[MeansImplicitUse]
		private sealed class CollectedAttribute : Attribute
		{
		}

		public static PrefabRegistry Collect()
		{
			var registry = new PrefabRegistry();
			var methods =
				from method in typeof(PrefabDatabase).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
				where method.ReturnType == typeof(Prefab)
				where method.GetParameters().Length == 0
				where method.GetCustomAttribute<CollectedAttribute>() != null
				orderby method.Name
				select method;

			foreach (var method in methods)
			{
				var prefab = (Prefab)method.Invoke(null, null);
				var name = method.Name.Replace("_", "/");
				registry.Add(name, prefab);
			}

			return registry;
		}

		private static Prefab Merge(Prefab @base, Prefab prefab)
		{
			foreach (var component in @base)
			{
				prefab.Add(component.DeepClone());
			}

			return prefab;
		}

		private static Prefab Creature()
		{
			return new Prefab
			{
				new Killable()
			};
		}

		[Collected]
		private static Prefab Creatures_Rat()
		{
			return new Prefab(Creature())
			{
				new Creature
				{
					Name = "rat",
					Description = "a rat"
				}
			};
		}
	}
}
