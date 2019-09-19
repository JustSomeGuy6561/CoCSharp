//Wings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:36 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	//may once have been multidyeable, as we skip button location 4; location 3 is wings.
	public sealed class Wings : BehavioralSaveablePart<Wings, WingType, WingData>, IDyeable, IMultiToneable //if combat grants a boost to running via wings, just add a hasWings check.
	{
		public HairFurColors featherColor
		{
			get => _featherColor;
			private set => _featherColor = value;
		}
		private HairFurColors _featherColor = HairFurColors.NO_HAIR_FUR;
		public Tones wingTone
		{
			get => _wingTone;
			private set => _wingTone = value;
		}
		private Tones _wingTone = Tones.NOT_APPLICABLE;
		public Tones wingBoneTone
		{
			get => _wingBoneTone;
			private set => _wingBoneTone = value;
		}
		private Tones _wingBoneTone = Tones.NOT_APPLICABLE;
		public bool isLarge { get; private set; } = false;

		public override WingType type
		{
			get => _type;
			protected set
			{
				if (value != _type)
				{
					isLarge = value.UpdateSizeOnTransform(isLarge);
					value.UpdateColorOnTransform(ref _featherColor, ref _wingTone, ref _wingBoneTone);
				}
				_type = value;
			}
		}
		private WingType _type = WingType.NONE;

		public override WingType defaultType => WingType.defaultValue;

		public bool hasWings => type != WingType.NONE;
		public bool canFly => type != WingType.NONE && isLarge;

		internal Wings(Creature source) : this(source, WingType.defaultValue)
		{ }

		internal Wings(Creature source, WingType wingType) : base(source)
		{
			type = wingType ?? throw new ArgumentNullException(nameof(wingType));
		}

		internal Wings(Creature source, TonableWings wingType, Tones tone, Tones boneTone) : this(source, wingType)
		{
			if (boneTone is null) boneTone = tone;
			wingTone = Tones.IsNullOrEmpty(tone) ? wingType.defaultWingTone : tone;
			wingBoneTone = Tones.IsNullOrEmpty(boneTone) ? wingType.defaultWingBoneTone : boneTone; //caught via testing. was the same as original tone.
		}

		internal Wings(Creature source, FeatheredWings wingType, HairFurColors color) : this(source, wingType)
		{
			featherColor = HairFurColors.IsNullOrEmpty(color) ? wingType.defaultFeatherColor : color;
		}

		public override WingData AsReadOnlyData()
		{
			return new WingData(this);
		}

		internal override bool UpdateType(WingType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = newType;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateWingsAndChangeColor(FeatheredWings featheredWings, HairFurColors featherCol)
		{
			if (featheredWings == null || type == featheredWings)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = featheredWings;
			if (featherColor != featherCol && !HairFurColors.IsNullOrEmpty(featherCol) && type.canChangeColor)
			{
				featherColor = featherCol;
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone)
		{
			if (toneWings == null || type == toneWings)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = toneWings;

			if (!Tones.IsNullOrEmpty(tone) && type.canChangeColor)
			{
				wingTone = tone;
				wingBoneTone = tone;
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone, Tones boneTone)
		{
			if (toneWings == null || type == toneWings)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = toneWings;

			if (type.canChangeColor)
			{
				if (!Tones.IsNullOrEmpty(tone))
				{
					wingTone = tone;
					wingBoneTone = boneTone;
				}

				if (!Tones.IsNullOrEmpty(boneTone))
				{
					wingBoneTone = boneTone;
				}
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateWingsForceSize(WingType wingType, bool large)
		{
			if (wingType == null || type == wingType)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = wingType;
			if (type.canChangeSize) isLarge = large;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}


		internal bool UpdateWingsForceSizeChangeColor(FeatheredWings featheredWings, HairFurColors featherCol, bool large)
		{
			if (featheredWings == null || type == featheredWings)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = featheredWings;

			if (type.canChangeSize) isLarge = large;

			if (featherColor != featherCol && !HairFurColors.IsNullOrEmpty(featherCol) && type.canChangeColor)
			{
				featherColor = featherCol;
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones tone, bool large)
		{
			if (toneWings == null || type == toneWings)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = toneWings;

			if (type.canChangeSize) isLarge = large;

			if (!Tones.IsNullOrEmpty(tone) && type.canChangeColor)
			{
				wingTone = tone;
				wingBoneTone = tone;
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones tone, Tones boneTone, bool large)
		{
			if (toneWings == null || type == toneWings)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = toneWings;

			if (type.canChangeColor)
			{
				if (!Tones.IsNullOrEmpty(tone))
				{
					wingTone = tone;
					wingBoneTone = boneTone;
				}

				if (!Tones.IsNullOrEmpty(boneTone))
				{
					wingBoneTone = boneTone;
				}
			}
			if (type.canChangeSize) isLarge = large;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		private void CheckDataChanged(WingData oldData)
		{
			if (wingTone != oldData.wingTone || wingBoneTone != oldData.wingBoneTone || featherColor != oldData.featherColor ||
				oldData.isLarge != isLarge || type.usesHair != oldData.currentType.usesHair || type.usesTone != oldData.currentType.usesTone)
			{
				NotifyDataChanged(oldData);
			}
		}

		internal bool GrowLarge()
		{
			if (!type.canChangeSize || isLarge)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			isLarge = true;
			NotifyDataChanged(oldData);
			return true;
		}

		internal bool ShrinkToSmall()
		{
			if (!type.canChangeSize || !isLarge)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			isLarge = false;
			NotifyDataChanged(oldData);
			return !isLarge;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			WingType wingType = type;
			var feather = featherColor;
			var tone = wingTone;
			var boneTone = wingBoneTone;
			bool valid = WingType.Validate(ref wingType, ref feather, ref tone, ref boneTone, correctInvalidData);
			type = wingType;
			featherColor = feather;
			wingTone = tone;
			wingBoneTone = boneTone;
			return valid;
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

	public partial class WingType : SaveableBehavior<WingType, Wings, WingData>
	{
		public enum BehaviorOnTransform { CONVERT_TO_SMALL, KEEP_SIZE, CONVERT_TO_LARGE }

		private static int indexMaker = 0;
		private static readonly List<WingType> wings = new List<WingType>();
		public static readonly ReadOnlyCollection<WingType> availableTypes = new ReadOnlyCollection<WingType>(wings);

		private readonly int _index;

		protected readonly BehaviorOnTransform transformBehavior;

		public static WingType defaultValue => NONE;


		//public virtual HairFurColors defaultFeatherColor => HairFurColors.NO_HAIR_FUR;
		//public virtual Tones defaultTone => Tones.NOT_APPLICABLE;
		//public virtual Tones defaultBoneTone => defaultTone;

		public bool usesTone => this is TonableWings;
		public bool usesHair => this is FeatheredWings;

		internal virtual SimpleDescriptor secondaryButtonText => Wing2Text;
		internal virtual SimpleDescriptor secondaryLocationDesc => YourBoneDesc;

		internal virtual string buttonText(bool isLotion) => WingText();
		internal virtual string locationDesc(bool isLotion) => WingDesc(isLotion);
		public readonly bool canChangeSize;
		public virtual bool canChangeColor => usesHair || usesTone;

		public bool defaultIsLarge => UpdateSizeOnTransform(false);

		protected WingType(bool canFly,
			SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			wings.AddAt(this, _index);
			transformBehavior = canFly ? BehaviorOnTransform.CONVERT_TO_LARGE : BehaviorOnTransform.CONVERT_TO_SMALL; //was wrong, caught in testing.
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

		internal static bool Validate(ref WingType wingType, ref HairFurColors featherCol, ref Tones wingTone, ref Tones wingBoneTone, bool correctInvalidData)
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
			return valid & wingType.ValidateType(ref featherCol, ref wingTone, ref wingBoneTone, correctInvalidData);
		}

		protected virtual bool ValidateType(ref HairFurColors featherCol, ref Tones wingTone, ref Tones wingBoneTone, bool correctInvalidData)
		{
			featherCol = HairFurColors.NO_HAIR_FUR;
			wingTone = Tones.NOT_APPLICABLE;
			wingBoneTone = Tones.NOT_APPLICABLE;
			return true;
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
		internal virtual void UpdateColorOnTransform(ref HairFurColors featherColor, ref Tones wingTone, ref Tones wingBoneTone)
		{
			featherColor = HairFurColors.NO_HAIR_FUR;
			wingTone = Tones.NOT_APPLICABLE;
			wingBoneTone = Tones.NOT_APPLICABLE;
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
		public readonly HairFurColors defaultFeatherColor;
		public FeatheredWings(HairFurColors defaultHair, bool canFly, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(canFly, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFeatherColor = defaultHair;
		}

		public FeatheredWings(HairFurColors defaultHair, BehaviorOnTransform behaviorOnTransform, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc,
			TypeAndPlayerDelegate<Wings> playerDesc, ChangeType<Wings> transform, RestoreType<Wings> restore) : base(behaviorOnTransform, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFeatherColor = defaultHair;
		}

		internal override void UpdateColorOnTransform(ref HairFurColors featherColor, ref Tones wingTone, ref Tones wingBoneTone)
		{
			featherColor = defaultFeatherColor;
			wingTone = Tones.NOT_APPLICABLE;
			wingBoneTone = Tones.NOT_APPLICABLE;
		}

		protected override bool ValidateType(ref HairFurColors featherCol, ref Tones wingTone, ref Tones wingBoneTone, bool correctInvalidData)
		{
			wingTone = Tones.NOT_APPLICABLE;
			wingBoneTone = Tones.NOT_APPLICABLE;
			if (HairFurColors.IsNullOrEmpty(featherCol))
			{
				if (correctInvalidData)
				{
					featherCol = defaultFeatherColor;
				}
				return false;
			}
			return true;
		}
	}

	public class TonableWings : WingType
	{
		public readonly Tones defaultWingTone;
		public readonly Tones defaultWingBoneTone;
		public TonableWings(Tones defaultTone, Tones defaultBoneTone, bool canFly, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc, TypeAndPlayerDelegate<Wings> playerDesc,
			ChangeType<Wings> transform, RestoreType<Wings> restore) : base(canFly, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultWingTone = defaultTone;
			defaultWingBoneTone = defaultBoneTone;
		}

		public TonableWings(Tones defaultTone, Tones defaultBoneTone, BehaviorOnTransform behaviorOnTransform, SimpleDescriptor shortDesc, DescriptorWithArg<Wings> fullDesc,
			TypeAndPlayerDelegate<Wings> playerDesc, ChangeType<Wings> transform, RestoreType<Wings> restore) : base(behaviorOnTransform, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultWingTone = defaultTone;
			defaultWingBoneTone = defaultBoneTone;
		}

		protected override bool ValidateType(ref HairFurColors featherCol, ref Tones wingTone, ref Tones wingBoneTone, bool correctInvalidData)
		{
			featherCol = HairFurColors.NO_HAIR_FUR;
			bool valid = true;
			if (Tones.IsNullOrEmpty(wingTone))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
				wingTone = defaultWingTone;
			}
			if (Tones.IsNullOrEmpty(wingBoneTone))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
				wingBoneTone = defaultWingBoneTone;
			}

			return valid;
		}
	}

	public sealed class WingData : BehavioralSaveablePartData<WingData, Wings, WingType>
	{
		public readonly HairFurColors featherColor;
		public readonly Tones wingTone;
		public readonly Tones wingBoneTone;
		public readonly bool isLarge;

		internal WingData(Wings source) : base(GetBehavior(source))
		{
			featherColor = source.featherColor;
			wingTone = source.wingTone;
			wingBoneTone = source.wingBoneTone;
			isLarge = source.isLarge;
		}
	}
}
