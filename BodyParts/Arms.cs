//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Tools;
using CoC.Items;
using System.Collections.Generic;
using static CoC.Strings.BodyParts.ArmsStrings;

namespace CoC.BodyParts
{
	//TODO: add nice comparison shit for this class.
	public class Arms : EpidermalBodyPart<Arms, ArmType>
	{
		protected Arms(ArmType arm)
		{
			type = arm;
			currentTone = arm.DefaultTone;
			currentFur = HairFurColors.NO_HAIR_FUR;
		}
		public override ArmType type { get; protected set; }

		//really clean getter. niceuuu
		public override Epidermis epidermis => type.epidermis;

		public override bool attemptToTone(Tones tone)
		{
			return type.tryToTone(ref currentTone, tone);
		}

		public override bool attemptToDye(HairFurColors dye)
		{
			return type.tryToDye(ref currentFur, dye);
		}



		public static Arms Generate(ArmType type, Tones currentSkinTone, HairFurColors currentHairOrFurColor)
		{
			Arms retVal = new Arms(type);
			//try to apply the tone and dye. 
			retVal.attemptToDye(currentHairOrFurColor);
			retVal.attemptToTone(currentSkinTone);
			return retVal;
		}

		public override void Restore()
		{
			type = ArmType.HUMAN;
			currentTone = Tones.HUMAN_DEFAULT;
			currentFur = HairFurColors.NO_HAIR_FUR;
		}

		//Updates the arms. Since arms cannot be dyed individually (that would just be an extension of the body color)
		//requires the current fur color (hair color is to be used if no hair is available) and current skin tone.
		//will update the arm color or tone based on internal ruleset, if applicable. generally this is the same value(s) passed in.
		public bool UpdateArms(ArmType newType, HairFurColors currentHairFurColor, Tones currentTone)
		{

		}

		//used to notify the arms that there was a change in colors or tones. 
		//internally, sets the color for the arms based on the designed behavior
		//(generally, this means it will be the same value passed in, if applicable).
		public bool NotifyHairSkinFurColorChange(HairFurColors color, Tones currentTone)
		{

		}

		#region CompareConvenience
		//Because of the convenience shit. Standard compares that need to be explicitly defined because
		//the non-standard ones are too.
		public bool Equals(Arms other)
		{
			return this == other;
		}

		public static bool operator ==(Arms first, Arms second)
		{
			return first.type == second.type && first.usesDye == second.usesDye && ((first.usesDye && first.currentFur == second.currentFur) || !first.usesDye) &&  first.usesTone == second.usesTone && ((first.usesTone && first.currentTone == second.currentTone) || !first.usesTone);
		}

		public static bool operator !=(Arms first, Arms second)
		{
			return first.type != second.type || first.usesDye != second.usesDye || first.usesTone != second.usesTone || (first.usesTone && first.currentTone != second.currentTone) || (first.usesDye && first.currentFur != second.currentFur);
		}

		//Convenience. Because everyone loves that shit
		public bool Equals(ArmType other)
		{
			return type == other;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
			{
				return false;
			}
			Arms arm = (Arms)obj;
			return Equals(arm);
		}

		//default implementation. i ain't touching that lol.
		//note that this will actually cause overlaps if the dye and tone isn't the same.
		//the equals operators will take care of that. it's far too easy to accidently 
		//create false negatives in a hash, because they may not be equal, but not active, which wouldn't matter for equality.
		public override int GetHashCode()
		{
			return 34944597 + EqualityComparer<ArmType>.Default.GetHashCode(type);
		}

		public static bool operator ==(Arms first, ArmType second)
		{
			return first.type == second;
		}

		public static bool operator !=(Arms first, ArmType second)
		{
			return first.type != second;
		}
		#endregion

	}
	public class ArmType : EpidermalBodyPartBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;

		public readonly HandType hands;

		private readonly Epidermis _epidermis;
		protected readonly string adjective = "";
		private readonly int _index;
		public readonly Tones defaultTone;
		public readonly HairFurColors defaultFurColor;


		protected ArmType(Hands hnd, Epidermis skinType, string epiderisAdjective, Tones defaultSkinScaleTone, HairFurColors defaultFurCol, GenericDescription shortDesc, 
			CreatureDescription<Arms> creatureDesc, PlayerDescription<Arms> playerDesc, ChangeType<ArmType> fromType, ChangeType<ArmType> revertToDefault)
		{
			_index = indexMaker++;
			hands = hnd;
			_epidermis = skinType;
			adjective = epiderisAdjective;
			defaultTone = defaultSkinScaleTone;
			defaultFurColor = defaultFurCol;
		
			shortDescription = shortDesc;
			creatureDescription = creatureDesc;
			playerDescription = playerDesc;
			transformFrom = fromType;
			restoreString = revertToDefault;
		}

		public override int index => _index;
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

		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ArmType HUMAN = new ArmType(HandType.HUMAN, Epidermis.SKIN, "smooth", );
		public static readonly ArmType HARPY = new ArmType(HandType.HUMAN, Epidermis.FEATHERS);
		public static readonly ArmType SPIDER = new ArmType(HandType.HUMAN, Epidermis.CARAPACE);
		public static readonly ArmType BEE = new ArmType(HandType.HUMAN, Epidermis.FUR, "shiny",); //MUST OVERRIDE. It uses black exoskeleton which requires tone. 
		//I split the predator arms so i could have one handtype per armtype. no functionality has been lost.
		//isPredatorArms has been added if you still need that check. 
		public static readonly ArmType DRAGON = new ArmType(HandType.DRAGON, Epidermis.SCALES); //Custom?
		public static readonly ArmType IMP = new ArmType(HandType.IMP, Epidermis.SCALES);
		public static readonly ArmType LIZARD = new ArmType(HandType.LIZARD, Epidermis.SCALES); //Custom?
		public static readonly ArmType SALAMANDER = new ArmType(HandType.SALAMANDER, Epidermis.SCALES); //Custom?
		public static readonly ArmType WOLF = new ArmType(HandType.DOG, Epidermis.FUR); //Custom. Updates fur. 
		public static readonly ArmType COCKATRICE = new ArmType(HandType.COCKATRICE, Epidermis.SCALES); 
		public static readonly ArmType RED_PANDA = new ArmType(HandType.RED_PANDA, Epidermis.FUR); // Custom
		public static readonly ArmType FERRET = new ArmType(HandType.FERRET, Epidermis.FUR);
		public static readonly ArmType CAT = new ArmType(HandType.CAT, Epidermis.FUR);
		public static readonly ArmType DOG = new ArmType(HandType.DOG, Epidermis.FUR);
		public static readonly ArmType FOX = new ArmType(HandType.FOX, Epidermis.FUR);
		//Add new Arm Types Here.

		public bool isPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}
	}
}
