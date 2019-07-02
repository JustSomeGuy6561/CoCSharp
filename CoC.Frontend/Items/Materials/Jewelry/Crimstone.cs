//Crimstone.cs
//Description:
//Author: JustSomeGuy
//6/19/2019, 12:57 AM
using CoC.Backend;
using CoC.Backend.Items.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Materials.Jewelry
{
	internal sealed partial class Crimstone : JewelryMaterial
	{
		public Crimstone() : base(CrimstoneStr, CrimstoneHue) {}
	}
}
