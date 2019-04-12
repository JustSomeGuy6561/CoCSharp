//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
	[DataContract]
	public class Arms : BehavioralSaveablePart<Arms, ArmType>, IBodyAware
	{
		public readonly Hands hands;

		private BodyDataGetter bodyData;

		private Epidermis epidermis => type.GetEpidermis(true, bodyData());
		private Epidermis secondaryEpidermis => type.GetEpidermis(false, bodyData());

		public EpidermalData epidermalData => epidermis.GetEpidermalData();
		public EpidermalData secondaryEpidermalData => secondaryEpidermis.GetEpidermalData();

		private protected Arms(ArmType type)
		{
			_type = type ?? throw new ArgumentNullException();
			hands = Hands.Generate(type.handType);
		}

		public override ArmType type
		{
			get => _type;
			protected set
			{
				_type = value;
				hands.UpdateHands(value.handType);
			}
		}
		private ArmType _type;

		public override bool isDefault => type == ArmType.HUMAN;
		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			ArmType armType = type;
			bool retVal = ArmType.Validate(ref armType, correctDataIfInvalid);
			type = armType; //automatically sets hand.
			return retVal;
		}


		internal static Arms GenerateDefault()
		{
			return new Arms(ArmType.HUMAN);
		}

		internal static Arms GenerateDefaultOfType(ArmType type)
		{
			return new Arms(type);
		}

		internal override bool Restore()
		{
			if (isDefault)
			{
				return false;
			}
			type = ArmType.HUMAN;

			return true;
		}

		internal bool UpdateArms(ArmType armType)
		{
			if (armType == null || type == armType)
			{
				return false;
			}
			type = armType;
			return true;
		}

		public bool usesTone => type is ToneArms;
		public bool usesFur => type is FurArms;

		#region IBodyAware
		void IBodyAware.GetBodyData(BodyDataGetter getter)
		{
			bodyData = getter;
		}
		#endregion

		#region Serialization
		private protected override Type currentSaveVersion => typeof(ArmSurrogateVersion1);

		private protected override Type[] saveVersions => new Type[] { typeof(ArmSurrogateVersion1) };

		private protected override BehavioralSurrogateBase<Arms, ArmType> ToCurrentSave()
		{
			return new ArmSurrogateVersion1()
			{
				armType = index
			};
		}

		internal Arms(ArmSurrogateVersion1 surrogate) : this(ArmType.Deserialize(surrogate.armType)) { }
		#endregion
	}

	public abstract partial class ArmType : SaveableBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;
		private static readonly List<ArmType> arms = new List<ArmType>();

		public readonly HandType handType;
		public readonly EpidermisType epidermisType;

		//update the original and secondary original based on the current data. 

		internal abstract Epidermis GetEpidermis(bool primary, in BodyData bodyData);

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

		internal static bool Validate(ref ArmType armType, bool correctInvalidData = false)
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
		//Add new Arm Types Here.

		private sealed class FerretArms : FurArms
		{
			private readonly FurColor defaultSecondaryColor = Species.FERRET.defaultUnderFur;
			public FerretArms() : base(HandType.FERRET, EpidermisType.FUR, Species.FERRET.defaultFur, FurTexture.NONDESCRIPT, true, FerretDescStr,
				FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr)
			{ }

			internal override Epidermis GetEpidermis(bool primary, in BodyData bodyData)
			{
				Epidermis source = primary ? bodyData.primary : bodyData.secondary;
				FurColor color = primary ? defaultColor : defaultSecondaryColor;

				if (source.usesFur && !source.fur.isEmpty)
				{
					color = source.fur;
				}
				//detects weird edge case wher both halves of ferret arms use hair for both. if the primary hair is null and used the hair, force the secondary to use the default.
				//clever logic people may note this ignores the case where the primary used its default - that'd only happen if the hair was empty, though, so it's irrelevant
				else if (!bodyData.hairColor.isEmpty && (primary || bodyData.primary.fur.isEmpty)) 
				{
					color = new FurColor(bodyData.hairColor);
				}
				return Epidermis.Generate((FurBasedEpidermisType)epidermisType, color, defaultTexture);
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
			internal override Epidermis GetEpidermis(bool primary, in BodyData bodyData)
			{
				if (primary)
				{
					FurColor color = defaultColor;
					if (bodyData.bodyType == BodyType.COCKATRICE && !bodyData.primary.fur.isEmpty)
					{
						color = bodyData.primary.fur;
					}
					else if (!bodyData.hairColor.isEmpty)
					{
						color = new FurColor(bodyData.hairColor);
					}
					return Epidermis.Generate((FurBasedEpidermisType)epidermisType, color, defaultTexture);
				}
				else
				{
					Tones color = bodyData.secondary.usesTone && !bodyData.secondary.tone.isEmpty ? bodyData.secondary.tone : defaultScales;
					return Epidermis.Generate(secondaryType, color, defaultScaleTexture);
				}
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
		protected readonly bool mutable;
		internal FurArms(HandType hand, FurBasedEpidermisType epidermis, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultColor = new FurColor(defaultFurColor);
			defaultTexture = defaultFurTexture;
			mutable = canChange;
		}

		internal override Epidermis GetEpidermis(bool primary, in BodyData bodyData)
		{
			if (primary && mutable)
			{
				FurColor color = this.defaultColor;
				if (bodyData.secondary.usesFur && !bodyData.secondary.fur.isEmpty) // can't be null
				{
					color = bodyData.secondary.fur;
				}
				else if (bodyData.primary.usesFur && !bodyData.primary.fur.isEmpty) //can't be null
				{
					color = bodyData.primary.fur;
				}
				else if (!bodyData.hairColor.isEmpty) //can't be null
				{
					color = new FurColor(bodyData.hairColor);
				}
				return Epidermis.Generate((FurBasedEpidermisType)epidermisType, color, defaultTexture);
			}
			else if (primary)
			{
				return Epidermis.Generate((FurBasedEpidermisType)epidermisType, defaultColor, defaultTexture);
			}
			else
			{
				return Epidermis.GenerateEmpty();
			}
		}

	}

	public class ToneArms : ArmType
	{
		public readonly SkinTexture defaultTexture;
		public readonly bool mutable;
		public readonly Tones defaultTone;
		internal ToneArms(HandType hand, ToneBasedEpidermisType epidermis, Tones defTone, SkinTexture defaultSkinTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTexture = defaultSkinTexture;
			defaultTone = defTone;
			mutable = canChange;
		}

		internal override Epidermis GetEpidermis(bool primary, in BodyData bodyData)
		{
			if (primary)
			{
				Tones color = mutable ? bodyData.primary.tone : defaultTone;
				return Epidermis.Generate((ToneBasedEpidermisType)epidermisType, color, defaultTexture);
			}
			return Epidermis.GenerateEmpty();
		}
	}

	[DataContract]
	public sealed class ArmSurrogateVersion1 : BehavioralSurrogateBase<Arms, ArmType>
	{
		[DataMember]
		public int armType;
		internal override Arms ToBodyPart()
		{
			return new Arms(this);
		}
	}
}
