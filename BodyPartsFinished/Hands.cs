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
	class Hands : IImmutableToneable
	{
		private static int indexMaker = 0;
		public readonly int index;
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
			index = indexMaker++;
			description = desc;
		}

		public GenericDescription description { get; private set; }

		public static readonly Hands HUMAN = new Hands(HumanShort);
		public static readonly Hands LIZARD = new Hands(LizardShort);
		public static readonly Hands DRAGON = new Hands(DragonShort);
		public static readonly Hands SALAMANDER = new Hands(SalamanderShort);
		public static readonly Hands CAT = new Hands(CatShort);
		public static readonly Hands DOG = new Hands(DogShort);
		public static readonly Hands FOX = new Hands(FoxShort);
		public static readonly Hands IMP = new Hands(ImpShort);
		public static readonly Hands COCKATRICE = new Hands(CockatriceShort);
		public static readonly Hands RED_PANDA = new Hands(RedPandaShort);
		public static readonly Hands FERRET = new Hands(FerretShort);
		//public static readonly Hands MANTIS = new Hands(MantisShort);
	}
}
