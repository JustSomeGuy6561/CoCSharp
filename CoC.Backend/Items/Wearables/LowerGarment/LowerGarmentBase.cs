using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.LowerGarment
{
	public abstract class LowerGarmentBase : WearableItemBase<LowerGarmentBase>
	{
		protected LowerGarmentBase() : base()
		{}

		//by default, we assume that
		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			if (creature.isBiped || creature.lowerBody.type == LowerBodyType.GOO)
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = GenericRequireBipedText(creature);
				return false;
			}
		}

		protected override LowerGarmentBase EquipItem(Creature wearer, out string equipOutput)
		{
			return EquipLowerGarment(wearer, out equipOutput);
		}

		protected internal override void OnRemove(Creature wearer)
		{
			base.OnRemove(wearer);
		}

		protected string GenericRequireBipedText(Creature creature)
		{
			throw new NotImplementedException();
		}



		//changing lower garments is not exposed publicly, so we potentially could run into issues if EquipItem is ever overridden, and then potentially overridden again.
		//this ensures that regardless of how crazy we go with inheritance, we can still do the bare necessities to change a lower garment and go from there.
		protected LowerGarmentBase EquipLowerGarment(Creature wearer, out string equipOutput)
		{
			var retVal = wearer.ChangeLowerGarment(this, out string removeText);
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
			if (creature.armor?.CanWearWithLowerGarment(this, out whyNot) == false)
			{
				return false;
			}
			whyNot = null;
			return true;
		}
	}
}
