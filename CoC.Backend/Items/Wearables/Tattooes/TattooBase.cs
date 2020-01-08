using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.Tattoos
{
	public enum TattooSize { SMALL, MEDIUM, LARGE, FULL}

	public abstract class GenericTattooBase
	{
		public readonly bool scaleable;

		public readonly TattooSize tattooSize;

		public GenericTattooBase(TattooSize size, bool scalesUp)
		{
			tattooSize = size;
			scaleable = scalesUp;
		}


		public abstract string ShortDescription(bool alternateFormat);
		public abstract string LongDescription(bool alternateFormat);

		//for generic tattoos that will work anywhere, this is fine. be sure to override this to only allow the target type (say, arms or whatever) for tattoos that only
		//work on one body part. it's always possible to derive this class with another abstract class (using arms, we might call it "ArmSleeves") that defines a group of tattoos
		//that only work for one part that all follow the same ruleset.
		public virtual bool CanTattooOn<T>(TattooablePart<T> source) where T:Enum
		{
			//for type specific:
			//return T is ArmTattoos;
			return true;
		}
	}
}
