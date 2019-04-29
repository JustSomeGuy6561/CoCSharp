using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Attacks;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using CoC.Backend.Attacks.BodyPartAttacks;

namespace CoC.Backend.BodyParts
{
	public enum HairStyle { NO_STYLE, MESSY, STRAIGHT, BRAIDED, WAVY, CURLY, COILED, PONYTAIL}


	//even when bald, we keep track of the hair color. just check if bald before saying they have (haircolor) hair.

	public sealed partial class Hair : BehavioralSaveablePart<Hair, HairType>, ISimultaneousMultiDyeable, ICanAttackWith// itimeaware
	{
		private static readonly HairFurColors DEFAULT_COLOR = HairFurColors.BLACK;

		//make sure to check this when deserializing - if you dont use the property is may cause errors.
		public HairFurColors hairColor
		{
			get => _hairColor;
			private set
			{
				if (!value.isEmpty)
				{
					_hairColor = value;
				}
			}
		}
		private HairFurColors _hairColor = DEFAULT_COLOR;
		public HairFurColors highlightColor { get; private set; } = HairFurColors.NO_HAIR_FUR;
		public bool isGrowing { get; private set; } = true;
		public bool isSemiTransparent { get; private set; } = false;



		//public HairStyles hairStyle {get; private set;}
		public float length { get; private set; } = 10;
		public override HairType type { get; protected set; }

		public HairStyle style { get; private set; }

		private Hair()
		{
			length = 0.0f;
			type = HairType.NO_HAIR;
			isGrowing = false;
			hairColor = type.defaultColor;
			highlightColor = HairFurColors.NO_HAIR_FUR;
			isSemiTransparent = false;
			style = HairStyle.NO_STYLE;
		}

		private Hair(HairType hairType)
		{
			type = hairType;
			length = type.defaultHairLength;
			isGrowing = type != HairType.NO_HAIR;
			hairColor = type.defaultColor;
			isSemiTransparent = false;
			style = HairStyle.NO_STYLE;
		}

		public override bool isDefault => type == HairType.NO_HAIR;

		internal static Hair GenerateDefault()
		{
			return new Hair();
		}

		internal static Hair GenerateDefaultOfType(HairType hairType)
		{
			return new Hair(hairType);
		}
		internal static Hair GenerateWithLength(HairType hairType, float hairLength)
		{
			return new Hair(hairType)
			{
				length = hairLength
			};
		}

		internal static Hair GenerateWithColor(HairType hairType, HairFurColors color, float? hairLength = null)
		{
			return new Hair(hairType)
			{
				length = hairLength ?? hairType.defaultHairLength,
				hairColor = color,
			};
		}

		internal static Hair GenerateWithColorAndHighlight(HairType hairType, HairFurColors color, HairFurColors highlight, float? hairLength = null)
		{
			return new Hair(hairType)
			{
				length = hairLength ?? hairType.defaultHairLength,
				hairColor = color,
				highlightColor = highlight,
			};
		}

		internal HairData ToHairData()
		{
			return new HairData(type, hairColor, highlightColor, style, length, isSemiTransparent, !isGrowing);
		}

		internal override bool Restore()
		{
			if (type == HairType.NO_HAIR)
			{
				return false;
			}
			type = HairType.NO_HAIR;
			return true;
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{

			HairType hairType = type;
			float len = length;
			bool growing = isGrowing;
			bool valid = HairType.Validate(ref hairType, ref len, ref growing, correctDataIfInvalid);
			isGrowing = growing;
			length = len;
			type = hairType;
			return valid;
		}

		byte IMultiDyeable.numDyeableMembers => 2;

		bool IMultiDyeable.allowsDye(byte index)
		{
			if (index >= numDyeMembers)
			{
				return false;
			}
			else if (index <= 1)
			{
				return type.canDye;
			}
			else
			{
				throw new NotImplementedException("Somebody added a new dyeable member and didn't implement it correctly.");
			}
		}

		bool IMultiDyeable.isDifferentColor(HairFurColors dyeColor, byte index)
		{
			if (index >= numDyeMembers)
			{
				return false;
			}
			else if (index == 1)
			{
				return dyeColor != highlightColor;
			}
			else if (index == 0)
			{
				return dyeColor != hairColor;
			}
			else
			{
				throw new NotImplementedException("Somebody added a new dyeable member and didn't implement it correctly.");
			}
		}

		bool IMultiDyeable.attemptToDye(HairFurColors dye, byte index)
		{
			IMultiDyeable instance = this;
			if (!instance.allowsDye(index) || !instance.isDifferentColor(dye, index))
			{
				return false;
			}
			else if (index == 1)
			{
				highlightColor = dye;
				return true;
			}
			else
			{
				return type.tryToDye(ref _hairColor, dye);
			}
		}

		string IMultiDyeable.buttonText(byte index)
		{
			if (index >= numDyeMembers)
			{
				return "";
			}
			else if (index == 1)
			{
				return HighlightStr();
			}
			else if (index == 0)
			{
				return HairStr();
			}
			else
			{
				throw new System.NotImplementedException("Hair's multidyeable was not fully implemented. Consider fixing this. ");
			}
		}

		string IMultiDyeable.locationDesc(byte index)
		{
			if (index >= numDyeMembers)
			{
				return "";
			}
			else if (index == 1)
			{
				return YourHighlightsStr();
			}
			else if (index == 0)
			{
				return YourHairStr();
			}
			else
			{
				throw new System.NotImplementedException("Hair's multidyeable was not fully implemented. Consider fixing this. ");
			}
		}

		private IMultiDyeable dyeable => this;
		private byte numDyeMembers => dyeable.numDyeableMembers;

		AttackBase ICanAttackWith.attack => type.attack;
		bool ICanAttackWith.canAttackWith() => type.canAttackWith(this);
	}

