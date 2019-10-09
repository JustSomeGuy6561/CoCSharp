using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables
{
	public abstract class WearableItemBase : CapacityItem
	{
		protected WearableItemBase(SimpleDescriptor shortName, SimpleDescriptor fullName) : base(shortName, fullName)
		{
		}

		protected abstract bool CanWearWithBodyData(Creature creature);

		/// <summary>
		/// Attempt to equip the item. if it can be equipped, returns the equipped item it is replacing, or null if no such equipment exists.
		/// Additionally, apply any perks, reactions, and/or status effects as necessary.
		/// </summary>
		/// <param name="wearer">The creature equipping this wearable item.</param>
		/// <returns>The wearable item this object replaces, or null if none exists.</returns>
		protected abstract WearableItemBase EquipItem(Creature wearer);

		/// <summary>
		/// Remove this item. this cannot fail. update any perks, reactions, and/or status effects as necessary.
		/// </summary>
		/// <param name="wearer">The creature that is removing this item</param>
		protected virtual void OnRemove(Creature wearer) { }

		public override byte maxCapacityPerSlot => 1;

		public override bool CanUse(Creature creature)
		{
			return CanWearWithBodyData(creature);
		}

		public override void AttemptToUse(Creature target, UseItemCallback useItem)
		{
			if (!CanUse(target))
			{
				useItem(false, null);
			}

			var retVal = EquipItem(target);
			useItem(true, retVal);
		}
	}
}
