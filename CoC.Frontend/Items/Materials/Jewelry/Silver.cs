//Silver.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 8:09 AM
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
