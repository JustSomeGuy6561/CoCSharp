using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;

namespace CoC.Backend.Strings
{
	//a helper class that lets us abstract out the pronouns so that we can use text for more than just the player.
	//this isn't all the pronoun forms in English, and i'm sure some languages have more pronoun forms that don't have an English equivalent as well,
	//but this (so far, at least) is all we need for the generic text. obviously if a situation is tailored to the player, we don't need to use this, so
	//most text doesn't care. but for the player description, for example, it's possible to change that to work with any creature if we replace the string "You" or "your" (etc)
	//with the correct pronoun.

	//i, you, he/she/it, we, you all, they
	public enum ConjugateForm { FIRST_SINGULAR, SECOND_SINGULAR, THIRD_SINGULAR, FIRST_PLURAL, SECOND_PLURAL, THIRD_PLURAL}

	public sealed class Conjugate
	{
		private readonly SimpleDescriptor possessiveNoun; //his, hers, mine, etc.
		private readonly SimpleDescriptor possessiveAdjective; //his, her, my, etc.

		private readonly SimpleDescriptor personalNoun; //he, she, i, etc.
		private readonly SimpleDescriptor personalNounWithHave; //he's, she's, they've, i've
		private readonly SimpleDescriptor personalNounWithAre; //he's, she's, i'm, they're
		private readonly SimpleDescriptor objectNoun; //me, him, her, etc.

		private readonly SimpleDescriptor reflexiveNoun; //myself, herself, himself, etc.

		public string PossessiveNoun() => possessiveNoun();
		public string PossessiveAdjective() => possessiveAdjective();

		public string PersonalNoun() => personalNoun();
		public string PersonalNounWithHave() => personalNounWithHave();
		public string PersonalNounWithAre() => personalNounWithAre();
		public string ObjectNoun() => objectNoun();

		public string ReflexiveNoun() => reflexiveNoun();

		private Conjugate(SimpleDescriptor possessiveNoun, SimpleDescriptor possessiveAdjective, SimpleDescriptor personalNoun, SimpleDescriptor objectNoun,
			SimpleDescriptor reflexiveNoun, SimpleDescriptor personalWithHave, SimpleDescriptor personalWithAre)
		{
			this.possessiveNoun = possessiveNoun ?? throw new ArgumentNullException(nameof(possessiveNoun));
			this.possessiveAdjective = possessiveAdjective ?? throw new ArgumentNullException(nameof(possessiveAdjective));
			this.personalNoun = personalNoun ?? throw new ArgumentNullException(nameof(personalNoun));
			this.objectNoun = objectNoun ?? throw new ArgumentNullException(nameof(objectNoun));
			this.reflexiveNoun = reflexiveNoun ?? throw new ArgumentNullException(nameof(reflexiveNoun));

			this.personalNounWithHave = personalWithHave ?? throw new ArgumentNullException(nameof(personalWithHave));
			this.personalNounWithAre = personalWithAre ?? throw new ArgumentNullException(nameof(personalWithAre));
		}

		public static Conjugate I = new Conjugate(IPoss, IAdj, INoun, IObj, IReflex, IHave, IAre);
		public static Conjugate YOU = new Conjugate(YouPoss, YouAdj, YouNoun, YouObj, YouReflex, YouHave, YouAre);
		public static Conjugate HE = new Conjugate(HePoss, HeAdj, HeNoun, HeObj, HeReflex, HeHave, HeAre);
		public static Conjugate SHE = new Conjugate(ShePoss, SheAdj, SheNoun, SheObj, SheReflex, SheHave, SheAre);
		public static Conjugate IT = new Conjugate(ItPoss, ItAdj, ItNoun, ItObj, ItReflex, ItHave, ItAre);
		public static Conjugate WE = new Conjugate(WePoss, WeAdj, WeNoun, WeObj, WeReflex, WeHave, WeAre);
		public static Conjugate YOU_ALL = new Conjugate(YouAllPoss, YouAllAdj, YouAllNoun, YouAllObj, YouAllReflex, YallHave, YouAllAre);
		public static Conjugate THEY = new Conjugate(TheyPoss, TheyAdj, TheyNoun, TheyObj, TheyReflex, TheyHave, TheyAre);

		private static string IHave()
		{
			return "I've";
		}

		private static string YouHave()
		{
			return "you've";
		}

		private static string HeHave()
		{
			return "he's";
		}