	//public class HairType : SaveableBehavior<HairType, Hair>
	//{
	//	private static int indexMaker = 0;
	//	private readonly int _index;
	//	private static readonly List<HairType> hairTypes = new List<HairType>();

	//	public HairType(float defaultLength, HairFurColors defaultColor, bool overrideColorOnChange,
	//		SimpleDescriptor shortDesc, DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc, 
	//		ChangeType<Hair> transform, RestoreType<Hair> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
	//	{
	//		_index = indexMaker++;
	//		hairTypes.AddAt(this, _index);
	//	}

	//	public override int index => _index;
	//}


	public partial class HairType : SaveableBehavior<HairType, Hair>
	{
		private static int indexMaker = 0;
		private static readonly List<HairType> hairTypes = new List<HairType>();
		public override int index => _index;
		private readonly int _index;

		public virtual bool preventHairGrowth => false;
		public virtual bool canDye => true;

		public virtual bool canCut => true;
		public virtual bool canLengthen => true;

		internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual bool canAttackWith(Hair hair) => attack != AttackBase.NO_ATTACK;

		public readonly HairFurColors defaultColor;
		public readonly float defaultHairLength;
		public readonly bool setLengthToDefaultOnChange;
		//Worth noting: As of vanilla, all hair (even basilisk plumes) can be dyed.
		//It can now be disabled, but default behavior is to allow it. to disable,
		//or to conditionally allow dyeing, implement a subclass of Hairtype and 
		//override canDye and possibly tryToDye with the new rules.

		private protected HairType(HairFurColors fallbackColor, float defaultLengthForBaldies, bool useDefaultLengthEvenIfNotBald,
			SimpleDescriptor shortDesc, DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc, PlayerStr growFromBaldStr,
			ChangeType<Hair> transform, RestoreType<Hair> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;

			defaultColor = fallbackColor;
			defaultHairLength = defaultLengthForBaldies;
			setLengthToDefaultOnChange = useDefaultLengthEvenIfNotBald;

			hairTypes.AddAt(this, index);
		}


		internal virtual bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
		{
			if (canDye)
			{
				currentColor = newColor;
				return true;
			}
			return false;
		}

		internal static bool Validate(ref HairType hairType, ref float length, ref bool growing, bool correctInvalidData = false)
		{
			if (!hairTypes.Contains(hairType))
			{
				if (correctInvalidData)
				{
					hairType = NORMAL;
					length = hairType.defaultHairLength;
					growing = !hairType.preventHairGrowth;
				}
				return false;
			}

			else if (hairType == NO_HAIR && (growing || length != 0f))
			{
				if (correctInvalidData)
				{
					length = 0f;
					growing = false;
				}
				return false;
			}

			else if (growing && hairType.preventHairGrowth)
			{
				if (correctInvalidData)
				{
					growing = false;
				}
				return false;
			}
			else return true;

		}

		//default l
		public static readonly HairType NO_HAIR = new NoHair(); //0.0
		public static readonly HairType NORMAL = new NormalHair();
		public static readonly HairType FEATHER = new HairType(HairFurColors.WHITE, 5.0f, false, FeatherDesc, FeatherFullDesc, FeatherPlayerStr, FeatherGrowStr, FeatherTransformStr, FeatherRestoreStr);
		public static readonly HairType GOO = new HairType(HairFurColors.CERULEAN, 5.0f, false, GooDesc, GooFullDesc, GooPlayerStr, GooGrowStr, GooTransformStr, GooRestoreStr); //5 in if bald. updating behavior to <5 or bald to 5 inch. just say your old type 
		public static readonly HairType ANEMONE = new AnemoneHair();
		public static readonly HairType QUILL = new HairType(HairFurColors.WHITE, 12.0f, true, QuillDesc, QuillFullDesc, QuillPlayerStr, QuillGrowStr, QuillTransformStr, QuillRestoreStr); //shoulder length. not set though. whoops.
																																															//Spines are a huge dick - they break the format that hair actually is hair (or hair-like)

