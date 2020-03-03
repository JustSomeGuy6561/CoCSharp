//GenitalHelpers.cs
//Description:
//Author: JustSomeGuy
//4/15/2019, 9:13 PM

using System;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using WeakEvent;

//most of these are simply bytes, though a few do have extra behavior. An common software engineering practice is to never use primitives directly - this can be
//confusing or arbitrary - 5 could mean 5 years, 5 decades, 5 score, 5 centuries, etc. While i don't agree with that assessment 100%, it sometimes has merit.

//i'm not 100% familiar with C#'s optimizations, though it may align objects to word (4 byte) boundaries, which would mean these could all use ints instead, but w/e.
//though i suppose with even remotely modern hardware (read: it runs windows XP+) this game will never require memory sufficiently large enough to be an issue.
//Honestly, if this thing costs more than a few mbs (if that) i'll be very surprised.
namespace CoC.Backend.BodyParts
{
	[Flags]
	public enum Gender : byte { GENDERLESS = 0, MALE = 1, FEMALE = 2, HERM = MALE | FEMALE }

	public enum LactationStatus { NOT_LACTATING, LIGHT, MODERATE, STRONG, HEAVY, EPIC }

	public static class GenitalHelpers
	{
		public static bool CanHaveSexWith(this Gender source, Gender other)
		{
			return source != Gender.GENDERLESS && other != Gender.GENDERLESS && (source | other) == Gender.HERM;
		}

		//Genderless can also be used if gender is unimportant.
		public static string AsPronoun(this Gender gender)
		{
			switch (gender)
			{
				case Gender.HERM:
				case Gender.FEMALE:
					return "her";
				case Gender.MALE:
					return "his";
				case Gender.GENDERLESS:
				default:
					return "its";
			}
		}

		public static string AsText(this Gender gender)
		{
			switch (gender)
			{
				case Gender.HERM:
					return "herm";
				case Gender.MALE:
					return "male";
				case Gender.FEMALE:
					return "female";
				case Gender.GENDERLESS:
				default:
					return "genderless";
			}
		}


		public static double MinThreshold(this LactationStatus lactationStatus)
		{
			switch (lactationStatus)
			{
				case LactationStatus.EPIC:
					return Genitals.EPIC_LACTATION_THRESHOLD;
				case LactationStatus.HEAVY:
					return Genitals.HEAVY_LACTATION_THRESHOLD;
				case LactationStatus.STRONG:
					return Genitals.STRONG_LACTATION_THRESHOLD;
				case LactationStatus.MODERATE:
					return Genitals.MODERATE_LACTATION_THRESHOLD;
				case LactationStatus.LIGHT:
					return Genitals.LACTATION_THRESHOLD;
				case LactationStatus.NOT_LACTATING:
				default:
					return 0;
			}
		}

		public static PiercingJewelry GenerateCockJewelry(this Cock cock, CockPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (cock.piercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}

		public static PiercingJewelry GenerateClitJewelry(this Clit clit, ClitPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (clit.piercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}

		public static PiercingJewelry GenerateLabiaJewelry(this Vagina vagina, LabiaPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (vagina.labiaPiercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}

		public static PiercingJewelry GenerateNippleJewelry(this Breasts breasts, NipplePiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (breasts.nipplePiercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}
	}
}
