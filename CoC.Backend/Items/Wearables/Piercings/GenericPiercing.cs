//JewelryDummyForDebugging.cs
//Description:
//Author: JustSomeGuy
//4/11/2019, 9:22 PM

using CoC.Backend.Items.Materials;

namespace CoC.Backend.Items.Wearables.Piercings
{
	public sealed class GenericPiercing : PiercingJewelry
	{
		public GenericPiercing(JewelryType jewelryType, JewelryMaterial material) : base(jewelryType, material, true) { }
	}
}
