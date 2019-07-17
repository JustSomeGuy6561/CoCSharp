//PiercingJewelry.cs
//Description:
//Author: JustSomeGuy
//4/11/2019, 9:22 PM
using CoC.Backend.Items.Materials;
using System;

namespace CoC.Backend.Items.Wearables.Piercings
{
	//to whom it may concern: I know a "horseshoe" is a "curved barbell" and that may annoy certain people. 
	//but from a descriptive/simple standpoint we treat studs and straight barbells identically. Additionally, barbells that are curved due to the anatomy
	//but otherwise appear straight (vch, navel piercings, prince albert, etc) as straight barbells, event though they are actually curved barbells. 
	//To avoid confusion between slightly curved barbells and really curved barbells, i'm using "horseshoe."
	[Flags]
	public enum JewelryType { BARBELL_STUD = 1, RING = 2, DANGLER = 4, HOOP = 8, HORSESHOE = 16, /*CHAIN = 32, SPECIAL = 64*/ SPECIAL = 32 }

	public class PiercingJewelry : IEquatable<PiercingJewelry>
	{
#warning may want to make jewelry material IEquatable and use .Equals in our Equals method.
		public readonly JewelryType jewelryType;
		public readonly JewelryMaterial jewelryMaterial;

		//non-removable jewelry can only be removed via a piercing expert (Ceraph, Yara). Of course, if a body part ceases to exist, the jewelry is no longer in it.
		public readonly bool removable;

		public PiercingJewelry(JewelryType jewelryType, JewelryMaterial jewelryMaterial, bool canRemove)
		{
			removable = canRemove;
			this.jewelryMaterial = jewelryMaterial ?? throw new ArgumentNullException(nameof(jewelryMaterial));
			if (!Enum.IsDefined(typeof(JewelryType), jewelryType)) throw new ArgumentException("A single pierce of piercing jewelry can only have one, valid jewelry type.");
			this.jewelryType = jewelryType;
		}

		public bool Equals(PiercingJewelry other)
		{
			return other != null && removable == other.removable && jewelryMaterial == other.jewelryMaterial && jewelryType == other.jewelryType;
		}
	}
}
