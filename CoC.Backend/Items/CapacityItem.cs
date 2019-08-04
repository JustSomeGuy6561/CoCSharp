using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items
{
	public abstract class CapacityItem
	{
		public abstract byte maxCapacityPerSlot { get; }
	}
}
