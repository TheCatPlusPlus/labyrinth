using System.Collections.Generic;
using System.Drawing;

using Labyrinth.Entities;
using Labyrinth.Entities.Damage;
using Labyrinth.Utils;

namespace Labyrinth.Database
{
	public sealed class EntityDB
	{
		private readonly Dictionary<EntityID, ItemData> _items = new Dictionary<EntityID, ItemData>();

		private readonly Dictionary<EntityID, CreatureData> _creatures = new Dictionary<EntityID, CreatureData>
		{
			{
				DB.CreaturePlayer, new CreatureData("Player", unique: true, proper: true)
				{
					Glyph = new GlyphData('@')
					{
						Fore = Color.White
					}
				}
			},
			{
				DB.CreatureRat, new CreatureData("rat")
				{
					HP = 10,
					MeleeDamage = new Die(1, 6),
					MeleeDamageType = DamageType.Piercing,
					Glyph = new GlyphData('r')
					{
						Fore = Color.White
					}
				}
			}
		};

		public CreatureData Get(Creature creature)
		{
			return GetCreature(creature.ID);
		}

		public ItemData Get(Item item)
		{
			return GetItem(item.ID);
		}

		public CreatureData GetCreature(EntityID id)
		{
			return _creatures[id];
		}

		public ItemData GetItem(EntityID id)
		{
			return _items[id];
		}
	}
}
