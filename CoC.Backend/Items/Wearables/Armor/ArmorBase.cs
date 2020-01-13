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
		protected ArmorBase(SimpleDescriptor abbreviate, SimpleDescriptor itemName, SimpleDescriptor shortDesc, SimpleDescriptor appearance)
			: base(abbreviate, itemName, shortDesc, appearance)
		{}

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

		public abstract bool CanWearWithLowerGarment(LowerGarmentBase lowerGarment, out string whyNot);

		public abstract bool CanWearWithUpperGarment(UpperGarmentBase upperGarment, out string whyNot);

		//by default, body data is given priority, and if it fails the why not is just for that. then, if that passes, we check the garments and return them.
		//if you prefer more control, override this.
		public override bool CanUse(Creature creature, out string whyNot)
		{
			if (!CanWearWithBodyData(creature, out whyNot))
			{
				return false;
			}
			else
			{
				bool result = true;
				whyNot = "";

				if (!CanWearWithUpperGarment(creature.upperGarment, out string outText))
				{
					whyNot = outText;
					result = false;
				}
				if (!CanWearWithLowerGarment(creature.lowerGarment, out outText))
				{
					whyNot += outText;
					result = false;
				}

				return result;
			}
		}

		public abstract bool supportsBulgeArmor { get; }
	}
}
