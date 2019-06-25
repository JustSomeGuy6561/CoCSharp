﻿//Epidermis.cs
//Description: Epidermis Sub-part. it is used in pther parts to determine their tone, fur color, etc.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.EpidermalColors;
using CoC.Strings;
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
	internal enum SkinTexture { NONDESCRIPT, SHINY, SOFT, SMOOTH, SEXY, ROUGH, THICK, FRECKLED}
	internal enum FurTexture { NONDESCRIPT, SHINY, SOFT, SMOOTH, MANGEY }

	internal partial class Epidermis : SimpleBodyPart<EpidermisType>
	{
		public FurColor fur { get; protected set; }
		public Tones tone { get; protected set; }

		protected SkinTexture skinTexture;
		protected FurTexture furTexture;

		public bool usesFur => type.usesFur;
		public bool usesTone => type.usesTone;

		public override EpidermisType type { get; protected set; }

		public EpidermalData GetEpidermalData()
		{
			if (type.usesFur)
			{
				return new EpidermalData(type, fur, furTexture);
			}
			else return new EpidermalData(type, tone, skinTexture);
			
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

		public void Reset()
		{
			type = EpidermisType.EMPTY;
			fur.Reset();
			tone = Tones.NOT_APPLICABLE;
			furTexture = FurTexture.NONDESCRIPT;
			skinTexture = SkinTexture.NONDESCRIPT;
		}

		public bool UpdateEpidermis(EpidermisType epidermisType, bool resetOther = false)
		{
			if (type == epidermisType)
			{
				return false;
			}
			type = epidermisType;
			if (epidermisType is ToneBasedEpidermisType && tone == Tones.NOT_APPLICABLE)
			{
				tone = ((ToneBasedEpidermisType)type).defaultTone;
				if (resetOther)
				{
					fur.Reset();
				}
			}
			else if (epidermisType is FurBasedEpidermisType && fur.isNoFur())
			{
				fur.UpdateFurColor(((FurBasedEpidermisType)type).defaultFur);
				if (resetOther)
				{
					tone = Tones.NOT_APPLICABLE;
				}
			}
			return true;
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurColor overrideColor, bool resetTone = false)
		{
			return UpdateEpidermis(furType, overrideColor, furTexture, resetTone);
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, Tones overrideTone, bool resetFur = false)
		{
			return UpdateEpidermis(toneType, overrideTone, skinTexture, resetFur);
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurTexture texture, bool resetTone = false)
		{
			return UpdateEpidermis(furType, fur, texture, resetTone);
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, SkinTexture texture, bool resetFur = false)
		{
			return UpdateEpidermis(toneType, tone, texture, resetFur);
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurColor overrideColor, FurTexture texture, bool resetTone = false)
		{
			if (type == furType)
			{
				return false;
			}
			type = furType;
			if (!overrideColor.isNoFur())
			{
				fur.UpdateFurColor(overrideColor);
			}
			else if (fur.isNoFur())
			{
				fur.UpdateFurColor(furType.defaultFur);
			}
			if (resetTone)
			{
				tone = Tones.NOT_APPLICABLE;
			}
			furTexture = texture;
			return true;
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, Tones overrideTone, SkinTexture texture, bool resetFur = false)
		{
			if (type == toneType)
			{
				return false;
			}
			type = toneType;
			if (overrideTone != Tones.NOT_APPLICABLE)
			{
				tone = overrideTone;
			}
			else if (tone == Tones.NOT_APPLICABLE)
			{
				tone = toneType.defaultTone;
			}
			if (resetFur)
			{
				fur.Reset();
			}
			skinTexture = texture;
			return true;
		}

		public bool ChangeTone(Tones newTone, bool resetFur = false)
		{
			if (resetFur)
			{
				fur.Reset();
			}
			if (type.toneMutable && tone != newTone && newTone != Tones.NOT_APPLICABLE)
			{
				tone = newTone;
				return true;
			}
			return false;
		}

		public bool ChangeFur(FurColor furColor, bool resetTone = false)
		{
			if (resetTone)
			{
				tone = Tones.NOT_APPLICABLE;
			}
			if (fur != furColor && type.hairMutable && !furColor.isNoFur())
			{
				fur.UpdateFurColor(furColor);
				return true;
			}
			return false;
		}

		//Useful Helpers. Update if different, change if same. I'm not overly fond of the idea as the behavior is not identical in all instances, but
		//considering how often the if/else check would be used this makes more sense. use these only if you are truly doing it - if you know the type is 
		//correct, then just call change.
		public bool UpdateOrChange(FurBasedEpidermisType furType, FurColor overrideColor, bool resetTone = false)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, overrideColor, resetTone);
			}
			else return ChangeFur(overrideColor, resetTone);
		}

		public bool UpdateOrChange(ToneBasedEpidermisType toneType, Tones overrideColor, bool resetFur = false)
		{
			if (toneType != type)
			{
				return UpdateEpidermis(toneType, overrideColor, resetFur);
			}
			else return ChangeTone(overrideColor, resetFur);
		}
		public bool UpdateOrChange(FurBasedEpidermisType furType, FurTexture texture, bool resetTone = false)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, texture, resetTone);
			}
			else return ChangeFur(furType.defaultFur, resetTone);
		}

		public bool UpdateOrChange(ToneBasedEpidermisType toneType, Tones overrideColor, SkinTexture texture, bool resetFur = false)
		{
			if (toneType != type)
			{
				return UpdateEpidermis(toneType, overrideColor, texture, resetFur);
			}
			else return ChangeTone(overrideColor, resetFur);
		}
		public bool UpdateOrChange(FurBasedEpidermisType furType, FurColor overrideColor, FurTexture texture, bool resetTone = false)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, overrideColor, texture, resetTone);
			}
			else return ChangeFur(overrideColor, resetTone);
		}

		public bool UpdateOrChange(ToneBasedEpidermisType toneType, Tones overrideColor, SkinTexture texture, bool resetFur = false)
		{
			if (toneType != type)
			{
				return UpdateEpidermis(toneType, overrideColor, texture, resetFur);
			}
			else return ChangeTone(overrideColor, resetFur);
		}
	}


	//IMMUTABLE
	internal abstract partial class EpidermisType : SimpleBodyPartType
	{
		private static int indexMaker = 0;

		public abstract bool usesTone { get; }
		public virtual bool usesFur => !usesTone;

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
		public static readonly EmptyEpidermisType EMPTY = new EmptyEpidermisType();
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

	internal class EmptyEpidermisType : EpidermisType
	{
		public EmptyEpidermisType() : base(GlobalStrings.None, false) {}

		public override bool usesTone => false;
		public override bool usesFur => false;

	}

	internal partial class EpidermalData
	{

		public EpidermisType epidermisType { get; private set; }
		private readonly FurColor _fur;
		private readonly Tones _tone;

		private SkinTexture _skinTexture;
		private FurTexture _furTexture;

		public EpidermalData(EpidermisType type, FurColor furColor, FurTexture texture)
		{
			epidermisType = type;
			_fur = FurColor.GenerateFromOther(furColor);
			_tone = Tones.NOT_APPLICABLE;
		}

		public EpidermalData(EpidermisType type, Tones tones, SkinTexture texture)
		{
			epidermisType = type;
			_fur = FurColor.GenerateEmpty();
			_tone = tones;
		}

		public bool usesFur => epidermisType.usesFur;
		public bool usesTone => epidermisType.usesTone;

		public FurColor fur => epidermisType.usesFur ? _fur : FurColor.GenerateEmpty();
		public Tones tone => epidermisType.usesTone ? _tone : Tones.NOT_APPLICABLE;

		public SkinTexture skinTexture => epidermisType.usesTone ? _skinTexture : SkinTexture.NONDESCRIPT;
		public FurTexture furTexture => epidermisType.usesTone ? _furTexture : FurTexture.NONDESCRIPT;

		public string shortDescription()
		{
			return epidermisType.shortDescription();
		}
		public string FullDescription()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return fullStr(skinTexture.AsString(), tone, epidermisType.shortDescription);
			else return fullStr(furTexture.AsString(), fur, epidermisType.shortDescription);
		}

		public string descriptionWithColor()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return ColoredStr(tone, epidermisType.shortDescription);
			else return ColoredStr(fur, epidermisType.shortDescription);
		}

		public string justTexture()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return skinTexture.AsString();
			else return furTexture.AsString();
		}

		public string justColor()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return tone.AsString();
			else return fur.AsString();
		}
	}
}