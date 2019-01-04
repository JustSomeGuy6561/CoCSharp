//Hands.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.BodyParts.SpecialInteraction;
using static CoC.Strings.BodyParts.HandStrings;
using CoC.Items;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	public class Hands : SimpleBodyPart<HandType>, IToneAware
	{
		public Tones clawTone { get; protected set; }
		public void reactToChangeInSkinTone(Tones newTone)
		{
			if (type.canTone())
			{
				Tones claw = clawTone;
				type.tryToTone(ref claw, newTone);
				clawTone = claw;
			}
		}
	}

	public class HandType : SimpleBodyPartType
	{
		private static int indexMaker = 0;
		public override int index => _index;
		protected readonly int _index;
		public virtual bool canTone()
		{
			return false;
		}

		public virtual bool tryToTone(ref Tones currentTone, Tones newTone)
		{
			if (canTone())
			{
				currentTone = newTone;
				return currentTone == newTone;
			}
			return false;
		}

		protected HandType(GenericDescription shortDesc) : base(shortDesc)
		{
			_index = indexMaker++;
		}

		public static readonly HandType HUMAN = new HandType(HumanShort);
		public static readonly HandType LIZARD = new LizardClaws();
		public static readonly HandType DRAGON = new HandType(DragonShort);
		public static readonly HandType SALAMANDER = new HandType(SalamanderShort);
		public static readonly HandType CAT = new HandType(CatShort);
		public static readonly HandType DOG = new HandType(DogShort);
		public static readonly HandType FOX = new HandType(FoxShort);
		public static readonly HandType IMP = new ImpClaws();
		public static readonly HandType COCKATRICE = new HandType(CockatriceShort);
		public static readonly HandType RED_PANDA = new HandType(RedPandaShort);
		public static readonly HandType FERRET = new HandType(FerretShort);
		//public static readonly Hands MANTIS = new Hands(MantisShort);

		private class LizardClaws : HandType
		{

			public LizardClaws() : base(LizardShort) { }

			public override bool canTone()
			{
				return true;
			}

			public override bool tryToTone(ref Tones currentTone, Tones newTone)
			{
				//do some magic to the tone to make it lizard claw compatible
				currentTone = newTone;
				//maybe implement the switch here? imo it's just easier to create a helper
				//that outputs the correct -ish or -y when asked for the claw color.
				return true;
			}
		}

		private class ImpClaws : HandType
		{
			public ImpClaws() : base(ImpShort) { }

			public override bool canTone()
			{
				return true;
			}

			public override bool tryToTone(ref Tones currentTone, Tones newTone)
			{
				//do some magic to the tone to make it imp claw compatible
				currentTone = newTone;
				return true;
			}
		}
	}
}
