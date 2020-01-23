using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.LowerGarment
{
	public abstract class LowerGarmentBase : WearableItemBase<LowerGarmentBase>
	{
		protected LowerGarmentBase() : base()
		{}

		protected override LowerGarmentBase EquipItem(Creature wearer, out string equipOutput)
		{
			var retVal = wearer.ReplaceLowerGarmentInternal(this);
			OnEquip(wearer);
			equipOutput = EquipText(wearer);
			return retVal;
		}

		protected virtual void OnEquip(Creature wearer) { }
		protected abstract string EquipText(Creature wearer);

		public override bool CanUse(Creature creature, out string whyNot)
		{
			if (!base.CanUse(creature, out whyNot))
			{
				return false;
			}
			if (creature.armor?.CanWearWithLowerGarment(this, out whyNot) == false)
			{
				return false;
			}
			whyNot = null;
			return true;
		}
	}
}
