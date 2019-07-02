//Gold.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 10:24 PM
using CoC.Backend;
using CoC.Backend.Items.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Materials.Jewelry
{
	internal sealed partial class Gold : JewelryMaterial
	{
		public Gold() : base(GoldStr, GoldHue) {}
	}
}
