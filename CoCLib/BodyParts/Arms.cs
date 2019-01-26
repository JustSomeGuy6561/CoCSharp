//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Tools;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	//TODO: add nice comparison shit for this class.
	internal class Arms : BodyPartBase<Arms, ArmType>, IToneAware, IFurAware
	{
		public readonly Hands hands;
		public Epidermis epidermis => type.parseEpidermis(_epidermis);
		private readonly Epidermis _epidermis; //stores the data so that after a change in type, the new rules will apply to the correct data, not the old data. 
		public Epidermis secondaryEpidermis => type.parseSecondaryEpidermis(_secondaryEpidermis, _epidermis);
		private readonly Epidermis _secondaryEpidermis;
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
				epidermis.UpdateEpidermis(value.epidermisType);
			}
		}
		private ArmType _type;

		public static Arms GenerateDefault(ArmType type)
		{
			return new Arms(type);
		}

		public static Arms Generate(FurArms type, FurColor fur, FurTexture furTexture = FurTexture.NONDESCRIPT)
		{
			Arms retVal = new Arms(type);
			retVal.epidermis.UpdateEpidermis((FurBasedEpidermisType)retVal.epidermis.type, fur, furTexture);
			return retVal;
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

		public bool UpdateArms(FurArms furArms)
		{
			if (type == furArms)
			{
				return false;
			}
			type = furArms;
			//let the type do it. 
			return true;
		}
		public bool UpdateArms(ToneArms furArms)
		{
			if (type == furArms)
			{
				return false;
			}
			type = furArms;
			//let the type do it. 
			return true;
		}
		public bool UpdateArmsAndDisplayMessage(ToneArms newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateArms(newType);
		}

		public bool UpdateArmsAndDisplayMessage(FurArms newType, Player player)
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
			_epidermis.UpdateTone(e.primaryTone);
			_secondaryEpidermis.UpdateTone(e.secondaryTone);
		}

		public void reactToChangeInFurColor(object sender, FurAwareEventArg e)
		{
			_epidermis.UpdateFur(e.primaryColor);
			_secondaryEpidermis.UpdateFur(e.secondaryColor);
		}
	}

	internal abstract partial class ArmType : BodyPartBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;

		public readonly HandType handType;
		public readonly EpidermisType epidermisType;

		public abstract Epidermis parseEpidermis(Epidermis original);

		public virtual Epidermis parseSecondaryEpidermis(Epidermis secondary, Epidermis originalFallback)
		{
			epidermisHelper.Reset(epidermisType);
			return epidermisHelper;
		}

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
		}

		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ToneArms HUMAN = new ToneArms(HandType.HUMAN, EpidermisType.SKIN, Tones.HUMAN_DEFAULT, SkinTexture.NONDESCRIPT, true, HumanDescStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FurArms HARPY = new FurArms(HandType.HUMAN, EpidermisType.FEATHERS, FurColor.HARPY_DEFAULT, FurTexture.NONDESCRIPT, true, HarpyDescStr, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ToneArms SPIDER = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Tones.BLACK, SkinTexture.SHINY, false, SpiderDescStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ToneArms BEE = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Tones.BLACK, SkinTexture.SHINY, false, BeeDescStr, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ToneArms DRAGON = new ToneArms(HandType.DRAGON, EpidermisType.SCALES, Tones.DRAGON_DEFAULT, SkinTexture.NONDESCRIPT, true, DragonDescStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ToneArms IMP = new ToneArms(HandType.IMP, EpidermisType.SCALES, Tones.IMP_DEFAULT, SkinTexture.NONDESCRIPT, true, ImpDescStr, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ToneArms LIZARD = new ToneArms(HandType.LIZARD, EpidermisType.SCALES, Tones.LIZARD_DEFAULT, SkinTexture.NONDESCRIPT, true, LizardDescStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ToneArms SALAMANDER = new ToneArms(HandType.SALAMANDER, EpidermisType.SCALES, Tones.DARK_RED, SkinTexture.NONDESCRIPT, false, SalamanderDescStr, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly FurArms WOLF = new FurArms(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, FurTexture.NONDESCRIPT, true, WolfDescStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FurArms COCKATRICE = new CockatriceArms();
		public static readonly FurArms RED_PANDA = new FurArms(HandType.RED_PANDA, EpidermisType.FUR, FurColor.RED_PANDA_DEFAULT, FurTexture.NONDESCRIPT, true, RedPandaDescStr, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
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
