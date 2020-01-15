using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables
{
	public abstract class WearableItemBase<T> : CapacityItem<T> where T:WearableItemBase<T>
	{
		protected WearableItemBase(SimpleDescriptor abbreviate, SimpleDescriptor itemName, SimpleDescriptor shortDesc, SimpleDescriptor appearance)
			: base(abbreviate, itemName, shortDesc, appearance)
		{}

		protected abstract bool CanWearWithBodyData(Creature creature, out string whyNot);

		/// <summary>
		/// Equip the item. At this point, CanUse has been called. It's assumed that this will always succeed at this point.
		/// Additionally, apply any perks, reactions, and/or status effects as necessary.
		/// </summary>
		/// <param name="wearer">The creature equipping this wearable item.</param>
		/// <returns>The wearable item this object replaces, or null if none exists.</returns>
		protected abstract T EquipItem(Creature wearer, out string equipOutput);

		protected override T UseItem(Creature target, out string resultsOfUseText)
		{
			return EquipItem(target, out resultsOfUseText);
		}

		/// <summary>
		/// Remove this item. this cannot fail. update any perks, reactions, and/or status effects as necessary.
		/// </summary>
		/// <param name="wearer">The creature that is removing this item</param>
		protected internal virtual void OnRemove(Creature wearer) { }

		protected internal virtual string OnRemoveText()
		{
			return "You remove your " + this.ItemName();
		}

		public override byte maxCapacityPerSlot => 1;

		public override bool CanUse(Creature creature, out string whyNot)
		{
			return CanWearWithBodyData(creature, out whyNot);
		}
	}
}
