//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	/*
	 * Arm covering (skin, scales, etc) Note:
	 * Arms now have a consistent logic - if the arm is furry, it will first try to use the secondary (underbody) color, if the body has one. if not, it will fallback to the
	 * body's regular fur color, if one exists. It will fallback to hair if not available, and if THAT is not available, fallback to the default for whatever type of arm it is. 
	 * Tones will simply use the primary skin tone - i cannot think of a reason arms would have a special tone different from the body. 
	 * Since this logic is implemented in the arm type, a derived class can override this behavior for custom arm types. currently, Ferrets do this.
	 * 
	 * This epidermis data must be LAZY! meaning it won't be calculated until the moment it's needed. We achieve this by using a delegate (function pointer) to get the body data on demand.
	 * We could (theoretically) get it and deal with it immediately, but Lazy implementation allows us to get around creating this before the body, if that ever happened.
	 * 
	 * An aside: unfortunately, i can't think up a means to memoize the epidermis/secondary epidermis. so it'll have to get them each time. This shouldn't be costly enough to worry about that, though
	 */

	//Note: Never fires a data change event, as it has no data that can be changed. Note that technically claws could fire a change, but whatever. 
	public sealed partial class Arms : BehavioralSaveablePart<Arms, ArmType, ArmData>
	{
		public override string BodyPartName() => Name();

		public readonly Hands hands;

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		public EpidermalData epidermis => type.GetPrimaryEpidermis(bodyData);
		public EpidermalData secondaryEpidermis => type.GetSecondaryEpidermis(bodyData);


		public override ArmType type
		{
			get => _type;
			protected set
			{
				_type = value;
				hands.UpdateType(value.handType);
			}
		}
		private ArmType _type;

		public override ArmType defaultType => ArmType.defaultValue;

		public override ArmData AsReadOnlyData()
		{
			return new ArmData(this);
		}

		protected internal override void PostPerkInit()
		{
			hands.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			hands.LateInit();
		}

		public bool usesPrimaryTone => type.hasPrimaryTone;
		public bool usesPrimaryFur => type.hasPrimaryFur;
		public bool usesSecondaryTone => type.hasSecondaryTone;
		public bool usesSecondaryFur => type.hasSecondaryFur;

		public bool usesAnyTone => usesPrimaryTone || usesSecondaryTone;
		public bool usesAnyFur => usesPrimaryFur || usesSecondaryFur;

		internal Arms(Guid creatureID) : this(creatureID, ArmType.defaultValue) { }

		internal Arms(Guid creatureID, ArmType armType) : base(creatureID)
		{
			_type = armType ?? throw new ArgumentNullException(nameof(armType));
			hands = new Hands(creatureID, type.handType, (x) => x ? epidermis : secondaryEpidermis);
		}

		//default implementation of update and restore are fine

		internal override bool Validate(bool correctInvalidData)
		{
			ArmType armType = type;
			bool retVal = ArmType.Validate(ref armType, correctInvalidData);
			type = armType; //automatically sets hand.
			return retVal;
		}

		public string EpidermisDescription()
		{
			return ArmType.ArmEpidermisDescription(epidermis, secondaryEpidermis);
		}
	}

	public abstract partial class ArmType : SaveableBehavior<ArmType, Arms, ArmData>
	{
		private static int indexMaker = 0;
		private static readonly List<ArmType> arms = new List<ArmType>();
		public static readonly ReadOnlyCollection<ArmType> availableTypes = new ReadOnlyCollection<ArmType>(arms);

		public static ArmType defaultValue => HUMAN;


		public readonly HandType handType;
		public readonly EpidermisType epidermisType;

		//update the original and secondary original based on the current data. 

		//internal abstract Epidermis GetEpidermis(bool primary, in BodyData bodyData);
		internal abstract EpidermalData GetPrimaryEpidermis(in BodyData bodyData);
		internal virtual EpidermalData GetSecondaryEpidermis(in BodyData bodyData)
		{
			return new EpidermalData();
		}

		public abstract bool hasPrimaryFur { get; }
		public virtual bool hasSecondaryFur => false;
		public abstract bool hasPrimaryTone { get; }
		public virtual bool hasSecondaryTone => false;

		public override int index => _index;
		private readonly int _index;

		private protected ArmType(HandType hand, EpidermisType epidermis,
			SimpleDescriptor shortDesc, DescriptorWithArg<ArmData> longDesc, PlayerBodyPartDelegate<Arms> playerDesc,
			ChangeType<ArmData> transform, RestoreType<ArmData> restore) : base(shortDesc, longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;
			epidermisType = epidermis;
			arms.AddAt(this, _index);
		}
		internal static ArmType Deserialize(int index)
		{
			if (index < 0 || index >= arms.Count)
			{
				throw new System.ArgumentException("index for arm type deserialize out of range");
			}
			else
			{
				ArmType arm = arms[index];
				if (arm != null)
				{
					return arm;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref ArmType armType, bool correctInvalidData)
		{
			if (arms.Contains(armType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				armType = HUMAN;
			}
			return false;
		}


		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ToneArms HUMAN = new ToneArms(HandType.HUMAN, EpidermisType.SKIN, DefaultValueHelpers.defaultHumanTone, SkinTexture.NONDESCRIPT, true, HumanDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FurArms HARPY = new FurArms(HandType.HUMAN, EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, FurTexture.NONDESCRIPT, true, HarpyDescStr, HarpyLongDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ToneArms SPIDER = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, DefaultValueHelpers.defaultSpiderTone, SkinTexture.SHINY, false, SpiderDescStr, SpiderLongDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ToneArms BEE = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, DefaultValueHelpers.defaultBeeTone, SkinTexture.SHINY, false, BeeDesc, BeeLongDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ToneArms DRAGON = new ToneArms(HandType.DRAGON, EpidermisType.SCALES, DefaultValueHelpers.defaultDragonTone, SkinTexture.NONDESCRIPT, true, DragonDescStr, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ToneArms IMP = new ToneArms(HandType.IMP, EpidermisType.SCALES, DefaultValueHelpers.defaultImpTone, SkinTexture.NONDESCRIPT, true, ImpDescStr, ImpLongDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ToneArms LIZARD = new ToneArms(HandType.LIZARD, EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, SkinTexture.NONDESCRIPT, true, LizardDescStr, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ToneArms SALAMANDER = new ToneArms(HandType.SALAMANDER, EpidermisType.SCALES, DefaultValueHelpers.defaultSalamanderTone, SkinTexture.NONDESCRIPT, false, SalamanderDescStr, SalamanderLongDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly FurArms WOLF = new FurArms(HandType.DOG, EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, FurTexture.NONDESCRIPT, true, WolfDescStr, WolfLongDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FurArms COCKATRICE = new CockatriceArms();
		public static readonly FurArms RED_PANDA = new FurArms(HandType.RED_PANDA, EpidermisType.FUR, DefaultValueHelpers.defaultRedPandaUnderFur, FurTexture.SOFT, false, RedPandaDescStr, RedPandaLongDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		public static readonly FurArms FERRET = new FerretArms();
		public static readonly FurArms CAT = new FurArms(HandType.CAT, EpidermisType.FUR, DefaultValueHelpers.defaultCatFur, FurTexture.NONDESCRIPT, true, CatDescStr, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly FurArms DOG = new FurArms(HandType.DOG, EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, FurTexture.NONDESCRIPT, true, DogDescStr, DogLongDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FurArms FOX = new FurArms(HandType.FOX, EpidermisType.FUR, DefaultValueHelpers.defaultFoxFur, FurTexture.NONDESCRIPT, true, FoxDescStr, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//added gooey arms - it was a weird case where claws could be goopy, and that complicates things.
		public static readonly ToneArms GOO = new ToneArms(HandType.GOO, EpidermisType.GOO, DefaultValueHelpers.defaultGooTone, SkinTexture.SLIMY, true, GooDesc, GooLongDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//Add new Arm Types Here.

		private sealed class FerretArms : FurArms
		{
			private readonly FurColor defaultSecondaryColor = DefaultValueHelpers.defaultFerretUnderFur;
			public FerretArms() : base(HandType.FERRET, EpidermisType.FUR, DefaultValueHelpers.defaultFerretFur, FurTexture.NONDESCRIPT,
				true, FerretDescStr, FerretLongDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr)
			{ }

			internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
			{
				FurColor color = defaultColor;
				if (bodyData.main.usesFur && !bodyData.main.fur.isEmpty)
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.activeFur.fur.isEmpty)
				{
					color = new FurColor(bodyData.activeFur.fur);
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}

			public override bool hasSecondaryFur => true;

			internal override EpidermalData GetSecondaryEpidermis(in BodyData bodyData)
			{
				FurColor color = defaultColor;
				if (bodyData.supplementary.usesFur && !bodyData.supplementary.fur.isEmpty)
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.activeFur.fur.isEmpty) //
				{
					color = new FurColor(bodyData.activeFur.fur);
				}

				//make sure the ferret is actually two-toned. not strictly speaking necessary, but whatever.
				EpidermalData primary = GetPrimaryEpidermis(bodyData);
				if (primary.fur.Equals(color))
				{
					color = color.Equals(defaultSecondaryColor) ? defaultColor : defaultSecondaryColor;
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}
		}

		//upper half of arm - primary color, as fur. lower half: scales, uses secondary tone. 
		private sealed class CockatriceArms : FurArms
		{
			private readonly ToneBasedEpidermisType secondaryType = EpidermisType.SCALES;
			private readonly Tones defaultScales = DefaultValueHelpers.defaultCockatriceScaleTone;
			private readonly SkinTexture defaultScaleTexture = SkinTexture.NONDESCRIPT;

			public CockatriceArms() : base(HandType.COCKATRICE, EpidermisType.FEATHERS, DefaultValueHelpers.defaultCockatricePrimaryFeathers, FurTexture.NONDESCRIPT, true,
				CockatriceDescStr, CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
			{ }

			public override bool hasSecondaryTone => true;

			internal override EpidermalData GetSecondaryEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.supplementary.usesTone && !bodyData.supplementary.tone.isEmpty ? bodyData.supplementary.tone : defaultScales;
				return new EpidermalData(secondaryType, color, defaultScaleTexture);
			}

			internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
			{
				FurColor color = defaultColor;
				if (bodyData.type == BodyType.COCKATRICE && !bodyData.main.fur.isEmpty)
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.hairColor.isEmpty)
				{
					color = new FurColor(bodyData.hairColor);
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}
		}
		public bool isPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}
	}

	public class FurArms : ArmType
	{
		public readonly FurColor defaultColor;
		public readonly FurTexture defaultTexture;
		protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;
		protected readonly bool mutable;


		internal FurArms(HandType hand, FurBasedEpidermisType epidermis, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<ArmData> longDesc, PlayerBodyPartDelegate<Arms> playerDesc, ChangeType<ArmData> transform, RestoreType<ArmData> restore) :
			base(hand, epidermis, shortDesc, longDesc, playerDesc, transform, restore)
		{
			defaultColor = new FurColor(defaultFurColor);
			defaultTexture = defaultFurTexture;
			mutable = canChange;
		}

		public override bool hasPrimaryFur => true;
		public override bool hasPrimaryTone => false;

		internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
		{
			FurColor color = this.defaultColor;

			if (mutable)
			{
				if (bodyData.supplementary.usesFur && !bodyData.supplementary.fur.isEmpty) // can't be null
				{
					color = bodyData.supplementary.fur;
				}
				else if (bodyData.main.usesFur && !bodyData.main.fur.isEmpty) //can't be null
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.hairColor.isEmpty) //can't be null
				{
					color = new FurColor(bodyData.hairColor);
				}
			}
			return new EpidermalData(primaryEpidermis, color, defaultTexture);
		}


	}

	public class ToneArms : ArmType
	{
		public readonly SkinTexture defaultTexture;
		public readonly bool mutable;
		public readonly Tones defaultTone;
		protected ToneBasedEpidermisType primaryEpidermis => (ToneBasedEpidermisType)epidermisType;
		internal ToneArms(HandType hand, ToneBasedEpidermisType epidermis, Tones defTone, SkinTexture defaultSkinTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<ArmData> longDesc, PlayerBodyPartDelegate<Arms> playerDesc, ChangeType<ArmData> transform, RestoreType<ArmData> restore) :
			base(hand, epidermis, shortDesc, longDesc, playerDesc, transform, restore)
		{
			defaultTexture = defaultSkinTexture;
			defaultTone = defTone;
			mutable = canChange;
		}

		public override bool hasPrimaryFur => false;
		public override bool hasPrimaryTone => true;

		internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
		{
			Tones color = mutable ? bodyData.mainSkin.tone : defaultTone;
			return new EpidermalData(primaryEpidermis, color, defaultTexture);
		}
	}

	public sealed class ArmData : BehavioralSaveablePartData<ArmData, Arms, ArmType>
	{
		public readonly EpidermalData epidermis;
		public readonly EpidermalData secondaryEpidermis;
		public readonly HandData hands;

		public bool usesPrimaryTone => type.hasPrimaryTone;
		public bool usesPrimaryFur => type.hasPrimaryFur;
		public bool usesSecondaryTone => type.hasSecondaryTone;
		public bool usesSecondaryFur => type.hasSecondaryFur;

		public bool usesAnyTone => usesPrimaryTone || usesSecondaryTone;
		public bool usesAnyFur => usesPrimaryFur || usesSecondaryFur;

		public string EpidermisDescription()
		{
			return ArmType.ArmEpidermisDescription(epidermis, secondaryEpidermis);
		}

		public override ArmData AsCurrentData()
		{
			return this;
		}


		public ArmData(Arms source) : base(GetID(source), GetBehavior(source))
		{
			hands = source.hands.AsReadOnlyData();
			epidermis = source.epidermis;
			secondaryEpidermis = source.secondaryEpidermis;
		}
	}

}