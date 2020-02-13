//Handcuffs.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 7:55 AM
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Frontend.Items.Materials.Jewelry;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Wearables.Piercings
{
	internal sealed class Handcuffs : PiercingJewelry
	{
		public Handcuffs() : base(JewelryType.SPECIAL, new Emerald(), true) {}
	}
}
