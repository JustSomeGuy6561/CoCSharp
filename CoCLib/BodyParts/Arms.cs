//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Serialization;
using CoC.Tools;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	//TODO: add nice comparison shit for this class.
	
	//This class uses the Save attribute to show what will be saved, however, the save attributes aren't actually used.
	//Reflection is slow, so if the data isn't going to change, i can just hard code it. It does, however, make the code more understandable.
	[DataContract]
	internal class Arms : BodyPartBase<Arms, ArmType>, IToneAware, IFurAware, IHairAware, ISerializable
	{
		[Save]
		public readonly Hands hands;

		/* NOTE: these do not store the right epidermis type - these are only used for storage. They are NOT to be used
		 * outside this class. in the event somebody needs to override the default descriptors, they are NOT to be used there, either.
		 * additionally, these are split into two parts - the body data and the arm data. because i can't have a universal ruleset 
		 * as body parts are different and act differently, i must store both the body data and the current data. the types can then 
		 * do whatever they'd like with the type data, regardless of what happens to the body. so if you lose fur on your body, your ferret arms dont magically change color.
		 * maybe that's the desired behavior (as that's how it works now), but it should at least notify the player - something like "your arms shift colors to match your hair, now that your body fur is gone"
		 * it also could be arm-dependant - maybe some revert to their default colors, some keep the color, and some go to hair color, idk. it's possible to do any of those.
		 *
		 * Quick aside: body values aren't serialized here - that's redundant.
		 */
		private readonly Epidermis _bodyEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);          
		private readonly Epidermis _bodySecondaryEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private HairFurColors _bodyHairColor = HairFurColors.BLACK; //stored in case someone wants to use it. i currently dont.

		//however, these are.
		//these are used for storage. they do not necessarily store the right type, but it's irrelevant as it's corrected by the properties.
		[Save]
		private readonly Epidermis _epidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		[Save]
		private readonly Epidermis _secondaryEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);

		//these will be the right type. use these instead.
		public Epidermis epidermis => type.parseEpidermis(_epidermis);
		public Epidermis secondaryEpidermis => type.parseSecondaryEpidermis(_secondaryEpidermis, _epidermis);

		protected Arms(ArmType type)
		{
			_type = type;
			hands = Hands.Generate(type.handType);
		}

		public override ArmType type
		{
			get => _type;
			protected set
			{
				_type = value;
				hands.UpdateHands(value.handType);
				UpdateEpidermisData(value);
			}
		}
		[Save]
		private ArmType _type;

		public static Arms GenerateDefault()
		{
			return new Arms(ArmType.HUMAN);
		}

		public static Arms GenerateDefaultOfType(ArmType type)
		{
			return new Arms(type);
		}

		public override bool Restore()
		{
			if (type == ArmType.HUMAN)
			{
				return false;
			}
			type = ArmType.HUMAN;
			return true;
		}

		public bool UpdateArms(ArmType armType)
		{
			if (type == armType)
			{
				return false;
			}
			type = armType;
			return true;
		}

		public bool UpdateArmsAndDisplayMessage(ArmType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateArms(newType);
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == ArmType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			type = ArmType.HUMAN;
			return true;
		}

		public void reactToChangeInSkinTone(object sender, ToneAwareEventArg e)
		{
			hands.reactToChangeInSkinTone(sender, e);
			_bodyEpidermis.UpdateTone(e.primaryTone);
			_bodyEpidermis.copyTo(_epidermis);

			_bodySecondaryEpidermis.UpdateTone(e.secondaryTone);
			if (!type.usesSecondaryTone || _bodySecondaryEpidermis.tone != Tones.NOT_APPLICABLE)
			{
				_secondaryEpidermis.UpdateTone(e.secondaryTone);
			}
		}

		public void reactToChangeInFurColor(object sender, FurAwareEventArg e)
		{
			_bodyEpidermis.UpdateFur(e.primaryColor);
			_bodySecondaryEpidermis.UpdateFur(e.secondaryColor);
			if (!_bodyEpidermis.fur.isNoFur() || !type.usesPrimaryFur)
			{
				_epidermis.UpdateFur(e.primaryColor);
			}
			else
			{
				_epidermis.UpdateFur(e.primaryColor);
			}
			if (!_bodySecondaryEpidermis.fur.isNoFur() || !type.usesSecondaryFur)
			{
				_secondaryEpidermis.UpdateFur(e.secondaryColor);
			}
		}

		//we have different data for body and arms - they are identical unless the body loses fur or "underbody", but the current type still uses it
		//of course, if a type changes, it may no longer need to remember the old fur color the body lost, nor may it need to remember the underbody. 
		//if this is the case, this function will resync the data. 
		private void UpdateEpidermisData(ArmType armType)
		{
			if (!armType.usesPrimaryFur)
			{
				_epidermis.UpdateFur(_bodyEpidermis.fur);
			}
			if (!armType.usesSecondaryTone)
			{
				_secondaryEpidermis.UpdateTone(_bodySecondaryEpidermis.tone);
			}
			if (!armType.usesSecondaryFur)
			{
				_secondaryEpidermis.UpdateFur(_bodySecondaryEpidermis.fur);
			}
		}

		public void reactToChangeInHairColor(object sender, HairColorEventArg e)
		{
			_bodyHairColor = e.hairColor;
		}

		protected Arms(SerializationInfo info, StreamingContext context)
		{
			_type = (ArmType)info.GetValue(nameof(_type), typeof(ArmType));
			hands = (Hands)info.GetValue(nameof(hands), typeof(Hands));
			_epidermis = (Epidermis)info.GetValue(nameof(_epidermis), typeof(Epidermis));
			_secondaryEpidermis = (Epidermis)info.GetValue(nameof(_secondaryEpidermis), typeof(Epidermis));
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(_type), _type, typeof(ArmType));
			info.AddValue(nameof(hands), hands, typeof(Hands));
			info.AddValue(nameof(_epidermis), _epidermis, typeof(Epidermis));
			info.AddValue(nameof(_secondaryEpidermis), _secondaryEpidermis, typeof(Epidermis));
		}
	}

	internal abstract partial class ArmType : BodyPartBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;
		private static List<ArmType> arms = new List<ArmType>();

		public readonly HandType handType;
		public readonly EpidermisType epidermisType;

		public abstract Epidermis parseEpidermis(Epidermis original);

		public virtual Epidermis parseSecondaryEpidermis(Epidermis secondary, Epidermis originalFallback)
		{
			epidermisHelper.Reset(epidermisType);
			return epidermisHelper;
		}
		public virtual bool usesSecondaryTone => false;
		public virtual bool usesSecondaryFur => false;
		public virtual bool usesPrimaryFur => this is FurArms;
		protected Epidermis epidermisHelper = Epidermis.GenerateDefault(EpidermisType.SKIN);

		public override int index => _index;
		private readonly int _index;

		protected ArmType(HandType hand, EpidermisType epidermis,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc,
			ChangeType<Arms> transform, RestoreType<Arms> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;
			epidermisType = epidermis;
			arms[_index] = this;
		}
		public static ArmType Deserialize(int index)
		{
			if (index < 0 || index >= arms.Count)
			{
				throw new System.ArgumentException("index for antennae type desrialize out of range");
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
					throw new System.ArgumentException("index for antennae type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}
		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ToneArms HUMAN = new ToneArms(HandType.HUMAN, EpidermisType.SKIN, Tones.HUMAN_DEFAULT, SkinTexture.NONDESCRIPT, true, HumanDescStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FurArms HARPY = new FurArms(HandType.HUMAN, EpidermisType.FEATHERS, FurColor.HARPY_DEFAULT, FurTexture.NONDESCRIPT, true, HarpyDescStr, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ToneArms SPIDER = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Tones.SPIDER_DEFAULT, SkinTexture.SHINY, false, SpiderDescStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ToneArms BEE = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Tones.BEE_DEFAULT, SkinTexture.SHINY, false, BeeDescStr, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ToneArms DRAGON = new ToneArms(HandType.DRAGON, EpidermisType.SCALES, Tones.DRAGON_DEFAULT, SkinTexture.NONDESCRIPT, true, DragonDescStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ToneArms IMP = new ToneArms(HandType.IMP, EpidermisType.SCALES, Tones.IMP_DEFAULT, SkinTexture.NONDESCRIPT, true, ImpDescStr, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ToneArms LIZARD = new ToneArms(HandType.LIZARD, EpidermisType.SCALES, Tones.LIZARD_DEFAULT, SkinTexture.NONDESCRIPT, true, LizardDescStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ToneArms SALAMANDER = new ToneArms(HandType.SALAMANDER, EpidermisType.SCALES, Tones.SALAMANDER_DEFAULT, SkinTexture.NONDESCRIPT, false, SalamanderDescStr, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly FurArms WOLF = new FurArms(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, FurTexture.NONDESCRIPT, true, WolfDescStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FurArms COCKATRICE = new CockatriceArms();
		public static readonly FurArms RED_PANDA = new FurArms(HandType.RED_PANDA, EpidermisType.FUR, FurColor.RED_PANDA_DEFAULT, FurTexture.NONDESCRIPT, false, RedPandaDescStr, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		public static readonly FurArms FERRET = new FerretArms();
		public static readonly FurArms CAT = new FurArms(HandType.CAT, EpidermisType.FUR, FurColor.CAT_DEFAULT, FurTexture.NONDESCRIPT, true, CatDescStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly FurArms DOG = new FurArms(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, FurTexture.NONDESCRIPT, true, DogDescStr, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FurArms FOX = new FurArms(HandType.FOX, EpidermisType.FUR, FurColor.CAT_DEFAULT, FurTexture.NONDESCRIPT, true, FoxDescStr, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//Add new Arm Types Here.

		private class FerretArms : FurArms
		{
			private readonly FurColor defaultSecondaryColor = FurColor.Generate(HairFurColors.BROWN, HairFurColors.BLACK, FurMulticolorPattern.MIXED);
			public FerretArms() : base(HandType.FERRET, EpidermisType.FUR, FurColor.FERRET_DEFAULT, FurTexture.NONDESCRIPT, true, FerretDescStr,
				FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr)
			{ }

			public override bool usesSecondaryFur => true;

			public override Epidermis parseSecondaryEpidermis(Epidermis secondary, Epidermis originalFallback)
			{
				if (secondary.type is FurBasedEpidermisType)
				{
					secondary.copyTo(epidermisHelper);
					epidermisHelper.UpdateEpidermis(epidermisType);
				}
				else
				{
					epidermisHelper.UpdateEpidermis(EpidermisType.FUR, defaultSecondaryColor);
				}
				return epidermisHelper;
			}
		}

		private class CockatriceArms : FurArms
		{
			public CockatriceArms() : base(HandType.COCKATRICE, EpidermisType.FUR, FurColor.COCKATRICE_DEFAULT, FurTexture.NONDESCRIPT, true,
				CockatriceDescStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
			{ }

			public override bool usesSecondaryTone => true;

			public override Epidermis parseSecondaryEpidermis(Epidermis secondary, Epidermis originalFallback)
			{
				//if we have the right underbody, use it.
				if (secondary.type == EpidermisType.SCALES)
				{
					secondary.copyTo(epidermisHelper);
					epidermisHelper.UpdateEpidermis(epidermisType);
				}
				//otherwise, fallback to the primary skin tone.
				else
				{
					originalFallback.copyTo(epidermisHelper);
					epidermisHelper.UpdateEpidermis(EpidermisType.SCALES);
				}
				return epidermisHelper;
			}
		}
		public bool isPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}
	}

	internal class FurArms : ArmType
	{
		public readonly FurColor defaultColor;
		public readonly FurTexture defaultTexture;
		protected readonly bool mutable;
		public FurArms(HandType hand, FurBasedEpidermisType epidermis, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultColor = FurColor.GenerateFromOther(defaultFurColor);
			defaultTexture = defaultFurTexture;
			mutable = canChange;
		}

		public override bool usesPrimaryFur => true;

		public override Epidermis parseEpidermis(Epidermis original)
		{
			if (mutable)
			{
				original.copyTo(epidermisHelper);
				epidermisHelper.UpdateEpidermis(epidermisType);
			}
			else
			{
				epidermisHelper.UpdateEpidermis((FurBasedEpidermisType)epidermisType, defaultColor);
			}
			return epidermisHelper;
		}
	}

	internal class ToneArms : ArmType
	{
		public readonly SkinTexture defaultTexture;
		public readonly bool mutable;
		public ToneArms(HandType hand, ToneBasedEpidermisType epidermis, Tones defaultTone, SkinTexture defaultSkinTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTexture = defaultSkinTexture;
			mutable = canChange;
		}

		public override Epidermis parseEpidermis(Epidermis original)
		{
			if (mutable)
			{
				original.copyTo(epidermisHelper);
				epidermisHelper.UpdateEpidermis(epidermisType);
			}
			else
			{
				epidermisHelper.UpdateEpidermis((ToneBasedEpidermisType)epidermisType, defaultTexture);
			}
			return epidermisHelper;
		}
	}
}
