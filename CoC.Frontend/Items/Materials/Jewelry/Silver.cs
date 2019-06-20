using CoC.Backend;
using CoC.Backend.Items.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Materials.Jewelry
{
	internal sealed partial class Silver : JewelryMaterial
	{
		public Silver() : base(SilverStr, SilverHue) {}
	}
}
