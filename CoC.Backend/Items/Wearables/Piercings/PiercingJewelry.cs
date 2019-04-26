using CoC.Backend.Items.Materials;
using System;

namespace CoC.Backend.Items.Wearables.Piercings
{
	[Flags]
	public enum JewelryType { BARBELL_STUD = 1, RING = 2, DANGLER = 4, HOOP = 8, HORSESHOE = 16, /*CHAIN = 32, SPECIAL = 64*/ SPECIAL = 32 }

	public class PiercingJewelry
	{
		public readonly JewelryType jewelryType;
		public readonly JewelryMaterial jewelryMaterial;
		public readonly bool isSeamless;

		public PiercingJewelry(JewelryType jewelryType, JewelryMaterial jewelryMaterial, bool seamless)
		{
			isSeamless = seamless;
		}
	}
}