		private static string SheHave()
		{
			return "she's";
		}

		private static string ItHave()
		{
			//technically this is 'it is', but it's used both ways, informally.
			return "it's";
		}
		private static string WeHave()
		{
			return "we've";
		}

		private static string YallHave()
		{
			return "you all have";
		}

		private static string TheyHave()
		{
			return "they've";
		}

		private static string IAre()
		{
			return "I'm";
		}

		private static string YouAre()
		{
			return "you're";
		}

		private static string HeAre()
		{
			return "he's";
		}

		private static string SheAre()
		{
			return "she's";
		}

		private static string ItAre()
		{
			return "it's";
		}
		private static string WeAre()
		{
			return "we're";
		}

		private static string YouAllAre()
		{
			return "you all are";
		}

		private static string TheyAre()
		{
			return "they're";
		}

		public static Conjugate FromGender(Gender gender, bool plural = false)
		{
			if (plural)
			{
				return THEY;
			}
			else if (gender == Gender.GENDERLESS)
			{
				return IT;
			}
			else if (gender == Gender.MALE)
			{
				return HE;
			}
			else
			{
				return SHE;
			}
		}

		public static Conjugate FromCreature(Creature creature, bool useApparentGender = false, bool plural = false)
		{
			if (creature == CreatureStore.currentControlledCharacter)
			{
				return plural ? YOU_ALL : YOU;
			}
			else if (plural)
			{
				return THEY;
			}
			else if (useApparentGender)
			{
				return FromGender(creature.genitals.ApparentGender());
			}
			else
			{
				return FromGender(creature.gender, plural);
			}
		}


		private static string IPoss()
		{
			return "mine";
		}
		private static string IAdj()
		{
			return "my";
		}
		private static string INoun()
		{
			return "I";
		}
		private static string IObj()
		{
			return "me";
		}
		private static string IReflex()
		{
			return "myself";
		}
		private static string YouPoss()
		{
			return "yours";
		}
		private static string YouAdj()
		{
			return "your";
		}
		private static string YouNoun()
		{
			return "you";
		}
		private static string YouObj()
		{
			return "you";
		}
		private static string YouReflex()
		{
			return "yourself";
		}
		private static string HePoss()
		{
			return "his";
		}
		private static string HeAdj()
		{
			return "his";
		}
		private static string HeNoun()
		{
			return "he";
		}
		private static string HeObj()
		{
			return "him";
		}
		private static string HeReflex()
		{
			return "himself";
		}
		private static string ShePoss()
		{
			return "hers";
		}
		private static string SheAdj()
		{
			return "her";
		}
		private static string SheNoun()
		{
			return "she";
		}
		private static string SheObj()
		{
			return "her";
		}
		private static string SheReflex()
		{
			return "herself";
		}
		private static string ItPoss()
		{
			return "its";
		}
		private static string ItAdj()
		{
			return "its";
		}
		private static string ItNoun()
		{
			return "it";
		}
		private static string ItObj()
		{
			return "it";
		}
		private static string ItReflex()
		{
			return "itself";
		}
		private static string WePoss()
		{
			return "ours";
		}
		private static string WeAdj()
		{
			return "our";
		}
		private static string WeNoun()
		{
			return "we";
		}
		private static string WeObj()
		{
			return "them";
		}
		private static string WeReflex()
		{
			return "ourselves";
		}
		//god, english is shit for 'you all' conjugation. Y'all is the closest, but makes you sound like a hick.
		private static string YouAllPoss()
		{
			//hick format: "all y'all"
			return "all of you";
		}
		private static string YouAllAdj()
		{
			//hick format: "all y'alls"
			return "all of yours";
		}
		private static string YouAllNoun()
		{
			//hick format: "y'all"
			return "you all";
		}
		private static string YouAllObj()
		{
			//hick format: "y'all"
			return "you all";
		}

		//but for some reason English has a reflexive plural for 'you.' odd.
		private static string YouAllReflex()
		{
			//hick format: "alls y'all"
			return "yourselves";
		}
		private static string TheyPoss()
		{
			return "theirs";
		}
		private static string TheyAdj()
		{
			return "their";
		}
		private static string TheyNoun()
		{
			return "they";
		}
		private static string TheyObj()
		{
			return "them";
		}
		private static string TheyReflex()
		{
			return "themselvers";
		}
	}
}
