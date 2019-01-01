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
	public class Hands : SimpleBodyPart, IImmutableToneable
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
			return false;
		}

		protected Hands(GenericDescription desc)
		{
			_index = indexMaker++;
			shortDescription = desc;
		}

		public override GenericDescription shortDescription { get; protected set; }

		public static readonly Hands HUMAN = new Hands(HumanShort);
		public static readonly Hands LIZARD = new LizardClaws();
		public static readonly Hands DRAGON = new Hands(DragonShort);
		public static readonly Hands SALAMANDER = new Hands(SalamanderShort);
		public static readonly Hands CAT = new Hands(CatShort);
		public static readonly Hands DOG = new Hands(DogShort);
		public static readonly Hands FOX = new Hands(FoxShort);
		public static readonly Hands IMP = new ImpClaws();
		public static readonly Hands COCKATRICE = new Hands(CockatriceShort);
		public static readonly Hands RED_PANDA = new Hands(RedPandaShort);
		public static readonly Hands FERRET = new Hands(FerretShort);
		//public static readonly Hands MANTIS = new Hands(MantisShort);

		private class LizardClaws : Hands
		{
			public LizardClaws() : base(LizardShort) { }

			public override bool canTone()
			{
				return true;
			}

			public override bool tryToTone(ref Tones currentTone, Tones newTone)
			{
				currentTone = newTone;
				//maybe implement the switch here? imo it's just easier to create a helper
				//that outputs the correct -ish or -y when asked for the claw color.
				return true;
			}
		}

		private class ImpClaws : Hands
		{
			public ImpClaws() : base(ImpShort) { }

			public override bool canTone()
			{
				return true;
			}

			public override bool tryToTone(ref Tones currentTone, Tones newTone)
			{
				currentTone = newTone;
				return true;
			}
		}
	}
}
