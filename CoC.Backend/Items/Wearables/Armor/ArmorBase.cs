using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.Armor
{
	public abstract class ArmorBase : WearableItemBase<ArmorBase>
	{
		protected ArmorBase(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		//only called if can use returns true.

		protected override ArmorBase EquipItem(Creature wearer, out string equipOutput)
		{
			var retVal = wearer.ReplaceArmorInternal(this);
			OnEquip(wearer);
			equipOutput = EquipText(wearer);
			return retVal;
		}

		protected virtual void OnEquip(Creature wearer) { }
		protected abstract string EquipText(Creature wearer);

		public abstract bool CanWearWithUpperGarment(UpperGarmentBase currentUpperGarment);

		public abstract bool CanWearWithLowerGarment(LowerGarmentBase currentLowerGarment);

		public override bool CanUse(Creature creature)
		{
			return CanWearWithBodyData(creature) && CanWearWithLowerGarment(creature.lowerGarment) && CanWearWithUpperGarment(creature.upperGarment);
		}

		public abstract bool supportsBulgeArmor { get; }
	}
}
