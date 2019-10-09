using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.Armor
{
	public abstract class ArmorBase : WearableItemBase
	{
		protected ArmorBase(SimpleDescriptor shortName, SimpleDescriptor fullName) : base(shortName, fullName)
		{
		}

		public abstract bool CanWearWithUpperGarment(/*UpperGarmentBase currentUpperGarment*/);

		public abstract bool CanWearWithLowerGarment(/*LowerGarmentBase currentLowerGarment*/);

		public bool CanEquip(Creature creature)
		{
			return CanWearWithBodyData(creature) && CanWearWithLowerGarment(/*creature.lowerGarment*/) && CanWearWithUpperGarment(/*creature.upperGarment*/);
		}
	}
}
