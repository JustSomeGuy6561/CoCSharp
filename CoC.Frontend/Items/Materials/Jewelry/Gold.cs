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
