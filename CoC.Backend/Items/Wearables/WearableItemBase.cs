using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables
{
	public abstract class WearableItemBase : CapacityItem
	{

		protected abstract bool CanWearWithBodyData(Creature creature);

		protected virtual void OnEquip(Creature wearer) { }

		protected virtual void OnRemove(Creature wearer) { }

		public override byte maxCapacityPerSlot => 1;
	}
}
