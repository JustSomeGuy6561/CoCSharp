//Wings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:36 AM
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CoC.Strings.BodyParts.WingStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	//note: add itoneaware if you want some wing to update with skin tone. just add a var to the type class enabling or disabling this by type, then implement it
	//for the love of god, don't do typeof checks. that's just ugly. you will also need to add wings to the list of things subscribing to the skin tone. 
	public class Wings : BodyPartBase<Wings, WingType>, IDyeable //IToneAware
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
		private bool wasDyed;
		public bool isLarge { get; protected set; }
		public bool canFly => type != WingType.NONE && isLarge;

		private HairFurColors hairColorStorage = HairFurColors.NO_HAIR_FUR;
		private Tones toneStorage = Tones.NOT_APPLICABLE;

		public HairFurColors hairColor => type.usesHair ? hairColorStorage : HairFurColors.NO_HAIR_FUR;
		public Tones tone => type.usesTone ? toneStorage : Tones.NOT_APPLICABLE;

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
		public bool UpdateWings(WingType wingType, HairFurColors hairColor, Tones tone)
		{
			if (type == wingType)
			{
				return false;
			}
			type = wingType;
			hairColorStorage = type.usesHair ? hairColor : HairFurColors.NO_HAIR_FUR;
			toneStorage = type.usesTone ? tone : Tones.NOT_APPLICABLE;
			return true;
		}

		public bool UpdateWingsAndDisplayMessage(WingType newType, HairFurColors hairColor, Tones tone, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateWings(newType, hairColor, tone);
		}

		public bool UpdateWingsForceSize(WingType wingType, HairFurColors hairColor, Tones tone, bool large)
		{
			if (type == wingType)
			{
				return false;
			}
			bool retVal = UpdateWings(wingType, hairColor, tone);
			if (retVal)
			{
				isLarge = large;
			}
			return retVal;
		}

		public bool UpdateWingsForceSizeAndDisplayMessage(WingType newType, HairFurColors hairColor, Tones tone, bool large, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateWingsForceSize(newType, hairColor, tone, large);
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
			OutputText(restoreString(this, player));
			return Restore();
		}

		public bool canDye()
		{
			return type.usesHair;
		}

		public bool attemptToDye(HairFurColors dye)
		{
			if (canDye() && dye != HairFurColors.NO_HAIR_FUR && dye != hairColorStorage)
			{
				hairColorStorage = dye;
				return true;
			}
			return false;
		}
	}

	public class WingType : BodyPartBehavior<WingType, Wings>
	{
		protected enum BehaviorOnTransform { CONVERT_TO_SMALL, KEEP_SIZE, CONVERT_TO_LARGE }

		private static int indexMaker = 0;
		private readonly int _index;

		protected readonly BehaviorOnTransform transformBehavior;

		public readonly bool usesHair;
		public readonly bool usesTone;


		public readonly bool canGrow;
		//wings that support large wings need to define a behavior on transform - do they keep the large wings?
		protected WingType(bool canFly, bool dyeable, bool toneAware,
			GenericDescription shortDesc, FullDescription<Wings> fullDesc, PlayerDescription<Wings> playerDesc,
			ChangeType<Wings> transform, ChangeType<Wings> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			transformBehavior = BehaviorOnTransform.CONVERT_TO_SMALL;
			canGrow = false;
		}

		protected WingType(BehaviorOnTransform behaviorOnTransform, bool dyeable, bool toneAware,
			GenericDescription shortDesc, FullDescription<Wings> fullDesc, PlayerDescription<Wings> playerDesc,
			ChangeType<Wings> transform, ChangeType<Wings> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
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

		public static readonly WingType NONE = new WingType(false, false, false, NoneDesc, NoneFullDesc, NonePlayerStr, NoneTransformStr, NoneRestoreStr);
		public static readonly WingType BEE_LIKE = new WingType(BehaviorOnTransform.CONVERT_TO_SMALL, false, false, BeeLikeDesc, BeeLikeFullDesc, BeeLikePlayerStr, BeeLikeTransformStr, BeeLikeRestoreStr);
		//player always has larged feathered wings. small feathered wings are the new harpy wings. player can't get them naturally as of now.
		public static readonly WingType FEATHERED = new WingType(BehaviorOnTransform.CONVERT_TO_LARGE, true, false, FeatheredDesc, FeatheredFullDesc, FeatheredPlayerStr, FeatheredTransformStr, FeatheredRestoreStr);
		public static readonly WingType BAT_LIKE = new WingType(BehaviorOnTransform.KEEP_SIZE, false, false, BatLikeDesc, BatLikeFullDesc, BatLikePlayerStr, BatLikeTransformStr, BatLikeRestoreStr);
		public static readonly WingType DRACONIC = new WingType(BehaviorOnTransform.CONVERT_TO_SMALL, false, true, DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly WingType FAERIE = new WingType(BehaviorOnTransform.CONVERT_TO_SMALL, false, true, FaerieDesc, FaerieFullDesc, FaeriePlayerStr, FaerieTransformStr, FaerieRestoreStr);
		public static readonly WingType DRAGONFLY = new WingType(true, false, true, DragonflyDesc, DragonflyFullDesc, DragonflyPlayerStr, DragonflyTransformStr, DragonflyRestoreStr);
		public static readonly WingType IMP = new WingType(BehaviorOnTransform.KEEP_SIZE, false, false, ImpDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
	}
}
