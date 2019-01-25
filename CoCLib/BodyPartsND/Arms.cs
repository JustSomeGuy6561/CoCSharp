//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Tools;
using CoC.EpidermalColors;
using static CoC.UI.TextOutput;
using CoC.BodyParts.SpecialInteraction;
using CoC.Creatures;

namespace CoC.BodyParts
{
	//TODO: add nice comparison shit for this class.
	internal class Arms : BodyPartBase<Arms, ArmType>, IToneAware, IFurAware
	{
		public readonly Hands hands;
		public Epidermis epidermis { get; protected set; }
		protected Arms(ArmType type)
		{
			_type = type;
			hands = Hands.Generate(type.handType, type.defaultTone);
		}

		public override ArmType type
		{
			get => _type;
			protected set
			{
				_type = value;
				hands.UpdateHands(value.handType);
				//keep the epidermis up to date.
				if (epidermis.type != value.epidermisType)
				{
					epidermis.UpdateEpidermis(value.epidermisType, value.defaultTone, value.defaultFurColor, value.defaultEpidermisAdjective);
				}
			}
		}
		private ArmType _type;

		public static Arms Generate(ArmType type)
		{
			return new Arms(type);
		}

		public static Arms Generate(ArmType type, Tones tones, FurColor fur)
		{
			Arms retVal = new Arms(type);
			retVal.type.UpdateEpidermis(retVal.epidermis, tones, true, Tones.NOT_APPLICABLE, fur, null);
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

		public bool UpdateArms(ArmType newType, Tones currentTone, FurColor currentHairFurColor)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			//let the type do it. 
			type.UpdateEpidermis(epidermis, currentTone, true, Tones.NOT_APPLICABLE, currentHairFurColor, null);
			return true;
		}

		public bool UpdateArmsAndDisplayMessage(ArmType newType, Tones currentTone, FurColor currentHairFurColor, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateArms(newType, currentTone, currentHairFurColor);
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
			type.UpdateEpidermis(epidermis, e.primaryTone, e.primaryToneActive, e.secondaryTone, null, null);
		}

		public void reactToChangeInFurColor(object sender, FurAwareEventArg e)
		{
			type.UpdateEpidermis(epidermis, Tones.NOT_APPLICABLE, false, Tones.NOT_APPLICABLE, e.primaryColor, e.secondaryColor);
		}
	}

	internal partial class ArmType : BodyPartBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;

		public readonly HandType handType;
		public readonly EpidermisType epidermisType;
		protected readonly string adjective = "";
		private readonly int _index;
		public readonly Tones defaultTone;
		public readonly FurColor defaultFurColor = FurColor.GenerateEmpty();
		public readonly string defaultEpidermisAdjective;
		public override int index => _index;
		public readonly bool epidermisCanChangeFur;
		public readonly bool epidermisCanChangeTone;

		//we assume that they use the primary color. not all do.
		public virtual void UpdateEpidermis(Epidermis epidermis, Tones primaryTone, bool primaryToneActive, Tones secondaryTone, FurColor primaryFurColor, FurColor secondaryFurColor)
		{
			//basically, if we can change it and it's valid.
			if (epidermisCanChangeFur && epidermisType.usesFur && primaryFurColor != null && !primaryFurColor.isNoFur())
			{
				epidermis.UpdateFur(primaryFurColor);
			}
			else if (epidermisCanChangeTone && epidermisType.usesTone && primaryTone != null && primaryTone != Tones.NOT_APPLICABLE && primaryToneActive)
			{
				epidermis.UpdateTone(primaryTone);
			}
		}

		protected ArmType(HandType hand, EpidermisType epidermis, Tones defTone, bool canChange, string epidermisAdjective,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc,
			ChangeType<Arms> transform, RestoreType<Arms> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;
			epidermisType = epidermis;
			defaultTone = defTone;
			defaultFurColor.Reset();
			epidermisCanChangeFur = false;
			epidermisCanChangeTone = canChange;
			defaultEpidermisAdjective = epidermisAdjective;
		}

