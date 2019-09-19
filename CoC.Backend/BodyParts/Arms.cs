//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

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

	public sealed class Arms : BehavioralSaveablePart<Arms, ArmType, ArmData>
	{
		public readonly Hands hands;

		private BodyData bodyData => source.body.AsReadOnlyData();

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

		public bool usesTone => type is ToneArms;
		public bool usesFur => type is FurArms;

		internal Arms(Creature source) : this(source, ArmType.defaultValue) { }

		internal Arms(Creature source, ArmType armType) : base(source)
		{
			_type = armType ?? throw new ArgumentNullException(nameof(armType));
			hands = new Hands(source, type.handType, (x) => x ? epidermis : secondaryEpidermis);
		}

		//default implementation of update and restore are fine

		internal override bool Validate(bool correctInvalidData)
		{
			ArmType armType = type;
			bool retVal = ArmType.Validate(ref armType, correctInvalidData);
			type = armType; //automatically sets hand.
			return retVal;
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

		internal abstract bool hasPrimaryFur { get; }
		internal virtual bool hasSecondaryFur => false;
		internal abstract bool hasPrimaryTone { get; }
		internal virtual bool hasSecondaryTone => false;

		public override int index => _index;
		private readonly int _index;

		private protected ArmType(HandType hand, EpidermisType epidermis,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc,
			ChangeType<Arms> transform, RestoreType<Arms> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
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
		public static readonly ToneArms HUMAN = new ToneArms(HandType.HUMAN, EpidermisType.SKIN, Species.HUMAN.defaultTone, SkinTexture.NONDESCRIPT, true, HumanDescStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FurArms HARPY = new FurArms(HandType.HUMAN, EpidermisType.FEATHERS, Species.HARPY.defaultFeathers, FurTexture.NONDESCRIPT, true, HarpyDescStr, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ToneArms SPIDER = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Species.SPIDER.defaultTone, SkinTexture.SHINY, false, SpiderDescStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ToneArms BEE = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Species.BEE.defaultTone, SkinTexture.SHINY, false, BeeDescStr, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ToneArms DRAGON = new ToneArms(HandType.DRAGON, EpidermisType.SCALES, Species.DRAGON.defaultTone, SkinTexture.NONDESCRIPT, true, DragonDescStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ToneArms IMP = new ToneArms(HandType.IMP, EpidermisType.SCALES, Species.IMP.defaultTone, SkinTexture.NONDESCRIPT, true, ImpDescStr, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ToneArms LIZARD = new ToneArms(HandType.LIZARD, EpidermisType.SCALES, Species.LIZARD.defaultTone, SkinTexture.NONDESCRIPT, true, LizardDescStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ToneArms SALAMANDER = new ToneArms(HandType.SALAMANDER, EpidermisType.SCALES, Species.SALAMANDER.defaultTone, SkinTexture.NONDESCRIPT, false, SalamanderDescStr, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly FurArms WOLF = new FurArms(HandType.DOG, EpidermisType.FUR, Species.DOG.defaultFur, FurTexture.NONDESCRIPT, true, WolfDescStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FurArms COCKATRICE = new CockatriceArms();
		public static readonly FurArms RED_PANDA = new FurArms(HandType.RED_PANDA, EpidermisType.FUR, Species.RED_PANDA.defaultUnderFur, FurTexture.SOFT, false, RedPandaDescStr, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		public static readonly FurArms FERRET = new FerretArms();
		public static readonly FurArms CAT = new FurArms(HandType.CAT, EpidermisType.FUR, Species.CAT.defaultFur, FurTexture.NONDESCRIPT, true, CatDescStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly FurArms DOG = new FurArms(HandType.DOG, EpidermisType.FUR, Species.DOG.defaultFur, FurTexture.NONDESCRIPT, true, DogDescStr, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FurArms FOX = new FurArms(HandType.FOX, EpidermisType.FUR, Species.FOX.defaultFur, FurTexture.NONDESCRIPT, true, FoxDescStr, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//added gooey arms - it was a weird case where claws could be goopy, and that complicates things.
		public static readonly ToneArms GOO = new ToneArms(HandType.GOO, EpidermisType.GOO, Species.GOO.defaultTone, SkinTexture.SLIMY, true, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//Add new Arm Types Here.

		private sealed class FerretArms : FurArms
		{
			private readonly FurColor defaultSecondaryColor = Species.FERRET.defaultUnderFur;
			public FerretArms() : base(HandType.FERRET, EpidermisType.FUR, Species.FERRET.defaultFur, FurTexture.NONDESCRIPT,
				true, FerretDescStr, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr)
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

			internal override bool hasSecondaryFur => true;

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
			private readonly Tones defaultScales = Species.COCKATRICE.defaultScaleTone;
			private readonly SkinTexture defaultScaleTexture = SkinTexture.NONDESCRIPT;

			public CockatriceArms() : base(HandType.COCKATRICE, EpidermisType.FEATHERS, Species.COCKATRICE.defaultPrimaryFeathers, FurTexture.NONDESCRIPT, true,
				CockatriceDescStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
			{ }

			internal override bool hasSecondaryTone => true;

			internal override EpidermalData GetSecondaryEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.supplementary.usesTone && !bodyData.supplementary.tone.isEmpty ? bodyData.supplementary.tone : defaultScales;
				return new EpidermalData(secondaryType, color, defaultScaleTexture);
			}

			internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
			{
				FurColor color = defaultColor;
				if (bodyData.currentType == BodyType.COCKATRICE && !bodyData.main.fur.isEmpty)
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
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultColor = new FurColor(defaultFurColor);
			defaultTexture = defaultFurTexture;
			mutable = canChange;
		}

		internal override bool hasPrimaryFur => true;
		internal override bool hasPrimaryTone => false;

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
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTexture = defaultSkinTexture;
			defaultTone = defTone;
			mutable = canChange;
		}

		internal override bool hasPrimaryFur => false;
		internal override bool hasPrimaryTone => true;

		internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
		{
			Tones color = mutable ? bodyData.mainSkin.tone : defaultTone;
			return new EpidermalData(primaryEpidermis, color, defaultTexture);
		}
	}

	public sealed class ArmData : BehavioralSaveablePartData<ArmData, Arms, ArmType>
	{
		public readonly EpidermalData primaryEpidermis;
		public readonly EpidermalData secondaryEpidermis;
		public readonly HandData handData;

		public ArmData(Arms source) : base(GetBehavior(source))
		{
			handData = source.hands.AsReadOnlyData();
			primaryEpidermis = source.epidermis;
			secondaryEpidermis = source.secondaryEpidermis;
		}
	}

}