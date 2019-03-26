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
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	[DataContract]
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
		internal HairFurColors featherColor { get; private set; } = HairFurColors.NO_HAIR_FUR;
		internal Tones wingTone { get; private set; } = Tones.NOT_APPLICABLE;
		public bool isLarge { get; protected set; } = false;

		protected Wings()
		{
			_type = WingType.NONE; //not using the property. set to small, which is irrelevant.
		}

		protected Wings(TonableWings wingType, Tones tone)
		{
			type = wingType; //sets isLarge automatically.
			wingTone = Tones.isNullOrEmpty(tone) ? wingType.defaultTone : tone;
		}

		protected Wings(FeatheredWings wingType, HairFurColors color)
		{
			type = wingType;
			featherColor = color;
		}
		public bool canFly => type != WingType.NONE && isLarge;

		public override bool isDefault => type == WingType.NONE;

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
			return new Wings(tonableWings, tone);
		}

		internal static Wings GenerateColoredWithSize(TonableWings tonableWings, Tones tone, bool large)
		{
			Wings retVal = new Wings(tonableWings, tone);
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
				featherColor = type.defaultHair;
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
			if (featherColor != featherCol && !HairFurColors.isNullOrEmpty(featherCol))
			{
				featherColor = featherCol;
			}
			else if (HairFurColors.isNullOrEmpty(featherColor))
			{
				featherColor = featheredWings.defaultHair;

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
			return true;
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


		public bool GrowLarge()
		{
			if (!type.canChangeSize || isLarge)
			{
				return false;
			}
			isLarge = true;
			return isLarge;
		}

		public bool ShrinkToSmall()
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

		public bool allowsDye()
		{
			return type.usesHair && type.canChangeColor;
		}

		bool IDyeable.isDifferentColor(HairFurColors dyeColor)
		{
			return dyeColor != featherColor;
		}

		bool IDyeable.attemptToDye(HairFurColors dye)
		{
			if (allowsDye() && !HairFurColors.isNullOrEmpty(dye) && dye != featherColor)
			{
				featherColor = dye;
				return true;
			}
			return false;
		}

		public bool canToneLotion()
		{
			return type.usesTone && type.canChangeColor;
		}

		bool IToneable.attemptToUseLotion(Tones tone)
		{
			if (canToneLotion() && !Tones.isNullOrEmpty(tone) && tone != this.wingTone)
			{
				wingTone = tone;
			}
			return false;
		}

		bool IToneable.isDifferentTone(Tones tone)
		{
			return wingTone != tone;
		}

		internal override Type[] saveVersions => new Type[] { typeof(WingSurrogateVersion1) };
		internal override Type currentSaveVersion => typeof(WingSurrogateVersion1);
		internal override BodyPartSurrogate<Wings, WingType> ToCurrentSave()
		{
			return new WingSurrogateVersion1()
			{
				wingType = index,
				largeWings = isLarge,
				wingTone = this.wingTone,
				featherColor = this.featherColor
			};
		}

		internal Wings(WingSurrogateVersion1 surrogate)
		{
			type = WingType.Deserialize(surrogate.wingType);
			wingTone = surrogate.wingTone;
			featherColor = surrogate.featherColor;
			isLarge = type.canChangeSize ? surrogate.largeWings : isLarge;
		}
	}

	public partial class WingType : BodyPartBehavior<WingType, Wings>
	{
		public enum BehaviorOnTransform { CONVERT_TO_SMALL, KEEP_SIZE, CONVERT_TO_LARGE }

		private static int indexMaker = 0;
		private static readonly List<WingType> wings = new List<WingType>();
		private readonly int _index;

		protected readonly BehaviorOnTransform transformBehavior;

		public virtual Tones defaultTone => Tones.NOT_APPLICABLE;
		public virtual HairFurColors defaultHair => HairFurColors.NO_HAIR_FUR;
		public bool usesTone => !Tones.isNullOrEmpty(defaultTone);
		public bool usesHair => !HairFurColors.isNullOrEmpty(defaultHair);
		public readonly bool canChangeSize;
		public virtual bool canChangeColor => !Tones.isNullOrEmpty(defaultTone) || !HairFurColors.isNullOrEmpty(defaultHair);

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
		public static readonly FeatheredWings FEATHERED = new FeatheredWings(Species.HARPY.defaultFeatherHair, BehaviorOnTransform.CONVERT_TO_LARGE, FeatheredDesc, FeatheredFullDesc, FeatheredPlayerStr, FeatheredTransformStr, FeatheredRestoreStr);
		public static readonly WingType BAT_LIKE = new WingType(BehaviorOnTransform.KEEP_SIZE, BatLikeDesc, BatLikeFullDesc, BatLikePlayerStr, BatLikeTransformStr, BatLikeRestoreStr);
		public static readonly TonableWings DRACONIC = new TonableWings(Species.DRAGON.defaultWingTone, BehaviorOnTransform.CONVERT_TO_SMALL, DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
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

	[DataContract]
	public sealed class WingSurrogateVersion1 : BodyPartSurrogate<Wings, WingType>
	{
		[DataMember]
		public int wingType;

		[DataMember]
		public HairFurColors featherColor;
		[DataMember]
		public Tones wingTone;
		[DataMember]
		public bool largeWings;

		internal override Wings ToBodyPart()
		{
			return new Wings(this);
		}
	}
}
