using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Frontend.Items.Materials.Jewelry;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Strings.Items.Wearables.Piercings
{
	internal sealed class Handcuffs : PiercingJewelry
	{
		public Handcuffs() : base(JewelryType.SPECIAL, new Emerald(), false) {}
	}
}
