//Industrial.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 10:08 PM
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Strings.Items.Wearables.Piercings
{
	public sealed class Industrial : PiercingJewelry
	{
		public Industrial(JewelryMaterial jewelryMaterial) : base(JewelryType.SPECIAL, jewelryMaterial, false) {}
	}
}
