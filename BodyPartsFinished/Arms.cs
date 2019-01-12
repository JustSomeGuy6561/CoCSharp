//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Tools;
using CoC.EpidermalColors;
using System.Collections.Generic;
using static CoC.Strings.BodyParts.ArmsStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	//TODO: add nice comparison shit for this class.
	public class Arms : EpidermalBodyPart<Arms, ArmType>
	{
		public readonly Hands hands;
		public Arms(ArmType type) : base(type.epidermisType, type.defaultTone, type.defaultFurColor)
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
			retVal.reactToChangeInFurColor(fur);
			retVal.reactToChangeInSkinTone(tones);
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
			//set the epidermis to the default, 
			epidermis.UpdateEpidermis(type.epidermisType, type.defaultTone, type.defaultFurColor, type.defaultEpidermisAdjective);
			//then try to update the color passed in.
			reactToChangeInFurColor(currentHairFurColor);
			reactToChangeInSkinTone(currentTone);
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
			OutputText(restoreString(this, player));
			type = ArmType.HUMAN;
			return true;
		}

		public override void reactToChangeInSkinTone(Tones newTone)
		{
			hands.reactToChangeInSkinTone(newTone);
			if (type.epidermisCanChangeTone)
			{
				base.reactToChangeInSkinTone(newTone);
			}
		}

		public override void reactToChangeInFurColor(FurColor furColor)
		{
			if (type.epidermisCanChangeFur)
			{
				base.reactToChangeInFurColor(furColor);
			}
		}
	}

	public class ArmType : EpidermalBodyPartBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;

		public readonly HandType handType;

		protected readonly string adjective = "";
		private readonly int _index;
		public readonly Tones defaultTone;
		public readonly FurColor defaultFurColor = FurColor.GenerateEmpty();
		public readonly string defaultEpidermisAdjective;
		public override int index => _index;
		public readonly bool epidermisCanChangeFur;
		public readonly bool epidermisCanChangeTone;

		protected ArmType(HandType hand, EpidermisType epidermis, Tones defTone, bool canChange, string epidermisAdjective,
			GenericDescription shortDesc, FullDescription<Arms> fullDesc, PlayerDescription<Arms> playerDesc,
			ChangeType<Arms> transform, ChangeType<Arms> restore) : base(epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;
			defaultTone = defTone;
			defaultFurColor.Reset();
			epidermisCanChangeFur = false;
			epidermisCanChangeTone = canChange;
			defaultEpidermisAdjective = epidermisAdjective;
		}

		protected ArmType(HandType hand, EpidermisType epidermis, FurColor defaultFur, bool canChange, string epidermisAdjective,
			GenericDescription shortDesc, FullDescription<Arms> fullDesc, PlayerDescription<Arms> playerDesc,
			ChangeType<Arms> transform, ChangeType<Arms> restore) : base(epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;
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
		public static readonly ArmType RED_PANDA = new ArmType(HandType.RED_PANDA, EpidermisType.FUR, FurColor.RED_PANDA_DEFAULT, true, "", Red_pandaDescStr, Red_PandaFullDesc, Red_PandaPlayerStr, Red_PandaTransformStr, Red_PandaRestoreStr);
		public static readonly ArmType FERRET = new ArmType(HandType.FERRET, EpidermisType.FUR, FurColor.FERRET_DEFAULT, true, "", FerretDescStr, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly ArmType CAT = new ArmType(HandType.CAT, EpidermisType.FUR, FurColor.CAT_DEFAULT, true, "", CatDescStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly ArmType DOG = new ArmType(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, true, "", DogDescStr, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly ArmType FOX = new ArmType(HandType.FOX, EpidermisType.FUR, FurColor.CAT_DEFAULT, true, "", FoxDescStr, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//Add new Arm Types Here.

		public bool isPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}
	}
}
