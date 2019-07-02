//Steel.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 2:19 PM
using CoC.Backend;
using CoC.Backend.Items.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Materials.Jewelry
{
	internal sealed partial class Steel : JewelryMaterial
	{
		public Steel() : base(SteelStr, SteelHue) {}
	}
}
