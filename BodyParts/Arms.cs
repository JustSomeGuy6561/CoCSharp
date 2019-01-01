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
		public override ArmType type { get; protected set; }

		//really clean getter. niceuuu
		public override Epidermis epidermis => type.epidermis;

		public override bool attemptToTone(Tones tone)
		{
			return type.tryToTone(ref currentTone, tone);
		}

		public override bool attemptToDye(Dyes dye)
		{
			return type.tryToDye(ref currentDye, dye);
		}

		protected Arms(ArmType arm)
		{
			type = arm;
			currentTone = Tones.HUMAN_DEFAULT;
			currentDye = Dyes.NO_FUR;
		}

		public static Arms Generate(ArmType type, Tones currentSkinTone, Dyes currentHairOrFurColor)
		{
			Arms retVal = new Arms(type);
			//try to apply the tone and dye. 
			retVal.attemptToDye(currentHairOrFurColor);
			retVal.attemptToTone(currentSkinTone);
			return retVal;
		}

		/*

		public override string GetDescriptor()
		{
			string retVal = epidermis.GetDescriptor();
			retVal = String.IsNullOrWhiteSpace(retVal) ? "" : retVal + " ";
			retVal += descriptor + " arms";
			return retVal;
		}
		public string GetDescriptorWithHands()
		{
			return GetDescriptor() + " and " + hands.GetDescriptor();
		}*/

		public override void Restore()
		{
			type = ArmType.HUMAN;
			currentTone = Tones.HUMAN_DEFAULT;
			currentDye = Dyes.NO_FUR;
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
			return first.type == second.type && first.usesDye == second.usesDye && ((first.usesDye && first.currentDye == second.currentDye) || !first.usesDye) &&  first.usesTone == second.usesTone && ((first.usesTone && first.currentTone == second.currentTone) || !first.usesTone);
		}

		public static bool operator !=(Arms first, Arms second)
		{
			return first.type != second.type || first.usesDye != second.usesDye || first.usesTone != second.usesTone || (first.usesTone && first.currentTone != second.currentTone) || (first.usesDye && first.currentDye != second.currentDye);
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
		public Hands hands { get; protected set; }

		public override Epidermis epidermis => _epidermis;
		private readonly Epidermis _epidermis;

		protected ArmType(Hands hnd, Epidermis skinType, string epiderisAdjective, GenericDescription shortDesc, CreatureDescription<Arms> creatureDesc,
			PlayerDescription<Arms> playerDesc, ChangeType<ArmType> fromType)
		{
			_index = indexMaker++;
			hands = hnd;
			_epidermis = skinType;
			adjective = epiderisAdjective;
			shortDescription = shortDesc;
			creatureDescription = creatureDesc;
			playerDescription = playerDesc;
			transformFrom = fromType;
		}

		public override int index => _index;
		protected readonly string adjective = "";
		private readonly int _index;
		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription<Arms> creatureDescription {get; protected set;}
		public override PlayerDescription<Arms> playerDescription {get; protected set;}
		public override ChangeType<ArmType> transformFrom {get; protected set;}

		public override bool canTone()
		{
			return epidermis.canTone() || hands.canTone();
		}

		public override bool tryToTone(ref Tones currentTone, Tones newTone)
		{
			//first try the hands.
			if (hands.canTone())
			{
				//but only return if they succeed.
				if (hands.tryToTone(ref currentTone, newTone))
				{
					return true;
				}
			}
			//if they can't or they fail, fallback to the epidermis.
			if (epidermis.canTone())
			{
				return (epidermis.tryToTone(ref currentTone, newTone));
			}
			//if they all fail return false
			return false;
		}

		public override bool canDye()
		{
			return epidermis.canDye();
		}

		public override bool tryToDye(ref Dyes currentColor, Dyes newColor)
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
		public static readonly ArmType HUMAN = new ArmType(Hands.HUMAN, Epidermis.SKIN, "", );
		public static readonly ArmType HARPY = new ArmType(Hands.HUMAN, Epidermis.FEATHERS);
		public static readonly ArmType SPIDER = new ArmType(Hands.HUMAN, Epidermis.CARAPACE);
		public static readonly ArmType BEE = new ArmType(Hands.HUMAN, Epidermis.FUR);
		public static readonly ArmType DRAGON = new ArmType(Hands.DRAGON, Epidermis.SCALES); //Custom?
		public static readonly ArmType IMP = new ArmType(Hands.IMP, Epidermis.SCALES);
		public static readonly ArmType LIZARD = new ArmType(Hands.LIZARD, Epidermis.SCALES); //Custom?
		public static readonly ArmType SALAMANDER = new ArmType(Hands.SALAMANDER, Epidermis.SCALES); //Custom?
		public static readonly ArmType WOLF = new ArmType(Hands.DOG, Epidermis.FUR); //Custom. Updates fur. 
		public static readonly ArmType COCKATRICE = new ArmType(Hands.COCKATRICE, Epidermis.SCALES); 
		public static readonly ArmType RED_PANDA = new ArmType(Hands.RED_PANDA, Epidermis.FUR); // Custom
		public static readonly ArmType FERRET = new ArmType(Hands.FERRET, Epidermis.FUR);
		public static readonly ArmType CAT = new ArmType(Hands.CAT, Epidermis.FUR);
		public static readonly ArmType DOG = new ArmType(Hands.DOG, Epidermis.FUR);
		public static readonly ArmType FOX = new ArmType(Hands.FOX, Epidermis.FUR);
		//Add new Arm Types Here.

	}
}
