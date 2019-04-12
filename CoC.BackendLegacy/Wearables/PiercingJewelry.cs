using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Wearables
{
	[Flags]
	public enum JewelryType { STUD = 1, RING = 2, DANGLER = 4, HOOP = 8, CURVED_BARBELL = 16, SPECIAL = 32
	}
	[DataContract]
	[KnownType (typeof(JewelryMaterial))]
	public class PiercingJewelry
	{
		[DataMember]
		public readonly JewelryType jewelryType;
		[DataMember]
		public readonly JewelryMaterial jewelryMaterial;
		[DataMember]
		public readonly bool isSeamless;

		public PiercingJewelry(JewelryType jewelryType, JewelryMaterial jewelryMaterial, bool seamless)
		{
			isSeamless = seamless;
		}
	}
}
