//EpidermisType.cs
//Description: EpidermisType Sub-Body Part class. used in other body parts. 
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Tools;
using CoC.EpidermalColors;
using static CoC.Strings.BodyParts.EpidermisString;

namespace CoC.BodyParts
{
	//Epidermis represents the equivalent to skin on all species.

	/*
	 * NOTES: Epidermis is a simple part. That means it should not be attached to a creature directly
	 * instead, it should be part of another. it can either use skin tone, or fur color, not both.
	 * it will store both values passed into it, but if you query for one and it's not in use, it will
	 * return not applicable/no fur. 
	 */
	public class Epidermis : SimpleBodyPart<EpidermisType>
	{
		protected FurColor fur;
		protected Tones tone;
		public override EpidermisType type { get; protected set; }

		public EpidermalData GetEpidermalData()
		{
			return new EpidermalData(type, fur, tone);
		}

		public string epidermisAdjective;

		protected Epidermis(EpidermisType type) : base(type)
		{
			fur = FurColor.GenerateEmpty();
			tone = Tones.NOT_APPLICABLE;
			epidermisAdjective = "";
		}

		public static Epidermis Generate(EpidermisType type, Tones initialTone, FurColor furColor)
		{
			Epidermis retVal = new Epidermis(type);
			retVal.fur.UpdateFurColor(furColor);
			retVal.tone = initialTone;
			return retVal;
		}

		public bool UpdateEpidermis(EpidermisType newType, Tones currentTone, FurColor currentFur, string adjective = "")
		{
			if (newType == type)
			{
				return false;
			}
			type = newType;
			tone = currentTone;
			fur = currentFur;
			epidermisAdjective = adjective;
			return true;

		}

		public virtual string FullDescription()
		{
			if (type.usesTone) return fullStr(epidermisAdjective, tone, shortDescription);
			else return fullStr(epidermisAdjective, fur, shortDescription);
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
	public class EpidermisType : SimpleBodyPartType
	{
		private static int indexMaker = 0;

		public readonly bool usesTone;
		public bool usesFur => !usesTone;

		protected readonly bool updateable;
		protected readonly int _index;

		public bool hairMutable => usesFur && updateable;
		public bool toneMutable => usesTone && updateable;

		protected EpidermisType(SimpleDescriptor desc, bool isTone, bool canChange) : base(desc)
		{
			_index = indexMaker++;
			usesTone = isTone;
			updateable = canChange;
		}
		public override int index => _index;

		public static readonly EpidermisType SKIN = new EpidermisType(SkinStr, true, true);
		public static readonly EpidermisType FUR = new EpidermisType(FurStr, false, true);
		public static readonly EpidermisType SCALES = new EpidermisType(ScalesStr, true, true);
		public static readonly EpidermisType GOO = new EpidermisType(GooStr, true, true);
		public static readonly EpidermisType WOOL = new EpidermisType(WoolStr, false, true); //i'd like to merge this with fur but it's more trouble than it's worth
		public static readonly EpidermisType FEATHERS = new EpidermisType(FeathersStr, false, true);
		public static readonly EpidermisType BARK = new EpidermisType(BarkStr, true, true); //do you want the bark to change colors? idk? maybe make that false.
		public static readonly EpidermisType CARAPACE = new EpidermisType(CarapaceStr, true, true);
		//cannot be changed by lotion.
		public static readonly EpidermisType EXOSKELETON = new EpidermisType(ExoskeletonStr, true, false);
		public static readonly EpidermisType RUBBER = new EpidermisType(RubberStr, true, false); //now its own type. it's simpler this way imo.

	}

	public class EpidermalData
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
