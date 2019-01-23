//Hands.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;
using static CoC.Strings.BodyParts.HandStrings;

namespace CoC.BodyParts
{
	public class Hands : SimpleBodyPart<HandType>, IToneAware
	{
		public override HandType type { get; protected set; }

		public Hands(HandType type, Tones currentTone) : base(type)
		{
			this.clawTone = currentTone;
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

		public void reactToChangeInSkinTone(object sender, ToneAwareEventArg e)
		{
			if (type.canTone())
			{
				Tones claw = clawTone;
				type.tryToTone(ref claw, e.primaryTone, e.secondaryTone);
				clawTone = claw;
			}
		}
	}

	public class HandType : SimpleBodyPartType
	{
		protected enum HandStyle { HANDS, CLAWS, PAWS }
		private static int indexMaker = 0;
		public override int index => _index;
		protected readonly int _index;
		public virtual bool canTone()
		{
			return false;
		}
		public bool isClaws => handStyle == HandStyle.CLAWS;
		public bool isPaws => handStyle == HandStyle.PAWS;
		public bool isHands => handStyle == HandStyle.HANDS;

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

		protected HandType(HandStyle style, SimpleDescriptor shortDesc) : base(shortDesc)
		{
			_index = indexMaker++;
			handStyle = style;
		}

		public static readonly HandType HUMAN = new HandType(HandStyle.HANDS, HumanShort);
		public static readonly HandType LIZARD = new LizardClaws();
		public static readonly HandType DRAGON = new HandType(HandStyle.CLAWS, DragonShort);
		public static readonly HandType SALAMANDER = new HandType(HandStyle.CLAWS, SalamanderShort);
		public static readonly HandType CAT = new HandType(HandStyle.PAWS, CatShort);
		public static readonly HandType DOG = new HandType(HandStyle.PAWS, DogShort);
		public static readonly HandType FOX = new HandType(HandStyle.PAWS, FoxShort);
		public static readonly HandType IMP = new ImpClaws();
		public static readonly HandType COCKATRICE = new HandType(HandStyle.CLAWS, CockatriceShort);
		public static readonly HandType RED_PANDA = new HandType(HandStyle.PAWS, RedPandaShort);
		public static readonly HandType FERRET = new HandType(HandStyle.PAWS, FerretShort);
		//public static readonly Hands MANTIS = new Hands(MantisShort);

		private class LizardClaws : HandType
		{

			public LizardClaws() : base(HandStyle.CLAWS, LizardShort) { }

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
			public ImpClaws() : base(HandStyle.CLAWS, ImpShort) { }

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