		protected ArmType(HandType hand, EpidermisType epidermis, FurColor defaultFur, bool canChange, string epidermisAdjective,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc,
			ChangeType<Arms> transform, RestoreType<Arms> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;
			epidermisType = epidermis;
			defaultTone = Tones.NOT_APPLICABLE;
			defaultFurColor.UpdateFurColor(defaultFur);
			epidermisCanChangeTone = false;
			epidermisCanChangeFur = canChange;
			defaultEpidermisAdjective = epidermisAdjective;
		}


		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ArmType HUMAN = new ArmType(HandType.HUMAN, EpidermisType.SKIN, Tones.HUMAN_DEFAULT, true, "smooth", HumanDescStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly ArmType HARPY = new ArmType(HandType.HUMAN, EpidermisType.FEATHERS, FurColor.HARPY_DEFAULT, true, "", HarpyDescStr, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ArmType SPIDER = new ArmType(HandType.HUMAN, EpidermisType.CARAPACE, Tones.BLACK, false, "", SpiderDescStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ArmType BEE = new ArmType(HandType.HUMAN, EpidermisType.FUR, Tones.BLACK, false, "shiny", BeeDescStr, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ArmType DRAGON = new ArmType(HandType.DRAGON, EpidermisType.SCALES, Tones.DRAGON_DEFAULT, true, "", DragonDescStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ArmType IMP = new ArmType(HandType.IMP, EpidermisType.SCALES, Tones.IMP_DEFAULT, true, "", ImpDescStr, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ArmType LIZARD = new ArmType(HandType.LIZARD, EpidermisType.SCALES, Tones.LIZARD_DEFAULT, true, "", LizardDescStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ArmType SALAMANDER = new ArmType(HandType.SALAMANDER, EpidermisType.SCALES, Tones.DARK_RED, false, "", SalamanderDescStr, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly ArmType WOLF = new ArmType(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, true, "", WolfDescStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly ArmType COCKATRICE = new ArmType(HandType.COCKATRICE, EpidermisType.SCALES, Tones.COCKATRICE_DEFAULT, true, "", CockatriceDescStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly ArmType RED_PANDA = new RedPandaArms();
		public static readonly ArmType FERRET = new FerretArms();
		public static readonly ArmType CAT = new ArmType(HandType.CAT, EpidermisType.FUR, FurColor.CAT_DEFAULT, true, "", CatDescStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly ArmType DOG = new ArmType(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, true, "", DogDescStr, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly ArmType FOX = new ArmType(HandType.FOX, EpidermisType.FUR, FurColor.CAT_DEFAULT, true, "", FoxDescStr, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//Add new Arm Types Here.

		private class FerretArms : ArmType
		{
			public FerretArms() : base(HandType.FERRET, EpidermisType.FUR, FurColor.FERRET_DEFAULT, true, "", FerretDescStr, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr) {}

			public override void UpdateEpidermis(Epidermis epidermis, Tones primaryTone, bool primaryToneActive, Tones secondaryTone, FurColor primaryFurColor, FurColor secondaryFurColor)
			{
				if (secondaryFurColor != null && !secondaryFurColor.isNoFur())
				{
					epidermis.UpdateFur(secondaryFurColor);
				}
			}
		}

		private class RedPandaArms : ArmType
		{
			public RedPandaArms() : base(HandType.RED_PANDA, EpidermisType.FUR, FurColor.RED_PANDA_DEFAULT, true, "", RedPandaDescStr, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr) { }

			public override void UpdateEpidermis(Epidermis epidermis, Tones primaryTone, bool primaryToneActive, Tones secondaryTone, FurColor primaryFurColor, FurColor secondaryFurColor)
			{
				if (secondaryFurColor != null && !secondaryFurColor.isNoFur())
				{
					epidermis.UpdateFur(secondaryFurColor);
				}
			}
		}
		public bool isPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}
	}
}
