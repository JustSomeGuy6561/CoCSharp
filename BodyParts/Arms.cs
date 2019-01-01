//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Tools;
using CoC.Items;

namespace CoC.BodyParts
{
	//TODO: add nice comparison shit for this class.
	class Arms : EpidermalBodyPart<Arms, ArmType>
	{
		public override ArmType type { get; protected set; }

		//really clean getter. niceuuu
		public override Epidermis epidermis => type.epidermis;

		public override bool attemptToTone(Tones tone)
		{
			return type.tryToTone(ref currentTone, tone) || base.attemptToTone(tone);
		}

		public override bool attemptToDye(Dyes dye)
		{
			return type.tryToDye(ref currentDye, dye) || base.attemptToDye(dye);
		}

		protected Arms(ArmType arm)
		{
			type = arm;
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
		}

		protected Arms(string desc, Hands handType, Epidermis skinType)
		{
			hands = handType;
			descriptor = desc;
			epidermis = skinType;
			index = indexMaker++;
		}
		*/

		public override void Restore()
		{
			this.type = ArmType.HUMAN;
			this.currentTone = Tones.HUMAN_DEFAULT;
			this.currentDye = Dyes.NO_FUR;
		}

	}
	class ArmType : EpidermalBodyPartBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;
		protected Hands hands;
		public Epidermis epidermis {get; protected set;}

		protected ArmType(Hands hnd, Epidermis skinType, GenericDescription shortDesc, CreatureDescription<Arms> creatureDesc,
			PlayerDescription<Arms> playerDesc, ChangeType<ArmType> fromType)
		{
			_index = indexMaker++;
			hands = hnd;
			epidermis = skinType;

			shortDescription = shortDesc;
			creatureDescription = creatureDesc;
			playerDescription = playerDesc;
			transformFrom = fromType;
		}

		public override int index => _index;
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

		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ArmType HUMAN = new ArmType(Hands.HUMAN, Epidermis.HUMAN, Arm);
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
