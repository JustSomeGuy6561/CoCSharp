﻿using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.LowerGarment
{
	public abstract class LowerGarmentBase : WearableItemBase<LowerGarmentBase>
	{
		protected LowerGarmentBase(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		protected override LowerGarmentBase EquipItem(Creature wearer, out string equipOutput)
		{
			var retVal = wearer.ReplaceLowerGarmentInternal(this);
			OnEquip(wearer);
			equipOutput = EquipText(wearer);
			return retVal;
		}

		protected virtual void OnEquip(Creature wearer) { }
		protected abstract string EquipText(Creature wearer);

		public override bool CanUse(Creature creature)
		{
			return base.CanUse(creature) && creature.armor?.CanWearWithLowerGarment(this) == true;
		}
	}
}
