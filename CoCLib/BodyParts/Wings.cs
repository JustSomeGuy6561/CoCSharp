//Wings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:36 AM
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Tools;
using CoC.BodyParts.SpecialInteraction;
using static CoC.Strings.BodyParts.WingStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	//note: add itoneaware if you want some wing to update with skin tone. just add a var to the type class enabling or disabling this by type, then implement it
	//for the love of god, don't do typeof checks. that's just ugly. you will also need to add wings to the list of things subscribing to the skin tone. 
	public class Wings : BodyPartBase<Wings, WingType>, IDyeable, IToneable
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

		protected Wings()
		{
			type = WingType.NONE;
		}

		public bool isLarge { get; protected set; }
		public bool canFly => type != WingType.NONE && isLarge;

		public HairFurColors hairColor { get; private set; } = HairFurColors.NO_HAIR_FUR;
		public Tones wingTone { get; private set; } = Tones.NOT_APPLICABLE;

		public static Wings Generate()
		{
			return new Wings();
		}

		public static Wings GenerateNonStandard(WingType wingType)
		{
			return new Wings()
			{
				type = wingType
			};
		}

		//Savvy individuals can use this to change the wing color to something not the 
		//base hair color or skin tone. afaik, it'd only used by special characters
		//but w/e. i could have prevented this with itoneaware and ifuraware, but 
		//frankly, i'm fine with them doing this. this applies to all of these.
		public bool UpdateWings(WingType wingType)
		{
			if (type == wingType)
			{
				return false;
			}
			type = wingType;

			wingTone = Tones.NOT_APPLICABLE;
			hairColor = HairFurColors.NO_HAIR_FUR;

			if (type is FeatheredWings)
			{
				hairColor = type.defaultHair;
			}
			else if (type is TonableWings)
			{
				wingTone = type.defaultTone;
			}
			return true;
		}

		public bool UpdateWingsAndDisplayMessage(WingType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateWings(newType);
		}

		public bool UpdateWingsAndChangeColor(FeatheredWings featheredWings, HairFurColors featherCol)
		{
			if (type == featheredWings)
			{
				return false;
			}
			type = featheredWings;
			hairColor = featherCol;
			return true;
		}

		public bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone)
		{
			if (type == toneWings)
			{
				return false;
			}
			type = toneWings;
			this.wingTone = tone;
			return true;
		}

		public bool UpdateWingsChangeColorAndDisplayMessage(FeatheredWings featheredWings, HairFurColors featherCol, Player player)
		{
			if (type == featheredWings)
			{
				return false;
			}
			OutputText(transformInto(type, player));
			return UpdateWingsAndChangeColor(featheredWings, featherCol);
		}

		public bool UpdateWingsChangeColorAndDisplayMessage(TonableWings toneWings, Tones tone, Player player)
		{
			if (type == toneWings)
			{
				return false;
			}
			OutputText(transformInto(type, player));
			return UpdateWingsAndChangeColor(toneWings, tone);
		}


		public bool UpdateWingsForceSize(WingType wingType, bool large)
		{
			if (type == wingType)
			{
				return false;
			}
			bool retVal = UpdateWings(wingType);
			isLarge = large;
			return retVal;
		}

		public bool UpdateWingsForceSizeAndDisplayMessage(WingType newType, bool large, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateWingsForceSize(newType, large);
		}


		public bool UpdateWingsForceSizeChangeColor(FeatheredWings featheredWing, HairFurColors featherColor, bool large)
		{
			if (type == featheredWing)
			{
				return false;
			}
			bool retVal = UpdateWingsAndChangeColor(featheredWing, featherColor);
			isLarge = large;
			return retVal;
		}

		public bool UpdateWingsForceSizeChangeColorAndDisplayMessage(FeatheredWings featheredWing, HairFurColors featherColor, bool large, Player player)
		{
			if (type == featheredWing)
			{
				return false;
			}
			OutputText(transformInto(featheredWing, player));
			return UpdateWingsForceSizeChangeColor(featheredWing, featherColor, large);
		}

		public bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, bool large)
		{
			if (type == toneWings)
			{
				return false;
			}
			bool retVal = UpdateWingsAndChangeColor(toneWings, wingTone);
			isLarge = large;
			return retVal;
		}

		public bool UpdateWingsForceSizeChangeColorAndDisplayMessage(TonableWings toneWings, Tones wingTone, bool large, Player player)
		{
			if (type == toneWings)
			{
				return false;
			}
			OutputText(transformInto(toneWings, player));
			return UpdateWingsForceSizeChangeColor(toneWings, wingTone, large);
		}


		public bool GrowLarge()
		{
			if (!type.canGrow || isLarge)
			{
				return false;
			}
			isLarge = true;
			return isLarge;
		}

		public bool ShrinkToSmall()
		{
			if (!type.canGrow || !isLarge)
			{
				return false;
			}
			isLarge = false;
			return isLarge;
		}

		public override bool Restore()
		{
			if (type == WingType.NONE)
			{
				return false;
			}
			type = WingType.NONE;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == WingType.NONE)
			{
				return false;
			}
			OutputText(restoreString(player));
			return Restore();
		}

		public bool canDye()
		{
			return type.usesHair && type.canChangeColor;
		}

		public bool attemptToDye(HairFurColors dye)
		{
			if (canDye() && dye != HairFurColors.NO_HAIR_FUR && dye != hairColor)
			{
				hairColor = dye;
				return true;
			}
			return false;
		}

		public bool canToneLotion()
		{
			return type.usesTone && type.canChangeColor;
		}

		public bool attemptToUseLotion(Tones tone)
		{
			if (canToneLotion() && tone != Tones.NOT_APPLICABLE && tone != this.wingTone)
			{
				wingTone = tone;
			}
			return false;
		}
	}

	public class WingType : BodyPartBehavior<WingType, Wings>
	{
		public enum BehaviorOnTransform { CONVERT_TO_SMALL, KEEP_SIZE, CONVERT_TO_LARGE }

		private static int indexMaker = 0;
		private readonly int _index;

		protected readonly BehaviorOnTransform transformBehavior;

		public virtual Tones defaultTone => Tones.NOT_APPLICABLE;
		public virtual HairFurColors defaultHair => HairFurColors.NO_HAIR_FUR;
		public bool usesTone => defaultTone != Tones.NOT_APPLICABLE;
		public bool usesHair => defaultHair != HairFurColors.NO_HAIR_FUR;
		public readonly bool canGrow;
		public virtual bool canChangeColor => defaultTone != Tones.NOT_APPLICABLE || defaultHair != HairFurColors.NO_HAIR_FUR;


		protected WingType(bool canFly,
			SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			transformBehavior = BehaviorOnTransform.CONVERT_TO_SMALL;
			canGrow = false;
		}
		//wings that support large wings need to define a behavior on transform - do they keep the large wings?
		protected WingType(BehaviorOnTransform behaviorOnTransform,
			SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			transformBehavior = behaviorOnTransform;
			canGrow = true;
		}

		public override int index => _index;

		public bool UpdateSizeOnTransform(bool wasLarge)
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
		public static readonly FeatheredWings FEATHERED = new FeatheredWings(HairFurColors.WHITE, BehaviorOnTransform.CONVERT_TO_LARGE, FeatheredDesc, FeatheredFullDesc, FeatheredPlayerStr, FeatheredTransformStr, FeatheredRestoreStr);
		public static readonly WingType BAT_LIKE = new WingType(BehaviorOnTransform.KEEP_SIZE, BatLikeDesc, BatLikeFullDesc, BatLikePlayerStr, BatLikeTransformStr, BatLikeRestoreStr);
		public static readonly TonableWings DRACONIC = new TonableWings(Tones.DARK_RED, BehaviorOnTransform.CONVERT_TO_SMALL, DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
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

		public override HairFurColors defaultHair => _defaultHair;
	}

	public class TonableWings : WingType
	{
		private readonly Tones _defaultTone;
		public TonableWings(Tones defaultTone, bool canFly, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(canFly, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_defaultTone = defaultTone;
		}

		public TonableWings(Tones defaultTone, BehaviorOnTransform behaviorOnTransform, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc,
			TypeAndPlayerDelegate<Wings> playerDesc, ChangeType<Wings> transform, RestoreType<Wings> restore) : base(behaviorOnTransform, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_defaultTone = defaultTone;
		}

		public override Tones defaultTone => _defaultTone;
	}
}
