//Hands.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

using CoC.Backend.CoC_Colors;

namespace CoC.Backend.BodyParts
{
	public class Hands : SimpleBodyPart<HandType>
	{
		public override HandType type { get; protected set; }

		protected Hands(HandType handType, Tones currentTone)
		{
			type = handType;
			this.clawTone = currentTone;
		}

		public SimpleDescriptor fullDescription => () => type.fullDescription(this);

		public static Hands Generate(HandType handType)
		{
			return new Hands(handType, Tones.IVORY);
		}
		public static Hands Generate(HandType handType, Tones currentTone)
		{
			return new Hands(handType, currentTone);
		}

		public void UpdateHands(HandType newType)
		{
			type = newType;
			if (!type.canTone())
			{
				clawTone = Tones.NOT_APPLICABLE;
			}
		}

		public Tones clawTone { get; protected set; }

		public void reactToChangeInSkinTone(Tones primary, Tones secondary)
		{
			if (type.canTone())
			{
				Tones claw = clawTone;
				type.tryToTone(ref claw, primary, secondary);
				clawTone = claw;
			}
		}
	}

	public partial class HandType : SimpleBodyPartType
	{
		private static int indexMaker = 0;

		protected enum HandStyle { HANDS, CLAWS, PAWS /*, OTHER*/}

		public DescriptorWithArg<Hands> fullDescription;
		public override int index => _index;
		protected readonly int _index;

		public virtual bool canTone()
		{
			return false;
		}
		public bool isClaws => handStyle == HandStyle.CLAWS;
		public bool isPaws => handStyle == HandStyle.PAWS;
		public bool isHands => handStyle == HandStyle.HANDS;
		//default case. never procs, though that may change in the future, idk.
		public bool isOther => !(isClaws || isHands || isPaws);

		protected readonly HandStyle handStyle;
		public virtual bool tryToTone(ref Tones currentTone, Tones primaryTone, Tones secondaryTone)
		{
			if (canTone())
			{
				currentTone = primaryTone;
				return currentTone == primaryTone;
			}
			return false;
		}

		private protected HandType(HandStyle style, SimpleDescriptor shortDesc, DescriptorWithArg<Hands> fullDesc) : base(shortDesc)
		{
			_index = indexMaker++;
			fullDescription = fullDesc;
			handStyle = style;
		}

		public static readonly HandType HUMAN = new HandType(HandStyle.HANDS, HumanShort, HumanFullDesc);
		public static readonly HandType LIZARD = new LizardClaws();
		public static readonly HandType DRAGON = new HandType(HandStyle.CLAWS, DragonShort, DragonFullDesc);
		public static readonly HandType SALAMANDER = new HandType(HandStyle.CLAWS, SalamanderShort, SalamanderFullDesc);
		public static readonly HandType CAT = new HandType(HandStyle.PAWS, CatShort, CatFullDesc);
		public static readonly HandType DOG = new HandType(HandStyle.PAWS, DogShort, DogFullDesc);
		public static readonly HandType FOX = new HandType(HandStyle.PAWS, FoxShort, FoxFullDesc);
		public static readonly HandType IMP = new ImpClaws();
		public static readonly HandType COCKATRICE = new HandType(HandStyle.CLAWS, CockatriceShort, CockatriceFullDesc);
		public static readonly HandType RED_PANDA = new HandType(HandStyle.PAWS, RedPandaShort, RedPandaFullDesc);
		public static readonly HandType FERRET = new HandType(HandStyle.PAWS, FerretShort, FerretFullDesc);
		//public static readonly Hands MANTIS = new Hands(HandStyle.OTHER, MantisShort MantisFullDesc); //Not even remotely implemented.

		private class LizardClaws : HandType
		{

			public LizardClaws() : base(HandStyle.CLAWS, LizardShort, LizardFullDesc) { }

			public override bool canTone()
			{
				return true;
			}

			public override bool tryToTone(ref Tones currentTone, Tones primaryTone, Tones secondaryTone)
			{
				//do some magic to the tone to make it lizard claw compatible
				currentTone = primaryTone;
				//maybe implement the switch here? imo it's just easier to create a helper
				//that outputs the correct -ish or -y when asked for the claw color.
				return true;
			}
		}

		private class ImpClaws : HandType
		{
			public ImpClaws() : base(HandStyle.CLAWS, ImpShort, ImpFullDesc) { }

			public override bool canTone()
			{
				return true;
			}

			public override bool tryToTone(ref Tones currentTone, Tones primaryTone, Tones secondaryTone)
			{
				//do some magic to the tone to make it imp claw compatible
				currentTone = primaryTone;
				return true;
			}
		}
	}
}
