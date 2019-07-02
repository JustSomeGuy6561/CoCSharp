//Fertite.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 11:22 AM
using CoC.Backend;
using CoC.Backend.Items.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Materials.Jewelry
{
	internal sealed partial class Fertite : JewelryMaterial
	{
		public Fertite() : base(FertiteStr, FertiteHue) {}
	}
}
