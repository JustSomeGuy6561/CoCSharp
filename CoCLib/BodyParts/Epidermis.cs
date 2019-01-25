//EpidermisType.cs
//Description: EpidermisType Sub-Body Part class. used in other body parts. 
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.BodyParts
{
	//Epidermis represents the equivalent to skin on all species.

	/*
	 * Redesign: Tone or Fur EpidermisType, custom updates for both. a default is to be provided, with an optional bool to force override
	 * This makes it so even if the current fur or tone is empty, a valid value will exist. further, it allows an implementation to override
	 * the current color if they so desire.
	 */

	/*
	 * NOTES: Epidermis is a simple part. That means it should not be attached to a creature directly
	 * instead, it should be part of another. it can either use skin tone, or fur color, not both.
	 * it will store both values passed into it, but if you query for one and it's not in use, it will
	 * return not applicable/no fur. 
	 */

	//feel free to add more of these. i just did these because they were there, and i didn't want to use a string.
	internal enum SkinTexture { NONDESCRIPT, SOFT, SMOOTH, SEXY, ROUGH, THICK, FRECKLED}
	internal enum FurTexture { NONDESCRIPT, SHINY, SOFT, SMOOTH, MANGEY }

	internal partial class Epidermis : SimpleBodyPart<EpidermisType>
	{
		protected FurColor fur;
		protected Tones tone;

		protected SkinTexture skinTexture;
		protected FurTexture furTexture;
		public override EpidermisType type { get; protected set; }

		public EpidermalData GetEpidermalData()
		{
			return new EpidermalData(type, fur, tone);
		}


		protected Epidermis(EpidermisType type) : base(type)
		{
			fur = FurColor.GenerateEmpty();
			tone = Tones.NOT_APPLICABLE;
		}

		public static Epidermis GenerateDefault(EpidermisType epidermisType)
		{
			Epidermis retVal = new Epidermis(epidermisType);
			if (epidermisType is ToneBasedEpidermisType)
			{
				retVal.tone = ((ToneBasedEpidermisType)epidermisType).defaultTone;
			}
			else if (epidermisType is FurBasedEpidermisType)
			{
				retVal.fur.UpdateFurColor(((FurBasedEpidermisType)epidermisType).defaultFur);
			}
			return retVal;
		}

		//provide with initial values. these should not be null/empty, as they are the absolute fallbacks.
		public static Epidermis Generate(ToneBasedEpidermisType toneType, Tones initialTone, SkinTexture texture = SkinTexture.NONDESCRIPT)
		{
			return new Epidermis(toneType)
			{
				tone = initialTone,
				skinTexture = texture
			};
		}
		public static Epidermis Generate(FurBasedEpidermisType type, FurColor furColor, FurTexture texture = FurTexture.NONDESCRIPT)
		{
			Epidermis retVal = new Epidermis(type)
			{
				furTexture = texture
			};
			retVal.fur.UpdateFurColor(furColor);
			return retVal;
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurColor fallbackColor, bool replaceCurrentColorWithFallback = false)
		{
			if (type == furType)
			{
				return false;
			}
			if (replaceCurrentColorWithFallback || fur.isNoFur())
			{
				fur.UpdateFurColor(fallbackColor);
			}
			return true;
		}

		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, Tones fallbackTone, bool replaceCurrentColorWithFallback = false)
		{
			if (type == toneType)
			{
				return false;
			}
			if (replaceCurrentColorWithFallback || tone == Tones.NOT_APPLICABLE)
			{
				tone = fallbackTone;
			}
			return true;
		}

		public virtual string FullDescription()
		{
			if (type.usesTone) return fullStr(skinTexture.AsString(), tone, shortDescription);
			else return fullStr(furTexture.AsString(), fur, shortDescription);
		}

		public virtual string descriptionWithColor()
		{
			if (type.usesTone) return ColoredStr(tone, shortDescription);
			else return ColoredStr(fur, shortDescription);
		}
		public virtual string justColor()
		{
			return type.usesTone ? tone.AsString() : fur.AsString();
		}

		public bool UpdateTone(Tones newTone)
		{
			if (type.toneMutable && tone != newTone)
			{
				tone = newTone;
				return true;
			}
			return false;
		}

		public bool UpdateFur(FurColor furColor)
		{
			if (fur != furColor && type.hairMutable)
			{
				fur.UpdateFurColor(furColor);
				return true;
			}
			return false;
		}
	}


	//IMMUTABLE
	internal abstract partial class EpidermisType : SimpleBodyPartType
	{
		private static int indexMaker = 0;

		public abstract bool usesTone { get; }
		public bool usesFur => !usesTone;

		protected readonly bool updateable;
		protected readonly int _index;

		public bool hairMutable => usesFur && updateable;
		public bool toneMutable => usesTone && updateable;

		protected EpidermisType(SimpleDescriptor desc, bool canChange) : base(desc)
		{
			_index = indexMaker++;
			updateable = canChange;
		}
		public override int index => _index;

		public static readonly ToneBasedEpidermisType SKIN = new ToneBasedEpidermisType(SkinStr, true, Tones.LIGHT);
		public static readonly FurBasedEpidermisType FUR = new FurBasedEpidermisType(FurStr, true, FurColor.Generate(HairFurColors.BLACK));
		public static readonly ToneBasedEpidermisType SCALES = new ToneBasedEpidermisType(ScalesStr, true, Tones.GREEN);
		public static readonly ToneBasedEpidermisType GOO = new ToneBasedEpidermisType(GooStr, true, Tones.DEEP_BLUE);
		public static readonly FurBasedEpidermisType WOOL = new FurBasedEpidermisType(WoolStr, true, FurColor.Generate(HairFurColors.WHITE)); //i'd like to merge this with fur but it's more trouble than it's worth
		public static readonly FurBasedEpidermisType FEATHERS = new FurBasedEpidermisType(FeathersStr, true, FurColor.Generate(HairFurColors.WHITE));
		public static readonly ToneBasedEpidermisType BARK = new ToneBasedEpidermisType(BarkStr, true, Tones.WOODLY_BROWN); //do you want the bark to change colors? idk? maybe make that false.
		public static readonly ToneBasedEpidermisType CARAPACE = new ToneBasedEpidermisType(CarapaceStr, true, Tones.BLACK);
		//cannot be changed by lotion. May convert this to a perk, which affects everything.
		public static readonly ToneBasedEpidermisType RUBBER = new ToneBasedEpidermisType(RubberStr, false, Tones.GRAY); //now its own type. it's simpler this way imo - for now. may become a perk. 

	}

	internal class FurBasedEpidermisType : EpidermisType
	{
		public FurColor defaultFur;
		public FurBasedEpidermisType(SimpleDescriptor desc, bool canChange, FurColor defaultColor) : base(desc, canChange)
		{
			defaultFur = FurColor.GenerateFromOther(defaultColor);
		}

		public override bool usesTone => false;
	}

	internal class ToneBasedEpidermisType : EpidermisType
	{
		public Tones defaultTone;
		public ToneBasedEpidermisType(SimpleDescriptor desc, bool canChange, Tones defaultColor) : base(desc, canChange)
		{
			defaultTone = defaultColor;
		}

		public override bool usesTone => true;
	}


	internal class EpidermalData
	{
		public EpidermisType epidermisType { get; private set; }
		private readonly FurColor _fur;
		private readonly Tones _tone;

		public EpidermalData(EpidermisType type, FurColor furColor, Tones tones)
		{
			epidermisType = type;
			_fur = FurColor.GenerateFromOther(furColor);
			_tone = tones;
		}

		public FurColor fur => (epidermisType.usesFur) ? _fur : FurColor.GenerateEmpty();
		public Tones tone => (epidermisType.usesTone) ? _tone : Tones.NOT_APPLICABLE;
	}
}
