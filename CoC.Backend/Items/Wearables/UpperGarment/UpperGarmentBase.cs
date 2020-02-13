using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;

namespace CoC.Backend.Items.Wearables.UpperGarment
{
	public abstract class UpperGarmentBase : WearableItemBase<UpperGarmentBase>
	{
		protected UpperGarmentBase() : base()
		{ }

		protected override UpperGarmentBase EquipItem(Creature wearer, out string equipOutput)
		{
			return EquipUpperGarment(wearer, out equipOutput);
		}

		//changing upper garments is not exposed publicly, so we potentially could run into issues if EquipItem is ever overridden, and then potentially overridden again.
		//this ensures that regardless of how crazy we go with inheritance, we can still do the bare necessities to change an upper garment and go from there.
		protected UpperGarmentBase EquipUpperGarment(Creature wearer, out string equipOutput)
		{
			var retVal = wearer.ChangeUpperGarment(this, out string removeText);
			OnEquip(wearer);
			equipOutput = EquipText(wearer) + removeText;
			return retVal;
		}

		protected virtual void OnEquip(Creature wearer) { }
		protected abstract string EquipText(Creature wearer);

		public override bool CanUse(Creature creature, bool isInCombat, out string whyNot)
		{
			if (!base.CanUse(creature, isInCombat, out whyNot))
			{
				return false;
			}
			if (creature.armor?.CanWearWithUpperGarment(this, out whyNot) == false)
			{
				return false;
			}
			whyNot = null;
			return true;
		}
	}
}
