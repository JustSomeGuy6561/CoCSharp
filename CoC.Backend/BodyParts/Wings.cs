﻿//Wings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:36 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	//may once have been multidyeable, as we skip button location 4; location 3 is wings.
	public sealed partial class Wings : BehavioralSaveablePart<Wings, WingType, WingData>, IDyeable, IMultiToneable //if combat grants a boost to running via wings, just add a hasWings check.
	{
		public override string BodyPartName() => Name();

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
		public bool usesTone => type.usesTone;
		public bool usesHair => type.usesHair;

		internal Wings(Guid creatureID) : this(creatureID, WingType.defaultValue)
		{ }

		internal Wings(Guid creatureID, WingType wingType) : base(creatureID)
		{
			type = wingType ?? throw new ArgumentNullException(nameof(wingType));
		}

		internal Wings(Guid creatureID, TonableWings wingType, Tones tone, Tones boneTone) : this(creatureID, wingType)
		{
			if (boneTone is null) boneTone = tone;
			wingTone = Tones.IsNullOrEmpty(tone) ? wingType.defaultWingTone : tone;
			wingBoneTone = Tones.IsNullOrEmpty(boneTone) ? wingType.defaultWingBoneTone : boneTone; //caught via testing. was the same as original tone.
		}

		internal Wings(Guid creatureID, FeatheredWings wingType, HairFurColors color) : this(creatureID, wingType)
		{
			featherColor = HairFurColors.IsNullOrEmpty(color) ? wingType.defaultFeatherColor : color;
		}

		public string ShortDescriptionWithSize(bool isLarge) => type.ShortDescriptionWithSize(isLarge);
		public string ShortDescriptionWithSize(bool isLarge, bool plural) => type.ShortDescriptionWithSize(isLarge, plural);

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
				oldData.isLarge != isLarge || type.usesHair != oldData.type.usesHair || type.usesTone != oldData.type.usesTone)
			{
				NotifyDataChanged(oldData);
			}
		}

		public bool GrowLarge()
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

		public bool ShrinkToSmall()
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

		string IDyeable.locationDesc(out bool isPlural)
		{
			return type.locationDesc(false, out isPlural);
		}

		string IDyeable.postDyeDescription()
		{
			return type.postDyeText(featherColor);
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

		string IMultiToneable.buttonText()
		{
			return Name();
		}

		string IMultiToneable.memberButtonText(byte index)
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

		string IMultiToneable.memberLocationDesc(byte index, out bool isPlural)
		{
			if (index >= multiToneable.numToneableMembers)
			{
				isPlural = false;
				return "";
			}
			else if (index == 1)
			{
				return type.secondaryLocationDesc(out isPlural);
			}
			else if (index == 0)
			{
				return type.locationDesc(true, out isPlural);
			}
			else
			{
				throw new NotImplementedException("Somebody added a new toneable member and didn't implement it correctly.");
			}
		}

		string IMultiToneable.memberPostToneDescription(byte index)
		{
			return type.postToneText(this, index);
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

		internal virtual string secondaryButtonText() => Wing2Text();
		internal virtual string secondaryLocationDesc(out bool isPlural) => YourBoneDesc(out isPlural);

		internal virtual string buttonText(bool isLotion) => WingText();
		internal virtual string locationDesc(bool isLotion, out bool isPlural) => WingDesc(isLotion, out isPlural);

		internal virtual string postDyeText(HairFurColors color) => WingsDyeText(color);

		internal virtual string postToneText(Wings wings, byte index) => WingsToneText(wings, index);

		public readonly bool canChangeSize;
		public virtual bool canChangeColor => usesHair || usesTone;

		public bool defaultIsLarge => UpdateSizeOnTransform(false);

		//i hate everything.

		protected internal delegate string WingShortDesc(bool? isLarge, bool plural = true);


		private readonly WingShortDesc wingShortDesc;

		public string ShortDescription(bool plural) => wingShortDesc(null, plural);

		public string ShortDescriptionWithSize(bool isLarge) => wingShortDesc(isLarge);
		public string ShortDescriptionWithSize(bool isLarge, bool plural) => wingShortDesc(isLarge, plural);


		private readonly LongPluralDescriptor<WingData> longPluralDesc;

		public string LongDescription(WingData data, bool alternateForm, bool plural) => longPluralDesc(data, alternateForm, plural);

		public string LongDescriptionPrimary(WingData data, bool plural) => longPluralDesc(data, false, plural);

		public string LongDescriptionAlternate(WingData data, bool plural) => longPluralDesc(data, true, plural);

		protected WingType(bool canFly,
			SimplePluralDescriptor shortDesc, LongPluralDescriptor<WingData> longDesc, PlayerBodyPartDelegate<Wings> playerDesc,
			ChangeType<WingData> transform, RestoreType<WingData> restore) : base(PluralHelper(shortDesc), LongPluralHelper(longDesc), playerDesc, transform, restore)
		{
			_index = indexMaker++;
			wings.AddAt(this, _index);
			transformBehavior = canFly ? BehaviorOnTransform.CONVERT_TO_LARGE : BehaviorOnTransform.CONVERT_TO_SMALL; //was wrong, caught in testing.

			wingShortDesc = (_, plural) => shortDesc(plural);
			longPluralDesc = longDesc;

			canChangeSize = false;
		}
		//wings that support large wings need to define a behavior on transform - do they keep the large wings?
		protected WingType(BehaviorOnTransform behaviorOnTransform, WingShortDesc wingShortDesc, LongPluralDescriptor<WingData> longDesc,
			PlayerBodyPartDelegate<Wings> playerDesc, ChangeType<WingData> transform, RestoreType<WingData> restore)
			: base(WingParser(wingShortDesc), LongPluralHelper(longDesc), playerDesc, transform, restore)
		{
			_index = indexMaker++;
			wings.AddAt(this, _index);
			transformBehavior = behaviorOnTransform;

			this.wingShortDesc = wingShortDesc;
			longPluralDesc = longDesc;

			canChangeSize = true;
		}

		private static SimpleDescriptor WingParser(WingShortDesc wingShort)
		{
			if (wingShort is null) throw new ArgumentNullException(nameof(wingShort));
			return () => wingShort(null, true);
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

		public static readonly WingType NONE = new WingType(false, NoneDesc, NoneLongDesc, NonePlayerStr, NoneTransformStr, NoneRestoreStr);
		public static readonly WingType BEE_LIKE = new WingType(BehaviorOnTransform.CONVERT_TO_SMALL, BeeLikeDesc, BeeLikeLongDesc, BeeLikePlayerStr, BeeLikeTransformStr, BeeLikeRestoreStr);
		//player always has large feathered wings. small feathered wings are the new harpy wings. player can't get them naturally as of now.
		public static readonly FeatheredWings FEATHERED = new FeatheredWings(DefaultValueHelpers.defaultHarpyFeatherHair, BehaviorOnTransform.CONVERT_TO_LARGE, FeatheredDesc,
			FeatheredLongDesc, FeatheredPlayerStr, FeatheredTransformStr, FeatheredRestoreStr);
		public static readonly WingType BAT_LIKE = new WingType(BehaviorOnTransform.KEEP_SIZE, BatLikeDesc, BatLikeLongDesc, BatLikePlayerStr, BatLikeTransformStr, BatLikeRestoreStr);
		public static readonly TonableWings DRACONIC = new TonableWings(DefaultValueHelpers.defaultDragonWingTone, DefaultValueHelpers.defaultDragonWingTone,
			BehaviorOnTransform.CONVERT_TO_SMALL, DraconicDesc, DraconicLongDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly WingType FAERIE = new WingType(BehaviorOnTransform.CONVERT_TO_SMALL, FaerieDesc, FaerieLongDesc, FaeriePlayerStr, FaerieTransformStr, FaerieRestoreStr);
		public static readonly WingType DRAGONFLY = new WingType(true, DragonflyDesc, DragonflyLongDesc, DragonflyPlayerStr, DragonflyTransformStr, DragonflyRestoreStr);
		public static readonly WingType IMP = new WingType(BehaviorOnTransform.KEEP_SIZE, ImpDesc, ImpLongDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);


	}

	public class FeatheredWings : WingType
	{
		public readonly HairFurColors defaultFeatherColor;
		internal FeatheredWings(HairFurColors defaultHair, bool canFly, SimplePluralDescriptor shortDesc, LongPluralDescriptor<WingData> longDesc,
			PlayerBodyPartDelegate<Wings> playerDesc, ChangeType<WingData> transform, RestoreType<WingData> restore)
			: base(canFly, shortDesc, longDesc, playerDesc, transform, restore)
		{
			defaultFeatherColor = defaultHair;
		}

		internal FeatheredWings(HairFurColors defaultHair, BehaviorOnTransform behaviorOnTransform, WingShortDesc wingShortDesc, LongPluralDescriptor<WingData> longDesc,
			PlayerBodyPartDelegate<Wings> playerDesc, ChangeType<WingData> transform, RestoreType<WingData> restore)
			: base(behaviorOnTransform, wingShortDesc, longDesc, playerDesc, transform, restore)
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

		internal override string postDyeText(HairFurColors color) => FeatheredWingsPostDyeText(color);
	}

	public class TonableWings : WingType
	{
		public readonly Tones defaultWingTone;
		public readonly Tones defaultWingBoneTone;
		internal TonableWings(Tones defaultTone, Tones defaultBoneTone, bool canFly, SimplePluralDescriptor shortDesc, LongPluralDescriptor<WingData> longDesc,
			PlayerBodyPartDelegate<Wings> playerDesc, ChangeType<WingData> transform, RestoreType<WingData> restore)
			: base(canFly, shortDesc, longDesc, playerDesc, transform, restore)
		{
			defaultWingTone = defaultTone;
			defaultWingBoneTone = defaultBoneTone;
		}

		internal TonableWings(Tones defaultTone, Tones defaultBoneTone, BehaviorOnTransform behaviorOnTransform, WingShortDesc wingShortDesc,
			LongPluralDescriptor<WingData> longDesc, PlayerBodyPartDelegate<Wings> playerDesc, ChangeType<WingData> transform, RestoreType<WingData> restore)
			: base(behaviorOnTransform, wingShortDesc, longDesc, playerDesc, transform, restore)
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

		internal override string postToneText(Wings wings, byte index) => TonablePostToneText(wings, index);
	}

	public sealed class WingData : BehavioralSaveablePartData<WingData, Wings, WingType>
	{
		public readonly HairFurColors featherColor;
		public readonly Tones wingTone;
		public readonly Tones wingBoneTone;
		public readonly bool isLarge;

		public readonly bool usesHair;
		public readonly bool usesTone;

		public override WingData AsCurrentData()
		{
			return this;
		}

		internal WingData(Wings source) : base(GetID(source), GetBehavior(source))
		{
			featherColor = source.featherColor;
			wingTone = source.wingTone;
			wingBoneTone = source.wingBoneTone;
			isLarge = source.isLarge;

			usesHair = source.usesHair;
			usesTone = source.usesTone;
		}

		public string ShortDescriptionWithSize(bool isLarge) => type.ShortDescriptionWithSize(isLarge);
		public string ShortDescriptionWithSize(bool isLarge, bool plural) => type.ShortDescriptionWithSize(isLarge, plural);
	}
}
