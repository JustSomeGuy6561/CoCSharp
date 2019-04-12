using CoC.Backend.Items.Materials;
using System;

namespace CoC.Backend.Items.Wearables.Piercings
{
	[Flags]
	public enum JewelryType { STUD = 1, RING = 2, DANGLER = 4, HOOP = 8, CURVED_BARBELL = 16, SPECIAL = 32 }

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
