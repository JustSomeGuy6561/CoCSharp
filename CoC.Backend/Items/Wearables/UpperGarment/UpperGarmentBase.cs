using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Items.Wearables.UpperGarment
{
	public abstract class UpperGarmentBase : WearableItemBase
	{
		protected UpperGarmentBase(SimpleDescriptor shortName, SimpleDescriptor fullName) : base(shortName, fullName)
		{
		}

		protected override bool CanWearWithBodyData(Creature creature)
		{
			return true;
		}
	}
}
