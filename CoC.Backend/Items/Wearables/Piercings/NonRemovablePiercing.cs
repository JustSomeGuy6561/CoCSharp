using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Items.Materials;

namespace CoC.Backend.Items.Wearables.Piercings
{
	public class NonRemovablePiercing : PiercingJewelry
	{
		public NonRemovablePiercing(JewelryType jewelryType, JewelryMaterial jewelryMaterial) : base(jewelryType, jewelryMaterial, false) { }
	}
}
