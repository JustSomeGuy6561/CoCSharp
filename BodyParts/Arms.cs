//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Tools;
using CoC.EpidermalColors;
using System.Collections.Generic;
using static CoC.Strings.BodyParts.ArmsStrings;

namespace CoC.BodyParts
{
	//TODO: add nice comparison shit for this class.
	public class Arms : EpidermalBodyPart<Arms, ArmType>
	{
		public readonly Hands hands;
		public Arms(ArmType type, Tones currentTone) : base(type.epidermisType, currentTone)
		{
			_type = type;
			hands = Hands.Generate(type.handType, currentTone);
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

		//If i see generate used to change arm types, i will cry. 
		//please, please don't make me cry.

		public static Arms Generate(ArmType type, Tones currentSkinTone)
		{
			return new Arms(type, currentSkinTone);
		}

		public override void Restore()
		{
			type = ArmType.HUMAN;
		}

		public bool UpdateArms(ArmType newType, FurColor currentHairFurColor, Tones currentTone)
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

		public bool UpdateArmsAndDisplayMessage(ArmType newType, FurColor currentHairFurColor, Tones currentTone, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			transformFrom(this, player);
			return UpdateArms(newType, currentHairFurColor, currentTone);
		}

		public override void RestoreAndDisplayMessage(Player player)
		{
			restoreString(this, player);
			type = ArmType.HUMAN;
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

		public readonly EpidermisType epidermisType;
		protected readonly string adjective = "";
		private readonly int _index;
		public readonly Tones defaultTone;
		public readonly FurColor defaultFurColor;
		public readonly string defaultEpidermisAdjective;
		public override int index => _index;
		public readonly bool epidermisCanChangeFur;
		public readonly bool epidermisCanChangeTone;

		protected ArmType(HandType hand, EpidermisType epidermis, Tones defTone, bool canChange, string epidermisAdjective,
			GenericDescription shortDesc, CreatureDescription<Arms> creatureDesc, PlayerDescription<Arms> playerDesc,
			ChangeType<Arms> transform, ChangeType<Arms> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = epidermis;
			handType = hand;
			defaultTone = defTone;
			defaultFurColor = FurColor.GenerateEmpty();
			epidermisCanChangeFur = false;
			epidermisCanChangeTone = canChange;
			defaultEpidermisAdjective = epidermisAdjective;

		}

		protected ArmType(HandType hand, EpidermisType epidermis, FurColor defaultFur, bool canChange, string epidermisAdjective,
			GenericDescription shortDesc, CreatureDescription<Arms> creatureDesc, PlayerDescription<Arms> playerDesc,
			ChangeType<Arms> transform, ChangeType<Arms> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = epidermis;
			handType = hand;
			defaultTone = Tones.NOT_APPLICABLE;
			defaultFurColor = defaultFur;
			epidermisCanChangeTone = false;
			epidermisCanChangeFur = canChange;
			defaultEpidermisAdjective = epidermisAdjective;
		}




		/*
		public override Epidermis epidermis => _epidermis;

		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription<Arms> creatureDescription {get; protected set;}
		public override PlayerDescription<Arms> playerDescription {get; protected set;}
		public override ChangeType<ArmType> transformFrom {get; protected set;}
		public override ChangeType<ArmType> restoreString { get; protected set; }

		public override bool canTone()
		{
			return epidermis.canTone() || hands.canTone();
		}

		public override bool tryToTone(ref Tones currentTone, Tones newTone)
		{
			bool retVal = false;
			//first try the hands.
			if (hands.canTone())
			{
				//mark flag if they succeed
				if (hands.tryToTone(ref currentTone, newTone))
				{
					retVal = true;
				}
			}
			//then do epidermis.
			if (epidermis.canTone())
			{
				if (epidermis.tryToTone(ref currentTone, newTone))
				{
					retVal = true;
				}
			}
			//if they all fail return false
			return retVal;
		}

		public override bool canDye()
		{
			return epidermis.canDye();
		}

		public override bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
		{
			if (epidermis.canDye())
			{
				return epidermis.tryToDye(ref currentColor, newColor);
			}
			return false;
		}

		public override string defaultEpidermalAdjective()
		{
			return adjective;
		}
		*/
		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ArmType HUMAN = new ArmType(HandType.HUMAN, EpidermisType.SKIN, Tones.HUMAN_DEFAULT, true, "smooth", HumanDescStr, HumanCreatureStr, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly ArmType HARPY = new ArmType(HandType.HUMAN, EpidermisType.FEATHERS, FurColor.HARPY_DEFAULT, true, "", HarpyDescStr, HarpyCreatureStr, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ArmType SPIDER = new ArmType(HandType.HUMAN, EpidermisType.CARAPACE, Tones.BLACK, false, "", SpiderDescStr, SpiderCreatureStr, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ArmType BEE = new ArmType(HandType.HUMAN, EpidermisType.FUR, Tones.BLACK, false, "shiny", BeeDescStr, BeeCreatureStr, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ArmType DRAGON = new ArmType(HandType.DRAGON, EpidermisType.SCALES, Tones.DRAGON_DEFAULT, true, "", DragonDescStr, DragonCreatureStr, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ArmType IMP = new ArmType(HandType.IMP, EpidermisType.SCALES, Tones.IMP_DEFAULT, true, "", ImpDescStr, ImpCreatureStr, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ArmType LIZARD = new ArmType(HandType.LIZARD, EpidermisType.SCALES, Tones.LIZARD_DEFAULT, true, "", LizardDescStr, LizardCreatureStr, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ArmType SALAMANDER = new ArmType(HandType.SALAMANDER, EpidermisType.SCALES, Tones.DARK_RED, false, "", SalamanderDescStr, SalamanderCreatureStr, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly ArmType WOLF = new ArmType(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, true, "", WolfDescStr, WolfCreatureStr, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly ArmType COCKATRICE = new ArmType(HandType.COCKATRICE, EpidermisType.SCALES, Tones.COCKATRICE_DEFAULT, true, "", CockatriceDescStr, CockatriceCreatureStr, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly ArmType RED_PANDA = new ArmType(HandType.RED_PANDA, EpidermisType.FUR, FurColor.RED_PANDA_DEFAULT, true, "", Red_pandaDescStr, Red_PandaCreatureStr, Red_PandaPlayerStr, Red_PandaTransformStr, Red_PandaRestoreStr);
		public static readonly ArmType FERRET = new ArmType(HandType.FERRET, EpidermisType.FUR, FurColor.FERRET_DEFAULT, true, "", FerretDescStr, FerretCreatureStr, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly ArmType CAT = new ArmType(HandType.CAT, EpidermisType.FUR, FurColor.CAT_DEFAULT, true, "", CatDescStr, CatCreatureStr, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly ArmType DOG = new ArmType(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, true, "", DogDescStr, DogCreatureStr, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly ArmType FOX = new ArmType(HandType.FOX, EpidermisType.FUR, FurColor.CAT_DEFAULT, true, "", FoxDescStr, FoxCreatureStr, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//Add new Arm Types Here.

		public bool isPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}
	}
}
