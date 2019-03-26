using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Wearables
{
	public abstract class PiercingJewelry
	{
		public readonly bool isSeamless;

		protected PiercingJewelry(bool seamless)
		{
			isSeamless = seamless;
		}
	}
}
