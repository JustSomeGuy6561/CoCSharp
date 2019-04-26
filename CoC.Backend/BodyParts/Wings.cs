//Wings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:36 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{
	//may once have been multidyeable, as we skip button location 4; location 3 is wings.
	public sealed class Wings : BehavioralSaveablePart<Wings, WingType>, IDyeable, IMultiToneable //IPerkaware? idk, there's some combat bonus for wings with running, but i feel like that can just be hard-coded.
	{
		//add to creature. 
		//public bool hasWings => wings.type != WingType.NONE;

		public override WingType type
		{
			get => _type;
			protected set
			{
				if (value != _type)
				{
					isLarge = value.UpdateSizeOnTransform(isLarge);
				}
				_type = value;
			}
		}
		private WingType _type;
		public HairFurColors featherColor { get; private set; } = HairFurColors.NO_HAIR_FUR;
		public Tones wingTone { get; private set; } = Tones.NOT_APPLICABLE;
		public Tones wingBoneTone { get; private set; } = Tones.NOT_APPLICABLE;

		public bool isLarge { get; private set; } = false;

		private Wings()
		{
			_type = WingType.NONE; //not using the property. set to small, which is irrelevant.
		}

		private Wings(TonableWings wingType, Tones tone, Tones boneTone)
		{
			type = wingType; //sets isLarge automatically.
			wingTone = Tones.IsNullOrEmpty(tone) ? wingType.defaultTone : tone;
			wingBoneTone = Tones.IsNullOrEmpty(tone) ? wingTone : tone;
		}

		private Wings(FeatheredWings wingType, HairFurColors color)
		{
			type = wingType;
			featherColor = color;
		}
		public bool canFly => type != WingType.NONE && isLarge;

		public override bool isDefault => type == WingType.NONE;

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			WingType wingType = type;
			var feather = featherColor;
			var tone = wingTone;
			var boneTone = wingBoneTone;
			bool valid = WingType.Validate(ref wingType, ref feather, ref tone, ref boneTone, correctDataIfInvalid);
			type = wingType;
			featherColor = feather;
			wingTone = tone;
			wingBoneTone = boneTone;
			return valid;
		}

		internal static Wings GenerateDefault()
		{
			return new Wings();
		}

		internal static Wings GenerateDefaultOfType(WingType wingType)
		{
			return new Wings()
			{
				type = wingType
			};
		}

		internal static Wings GenerateDefaultWithSize(WingType wingType, bool large)
		{
			Wings retVal = new Wings()
			{
				type = wingType
			};
			retVal.isLarge = retVal.type.canChangeSize ? large : retVal.isLarge;
			return retVal;
		}

		internal static Wings GenerateColored(TonableWings tonableWings, Tones tone)
		{
			return new Wings(tonableWings, tone, tone);
		}

		internal static Wings GenerateColored(TonableWings tonableWings, Tones tone, Tones boneTone)
		{
			return new Wings(tonableWings, tone, boneTone);
		}

		internal static Wings GenerateColoredWithSize(TonableWings tonableWings, Tones tone, bool large)
		{
			Wings retVal = new Wings(tonableWings, tone, tone);
			retVal.isLarge = retVal.type.canChangeSize ? large : retVal.isLarge;
			return retVal;
		}

		internal static Wings GenerateColoredWithSize(TonableWings tonableWings, Tones tone, Tones boneTone, bool large)
		{
			Wings retVal = new Wings(tonableWings, tone, boneTone);
			retVal.isLarge = retVal.type.canChangeSize ? large : retVal.isLarge;
			return retVal;
		}

		internal static Wings GenerateColored(FeatheredWings featheredWings, HairFurColors feathers)
		{
			return new Wings(featheredWings, feathers);
		}

		internal static Wings GenerateColoredWithSize(FeatheredWings featheredWings, HairFurColors feathers, bool large)
		{
			Wings retVal = new Wings(featheredWings, feathers);
			retVal.isLarge = retVal.type.canChangeSize ? large : retVal.isLarge;
			return retVal;
		}

		//Savvy individuals can use this to change the wing color to something not the 
		//base hair color or skin tone. afaik, it'd only used by special characters
		//but w/e. i could have prevented this with itoneaware and ifuraware, but 
		//frankly, i'm fine with them doing this. this applies to all of these.
		internal bool UpdateWings(WingType wingType)
		{
			if (type == wingType)
			{
				return false;
			}
			type = wingType;

			wingTone = Tones.NOT_APPLICABLE;
			featherColor = HairFurColors.NO_HAIR_FUR;

			if (type is FeatheredWings)
			{
				featherColor = type.defaultFeatherColor;
			}
			else if (type is TonableWings)
			{
				wingTone = type.defaultTone;
			}
			return true;
		}

		internal bool UpdateWingsAndChangeColor(FeatheredWings featheredWings, HairFurColors featherCol)
		{
			if (type == featheredWings)
			{
				return false;
			}
			type = featheredWings;
			if (featherColor != featherCol && !HairFurColors.IsNullOrEmpty(featherCol))
			{
				featherColor = featherCol;
			}
			else if (HairFurColors.IsNullOrEmpty(featherColor))
			{
				featherColor = featheredWings.defaultFeatherColor;

			}
			return true;
		}

		internal bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone)
		{
			if (type == toneWings)
			{
				return false;
			}
			type = toneWings;
			this.wingTone = tone;
			if (wingBoneTone.isEmpty)
			{
				wingBoneTone = tone;
			}
			return true;
		}

		internal bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone, Tones boneTone)
		{
			if (type == toneWings)
			{
				return false;
			}
			type = toneWings;
			this.wingTone = tone;
			wingBoneTone = tone;
			return true;
		}

		internal bool UpdateWingsForceSize(WingType wingType, bool large)
		{
			if (type == wingType)
			{
				return false;
			}
			bool retVal = UpdateWings(wingType);
			isLarge = large;
			return retVal;
		}


		internal bool UpdateWingsForceSizeChangeColor(FeatheredWings featheredWing, HairFurColors featherColor, bool large)
		{
			if (type == featheredWing)
			{
				return false;
			}
			bool retVal = UpdateWingsAndChangeColor(featheredWing, featherColor);
			isLarge = large;
			return retVal;
		}

		internal bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, bool large)
		{
			if (type == toneWings)
			{
				return false;
			}
			bool retVal = UpdateWingsAndChangeColor(toneWings, wingTone);
			isLarge = large;
			return retVal;
		}

		internal bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, Tones wingBoneTone, bool large)
		{
			if (type == toneWings)
			{
				return false;
			}
			bool retVal = UpdateWingsAndChangeColor(toneWings, wingTone, wingBoneTone);
			isLarge = large;
			return retVal;
		}


		internal bool GrowLarge()
		{
			if (!type.canChangeSize || isLarge)
			{
				return false;
			}
			isLarge = true;
			return isLarge;
		}

		internal bool ShrinkToSmall()
		{
			if (!type.canChangeSize || !isLarge)
			{
				return false;
			}
			isLarge = false;
			return !isLarge;
		}

		internal override bool Restore()
		{
			if (type == WingType.NONE)
			{
				return false;
			}
			type = WingType.NONE;
			return true;
		}

		bool IDyeable.allowsDye()
		{
			return type.usesHair && type.canChangeColor;
		}

		bool IDyeable.isDifferentColor(HairFurColors dyeColor)
		{
			return dyeColor != featherColor;
		}

		bool IDyeable.attemptToDye(HairFurColors dye)
		{
			if (canDye && !HairFurColors.IsNullOrEmpty(dye) && dye != featherColor)
			{
				featherColor = dye;
				return true;
			}
			return false;
		}

		string IDyeable.buttonText()
		{
			return type.buttonText(false);
		}

		string IDyeable.locationDesc()
		{
			return type.locationDesc(false);
		}


		bool IMultiToneable.canToneOil(byte index)
		{
			return type.usesTone && type.canChangeColor;
		}

		bool IMultiToneable.attemptToTone(Tones tone, byte index)
		{
			if (!canTone(index) || Tones.IsNullOrEmpty(tone) || !multiToneable.isDifferentTone(tone, index))
			{
				return false;
			}
			else if (index == 1)
			{
				wingBoneTone = tone;
			}
			else
			{
				wingTone = tone;
			}
			return true;
		}

		bool IMultiToneable.isDifferentTone(Tones tone, byte index)
		{
			if (index >= multiToneable.numToneableMembers)
			{
				return false;
			}
			else if (index == 1)
			{
				return wingBoneTone != tone;
			}
			else
			{
				return wingTone != tone;
			}
		}

		byte IMultiToneable.numToneableMembers => 2;

		string IMultiToneable.buttonText(byte index)
		{
			if (index >= multiToneable.numToneableMembers)
			{
				return "";
			}
			else if (index == 1)
			{
				return type.secondaryButtonText();
			}
			else if (index == 0)
			{
				return type.buttonText(true);
			}
			else
			{
				throw new NotImplementedException("Somebody added a new toneable member and didn't implement it correctly.");
			}
		}

		string IMultiToneable.locationDesc(byte index)
		{
			if (index >= multiToneable.numToneableMembers)
			{
				return "";
			}
			else if (index == 1)
			{
				return type.secondaryLocationDesc();
			}
			else if (index == 0)
			{
				return type.locationDesc(true);
			}
			else
			{
				throw new NotImplementedException("Somebody added a new toneable member and didn't implement it correctly.");
			}
		}

		private IMultiToneable multiToneable => this;
		private IDyeable dyeable => this;
		private bool canDye => dyeable.allowsDye();
		private bool canTone(byte index) => multiToneable.canToneOil(index);
	}

	public partial class WingType : SaveableBehavior<WingType, Wings>
	{
		public enum BehaviorOnTransform { CONVERT_TO_SMALL, KEEP_SIZE, CONVERT_TO_LARGE }

		private static int indexMaker = 0;
		private static readonly List<WingType> wings = new List<WingType>();
		private readonly int _index;

		protected readonly BehaviorOnTransform transformBehavior;

		public virtual HairFurColors defaultFeatherColor => HairFurColors.NO_HAIR_FUR;
		public virtual Tones defaultTone => Tones.NOT_APPLICABLE;
		public virtual Tones defaultBoneTone => defaultTone;

		public bool usesTone => !Tones.IsNullOrEmpty(defaultTone);
		public bool usesHair => !HairFurColors.IsNullOrEmpty(defaultFeatherColor);

		internal virtual SimpleDescriptor secondaryButtonText => Wing2Text;
		internal virtual SimpleDescriptor secondaryLocationDesc => YourBoneDesc;

		internal virtual string buttonText(bool isLotion) => WingText();
		internal virtual string locationDesc(bool isLotion) => WingDesc(isLotion);
		public readonly bool canChangeSize;
		public virtual bool canChangeColor => !Tones.IsNullOrEmpty(defaultTone) || !HairFurColors.IsNullOrEmpty(defaultFeatherColor);

		public bool defaultIsLarge => UpdateSizeOnTransform(false);

		protected WingType(bool canFly,
			SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			wings.AddAt(this, _index);
			transformBehavior = BehaviorOnTransform.CONVERT_TO_SMALL;
			canChangeSize = false;
		}
		//wings that support large wings need to define a behavior on transform - do they keep the large wings?
		protected WingType(BehaviorOnTransform behaviorOnTransform,
			SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			wings.AddAt(this, _index);
			transformBehavior = behaviorOnTransform;
			canChangeSize = true;
		}

		public override int index => _index;

		internal static WingType Deserialize(int index)
		{
			if (index < 0 || index >= wings.Count)
			{
				throw new ArgumentException("index for back type deserialize out of range");
			}
			else
			{
				WingType wing = wings[index];
				if (wing != null)
				{
					return wing;
				}
				else
				{
					throw new ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref WingType wingType, ref HairFurColors featherCol, ref Tones wingTone, ref Tones wingBoneTone, bool correctInvalidData = false)
		{
			bool valid = true;
			if (!wings.Contains(wingType))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				wingType = NONE;
				valid = false;
			}
			if (wingType.usesHair && ((!wingType.canChangeColor && featherCol != wingType.defaultFeatherColor) || featherCol.isEmpty))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				featherCol = wingType.defaultFeatherColor;
				valid = false;
			}
			if (wingType.usesTone && ((!wingType.canChangeColor && wingTone != wingType.defaultTone) || wingTone.isEmpty))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				wingTone = wingType.defaultTone;
				valid = false;
			}
			if (wingType.usesTone && ((!wingType.canChangeColor && wingBoneTone != wingType.defaultBoneTone) || wingBoneTone.isEmpty))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				wingBoneTone = wingType.defaultBoneTone;
				valid = false;

			}
			return valid;
		}

		internal bool UpdateSizeOnTransform(bool wasLarge)
		{
			if (transformBehavior == BehaviorOnTransform.CONVERT_TO_LARGE)
			{
				return true;
			}
			else if (transformBehavior == BehaviorOnTransform.KEEP_SIZE)
			{
				return wasLarge;
			}
			else //if (transformBehavior == BehaviorOnTransform.CONVERT_TO_SMALL)
			{
				return false;
			}
		}

		public static readonly WingType NONE = new WingType(false, NoneDesc, NoneFullDesc, NonePlayerStr, NoneTransformStr, NoneRestoreStr);
		public static readonly WingType BEE_LIKE = new WingType(BehaviorOnTransform.CONVERT_TO_SMALL, BeeLikeDesc, BeeLikeFullDesc, BeeLikePlayerStr, BeeLikeTransformStr, BeeLikeRestoreStr);
		//player always has larged feathered wings. small feathered wings are the new harpy wings. player can't get them naturally as of now.
		public static readonly FeatheredWings FEATHERED = new FeatheredWings(Species.HARPY.defaultFeatherHair, BehaviorOnTransform.CONVERT_TO_LARGE, FeatheredDesc, FeatheredFullDesc, FeatheredPlayerStr, FeatheredTransformStr, FeatheredRestoreStr);
		public static readonly WingType BAT_LIKE = new WingType(BehaviorOnTransform.KEEP_SIZE, BatLikeDesc, BatLikeFullDesc, BatLikePlayerStr, BatLikeTransformStr, BatLikeRestoreStr);
		public static readonly TonableWings DRACONIC = new TonableWings(Species.DRAGON.defaultWingTone, Species.DRAGON.defaultWingBoneTone, BehaviorOnTransform.CONVERT_TO_SMALL, DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly WingType FAERIE = new WingType(BehaviorOnTransform.CONVERT_TO_SMALL, FaerieDesc, FaerieFullDesc, FaeriePlayerStr, FaerieTransformStr, FaerieRestoreStr);
		public static readonly WingType DRAGONFLY = new WingType(true, DragonflyDesc, DragonflyFullDesc, DragonflyPlayerStr, DragonflyTransformStr, DragonflyRestoreStr);
		public static readonly WingType IMP = new WingType(BehaviorOnTransform.KEEP_SIZE, ImpDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);


	}

	public class FeatheredWings : WingType
	{
		private readonly HairFurColors _defaultHair;
		public FeatheredWings(HairFurColors defaultHair, bool canFly, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(canFly, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_defaultHair = defaultHair;
		}

		public FeatheredWings(HairFurColors defaultHair, BehaviorOnTransform behaviorOnTransform, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc,
			TypeAndPlayerDelegate<Wings> playerDesc, ChangeType<Wings> transform, RestoreType<Wings> restore) : base(behaviorOnTransform, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_defaultHair = defaultHair;
		}

		public override HairFurColors defaultFeatherColor => _defaultHair;
	}

	public class TonableWings : WingType
	{
		private readonly Tones _defaultTone;
		private readonly Tones _defaultBoneTone;
		public TonableWings(Tones defaultTone, Tones defaultBoneTone, bool canFly, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(canFly, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_defaultTone = defaultTone;
			_defaultBoneTone = defaultBoneTone;
		}

		public TonableWings(Tones defaultTone, Tones defaultBoneTone, BehaviorOnTransform behaviorOnTransform, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc,
			TypeAndPlayerDelegate<Wings> playerDesc, ChangeType<Wings> transform, RestoreType<Wings> restore) : base(behaviorOnTransform, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_defaultTone = defaultTone;
			_defaultBoneTone = defaultBoneTone;
		}

		public override Tones defaultTone => _defaultTone;
		public override Tones defaultBoneTone => _defaultBoneTone;
	}
}
