//PiercingJewelry.cs
//Description:
//Author: JustSomeGuy
//4/11/2019, 9:22 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Materials;
using System;

namespace CoC.Backend.Items.Wearables.Piercings
{
	public enum JewelryType { BARBELL_STUD, RING, DANGLER, HOOP, HORSESHOE, /*CHAIN,*/ SPECIAL}

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
			if (!Enum.IsDefined(typeof(JewelryType), jewelryType)) throw new ArgumentException("A single piece of piercing jewelry can only have one, valid jewelry type.");
			this.jewelryType = jewelryType;
		}

		public bool Equals(PiercingJewelry other)
		{
			return other != null && removable == other.removable && jewelryMaterial == other.jewelryMaterial && jewelryType == other.jewelryType;
		}

		//This allows you to limit a piece of jewelry to specific piercing locations - i.e. a nipple chain would only make sense in nipple piercings. Note this does not take into
		//account existing piercings or currently equipped jewelry, just the location.
		public virtual bool CanEquipAt<T, U>(T piercable) where T : Piercing<U> where U : Enum
		{
			return true;
		}

		public virtual bool CanEquipAtVersion2<T, U>(T piercable) where T : Piercing<U> where U : PiercingLocation
		{
			return true;
		}
	}
}
