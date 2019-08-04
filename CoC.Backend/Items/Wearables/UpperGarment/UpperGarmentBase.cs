using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Items.Wearables.UpperGarment
{
	public abstract class UpperGarmentBase : WearableItemBase
	{
		protected override bool CanWearWithBodyData(Creature creature)
		{
			return true;
		}
	}
}
