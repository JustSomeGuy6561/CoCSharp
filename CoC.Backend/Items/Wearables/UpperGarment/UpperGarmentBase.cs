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

		protected override bool CanWearWithBodyData(Creature creature)
		{
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

		public override bool CanUse(Creature creature)
		{
			return base.CanUse(creature) && creature.armor?.CanWearWithUpperGarment(this) == true;
		}
	}
}
