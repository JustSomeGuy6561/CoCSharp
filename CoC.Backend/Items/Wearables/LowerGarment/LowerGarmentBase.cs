using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.LowerGarment
{
	public abstract class LowerGarmentBase : WearableItemBase
	{
		protected LowerGarmentBase(SimpleDescriptor shortName, SimpleDescriptor fullName) : base(shortName, fullName)
		{
		}
	}
}