		//Solution: Spines take the hair color of the original and convert it to the closest lizard tone
		//you can say something like "the spines clash with the rest of your scales, hinting at their unnatural origins"
		//then if you really want to, the next TF you can force the spines in line by dyeing them with a lizard related dye closest to their current tone. 
		//at that point you can then say "the spines 
		public static readonly HairType BASILISK_SPINES = new BasiliskSpines(); //2. Set regardless.
																				//end complain
		public static readonly HairType BASILISK_PLUME = new HairType(Species.BASILISK.defaultPlume, 2.0f, true, PlumeDesc, PlumeFullDesc, PlumePlayerStr, PlumeGrowStr, PlumeTransformStr, PlumeRestoreStr); //2
		public static readonly HairType WOOL = new HairType(HairFurColors.WHITE, 1.0f, false, WoolDesc, WoolFullDesc, WoolPlayerStr, WoolGrowStr, WoolTransformStr, WoolRestoreStr); //not defined. 
		public static readonly HairType LEAF = new Vines(); //not defined. not fully implemented imo.


		private class NoHair : HairType
		{
			public NoHair() : base(HairFurColors.BLACK, 0.0f, true, NoHairDesc, NoHairFullDesc, NoHairPlayerStr, NoHairGrowStr, NoHairTransformStr, NoHairRestoreStr) { }

			public override bool preventHairGrowth => true;
			public override bool canCut => false;
			public override bool canLengthen => false;
			public override bool canDye => false;

			internal override bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
			{
				return false;
			}

		}

		private class NormalHair : HairType
		{
			public NormalHair() : base(HairFurColors.BLACK, 0.0f, false, NormalDesc, NormalFullDesc, NormalPlayerStr, NormalGrowStr, NormalTransformStr, NormalRestoreStr) {}

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new HairWhip();
			//this could have been done as a special extended content in the frontend, but i've already got the attackwith interface for anemone hair, might as well do it here. 
			internal override bool canAttackWith(Hair hair) //only allow it if it's Ret2Go! (or if it's braided a la harem style)
			{
				return hair.length >= 36.0f && (hair.style == HairStyle.BRAIDED || hair.style == HairStyle.PONYTAIL) && hair.hairColor == HairFurColors.PURPLE;
			}
		}

		private class AnemoneHair : HairType
		{
			public AnemoneHair() : base(Species.ANEMONE.defaultHair, 8.0f, true, AnemoneDesc, AnemoneFullDesc, AnemonePlayerStr, AnemoneGrowStr, AnemoneTransformStr, AnemoneRestoreStr) { }

			public override bool canCut => false;

			public override bool canLengthen => false;

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new AnemoneSting();

		}

		private class Vines : HairType
		{
			public Vines() : base(Species.DRYAD.defaultVineColor, 12.0f, true, VineDesc, VineFullDesc, VinePlayerStr, VineGrowStr, VineTransformStr, VineRestoreStr) { }

			public override bool canCut => false;

			public override bool canLengthen => false;

		}

		private class BasiliskSpines : HairType
		{
			public BasiliskSpines() : base(Species.BASILISK.defaultSpines, 2.0f, true, SpineDesc, SpineFullDesc, SpinePlayerStr, SpineGrowStr, SpineTransformStr, SpineRestoreStr) { }

			public override bool canCut => false;

			public override bool canLengthen => false;

			public override bool canDye => false;

			internal override bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
			{
				return false;
			}

			public override bool preventHairGrowth => true;
		}
	}

	internal sealed class HairData
	{
		internal readonly HairFurColors hairColor;
		internal readonly HairFurColors highlightColor;
		internal readonly HairType hairType;
		internal readonly HairStyle hairStyle;
		internal readonly float hairLength;
		internal readonly bool isSemiTransparent;
		internal readonly bool isNotGrowing;
		internal bool isNoHair => hairType == HairType.NO_HAIR;
		internal bool hairDeactivated => hairType == HairType.NO_HAIR || (hairLength == 0 && isNotGrowing);
		internal bool isBald => isNoHair || hairLength == 0;

		internal HairFurColors activeHairColor => hairDeactivated ? HairFurColors.NO_HAIR_FUR : hairColor;

		internal HairData(HairType type, HairFurColors color, HairFurColors highlight, HairStyle style, float hairLen, bool semiTransparent, bool notGrowing)
		{
			hairType = type;
			hairColor = color;
			highlightColor = highlight;
			hairStyle = style;
			hairLength = hairLen;
			isSemiTransparent = semiTransparent;
			isNotGrowing = notGrowing;
		}

		internal HairData()
		{
			hairType = HairType.NO_HAIR;
			hairColor = HairFurColors.NO_HAIR_FUR;
			highlightColor = HairFurColors.NO_HAIR_FUR;
			hairStyle = HairStyle.NO_STYLE;
			hairLength = 0;
			isSemiTransparent = false;
			isNotGrowing = true;
		}
	}
}
