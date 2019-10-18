using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Items.Wearables.UpperGarment
{
	public abstract class UpperGarmentBase : WearableItemBase<UpperGarmentBase>
	{
		protected UpperGarmentBase(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		//by default, upper garments seem to not care. undergarments do based on lower body type, but not these. hence this default. of course, if you need to limit it,
		//override this.
		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override UpperGarmentBase EquipItem(Creature wearer, out string equipOutput)
		{
			var retVal = wearer.ReplaceUpperGarmentInternal(this);
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
			if (creature.armor?.CanWearWithUpperGarment(this, out whyNot) == false)
			{
				return false;
			}
			whyNot = null;
			return true;
		}
	}
}
