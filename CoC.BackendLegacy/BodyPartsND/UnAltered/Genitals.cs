//Genitals.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:16 AM
using CoC.Tools;
using System;
using System.Linq;

namespace  CoC.BodyParts
{
	//Container class for all cocks and vags the creature has. why? because it's easier this way.
	//I can do all the logic in here instead of in creature.
	//nearly everything here will get a courtesy alias in the creature class so you can be lazy.
	internal class Genitals
	{
		public const int MAX_COCKS = 10;
		public const int MAX_VAGINAS = 2;
		//max in game that i can find is 5, but they only ever use 4 rows.
		//apparently Fenoxo said 3 rows, but then after it went open, some shit got 4 rows.
		//i'm not being a dick and reverting that. 4 it is.
		public const int MAX_BREAST_ROWS = 4;

		readonly Cock[] cocks = new Cock[MAX_COCKS];
		//afaik, there are no cases where you have no breasts, but a hell of a lot of checks for them.
		//you now always have a pair of breasts. male breasts are flat with .25in nipples. viola
		readonly Breasts[] breasts = new Breasts[MAX_BREAST_ROWS];
		readonly Vagina[] vaginas = new Vagina[MAX_VAGINAS];
		readonly Balls balls;
		int cockCount = 0;
		int vaginaCount = 0;
		int breastRowCount = 0;

		public readonly Femininity masculinity;

		//normal gender check, based on your junk.
		public Gender gender
		{
			get
			{
				Gender retVal = Gender.GENDERLESS;
				retVal |= vaginaCount > 0 ? Gender.FEMALE : Gender.GENDERLESS;
				retVal |= cockCount > 0 || hasClitCock ? Gender.MALE : Gender.GENDERLESS;
				return retVal;
			}
		}

		//allows players with clit-dicks (a hard-to-obtain omnibus trait) to appear female, and do female scenes. 
		//NYI, but also allows players to "surprise" NPCs expecting lesbian sex (or males expecting straight sex)
		//Not to be confused with the "traps" check - this is a check for your junk.
		public Gender genderWithoutOmnibusClit
		{
			get
			{
				if (gender == Gender.HERM && cockCount == 0 && hasClitCock)
				{
					return Gender.FEMALE;
				}
				return gender;
			}
		}

		/* Trap check. use this where player appearance is more important than actual assets, or for trappy sex, idk.
		 * 
		 * Female: C-cup breasts and >35 masculinity OR <6in Dick and >65 masculinity
		 * Male: 6in+ Dick and <65 masculinity OR B-Cup or smaller breasts and <35 masculinity
		 * Genderless: <6in Dick, B-cup or smaller breasts, and 35-65 masculinity.
		 * Herm: everything else.
		 * 
		 * How you deal with androgenous and herm is up to you. Note that b/c this is a trap check, something may appear
		 * to be a herm, but not be (large breasts and a dick, for example, but no vag).
		 * 
		*/
		public Gender appearsAs
		{
			get
			{
				//noticable bulge and breasts
				if (biggestTitSize > CupSize.B && biggestCockSize >= 6)
				{
					return Gender.HERM;
				}
				//noticable breasts and sufficiently female
				else if (biggestTitSize > CupSize.B && !masculinity.atLeastSlightlyMasculine)
				{
					return Gender.FEMALE;
				}
				//noticable dick and sufficiently male
				else if (biggestCockSize >= 6 && !masculinity.atLeastSlightlyFeminine)
				{
					return Gender.MALE;
				}
				//not noticable assets - go by appearance
				else if (biggestCockSize < 6 && biggestTitSize <= CupSize.B)
				{
					if (masculinity.atLeastSlightlyFeminine) return Gender.FEMALE;
					else if (masculinity.atLeastSlightlyMasculine) return Gender.MALE;
					return Gender.GENDERLESS;
				}
				//noticable breasts or dick, but too masculine or feminine. 
				return Gender.HERM;
			}
		}

		public bool hasClitCock
		{
			get
			{
				for (int x = 0; x < vaginaCount; x++)
				{
					if (vaginas[x].omnibusClit) return true;
				}
				return false;
			}
		}

		//note: Constructor treats HERM as full-package futa.
		//if you want a ball-less herm, do female, then add a cock.
		//in the same vein, if you want a male with no balls, do genderless and add a cock.
		protected Genitals(Gender gender)
		{
			//Female or Herm:
			if (gender.HasFlag(Gender.FEMALE))
			{
				AddVagina(VaginaType.HUMAN);
				breasts[breastRowCount++] = Breasts.GenerateFemale();
			}
			//Male or Genderless
			else
			{
				breasts[breastRowCount++] = Breasts.GenerateMale();
			}
			//Male or Herm.
			if (gender.HasFlag(Gender.MALE))
			{
				AddCock(CockType.HUMAN);
			}
			balls = Balls.GenerateDefault(gender);
		}

		public static Genitals Generate(Gender gender)
		{
			return new Genitals(gender);
		}

		public static Genitals Generate(Cock[] cocks, Vagina[] vaginas, Balls balls, Breasts[] breasts)
		{

		}

#warning Add Message Helpers or Something, idk

		/// <summary>
		/// Evens out all breast rows so they are closer to the average nipple length and cup size, rounding up.
		/// large ones are shrunk, small ones grow. only does one unit of change, unless until even is set, then
		/// will completely average all values.
		/// </summary>
		/// <param name="untilEven">if true, forces all breast rows to average value, if false, only one unit.</param>
		public void NormalizeBreasts(bool untilEven = false)
		{
#warning IMPLEMENT ME!
			throw new System.NotImplementedException();
		}

		public void NormalizeDicks(bool untilEven = false)
		{
#warning IMPLEMENT ME!
			throw new System.NotImplementedException();
		}

		//Dog and wolf both make breast size one smaller than previous.
		//Everything else keeps the size.
		//Nipple status and blackness vary.
		//new behavior is that's uniform between all breasts.

		//nipple length will be the size of the average of all the other nipples.
		public bool AddBreastRow()
		{
#warning May want to make this use previous row size, idk.
			if (breastRowCount < MAX_BREAST_ROWS)
			{
				//linq ftw!
				//i find it funny that linq was created for databases, but it really is used for functional programming.
				double avgLength = breasts.Take(breastRowCount).Average((x) => (double)(x?.nipples.length));
				double avgCup = breasts.Take(breastRowCount).Average((x) => (double)x.cupSize.asInt());
				int cup = (int)Math.Ceiling(avgCup);
				breasts[breastRowCount++] = Breasts.GenerateFemale((CupSize)cup, (float)avgLength);
				return true;
			}
			return false;
		}

		public bool AddCock(CockType newCockType)
		{
			if (cockCount == MAX_COCKS)
			{
				return false;
			}
			cocks[cockCount++] = Cock.Generate(newCockType);
			return true;
		}

		public bool AddCock(CockType newCockType, float length, float girth)
		{
			if (cockCount == MAX_COCKS)
			{
				return false;
			}
			cocks[cockCount++] = Cock.Generate(newCockType, length, girth);
			return true;
		}

		public bool AddVagina(VaginaType newVaginaType)
		{
			if (vaginaCount == MAX_VAGINAS)
			{
				return false;
			}
			vaginas[vaginaCount++] = Vagina.Generate(newVaginaType);
			return true;
		}

		public bool AddVagina(VaginaType newVaginaType, float clitLength)
		{
			if (vaginaCount == MAX_VAGINAS)
			{
				return false;
			}
			vaginas[vaginaCount++] = Vagina.Generate(flags, newVaginaType, clitLength);
			return true;
		}

		public int RemoveBreastRow(int count = 1)
		{
			if (count < 0 || breastRowCount == 1 && breasts[0].isMale)
			{
				return 0;
			}
			int numRemoved = 0;
			//all breast rows except the first, or count rows, whichever comes first
			while (breastRowCount > 1 && count > 0)
			{
				//pre decrement, because we'd normally subtract by 1 anyway. so instead of saying x-1, and then x--, we can just say --x.
				breasts[--breastRowCount] = null;
				count--;
				numRemoved++;
			}
			//if we've removed all excess rows, but still have one 
			if (count > 0)
			{
				breasts[0].makeMale();
			}
			return numRemoved;
		}

		public int RemoveExtraBreastRows()
		{
			return RemoveBreastRow(breastRowCount - 1);
		}

		public int RemoveCock(int count = 1)
		{
			if (cockCount == 0 || count <= 0)
			{
				return 0;
			}
			int oldCount = cockCount;
			count = Math.Min(count, cockCount);
			for (int x = 0; x < count; x++)
			{
				cocks[--cockCount] = null;
			}
			return oldCount - cockCount;
		}

		public int RemoveVagina(int count = 1)
		{
			if (vaginaCount == 0 || count <= 0)
			{
				return 0;
			}
			int oldCount = vaginaCount;
			count = Math.Min(count, vaginaCount);
			for (int x = 0; x < count; x++)
			{
				vaginas[--vaginaCount] = null;
			}
			return oldCount - vaginaCount;
		}

		public bool MakeFemale()
		{
			if (cockCount == 0 && !hasClitCock)
			{
				return false;
			}
			RemoveCock(cockCount);
			for (int x = 0; x < vaginaCount; x++)
			{
				vaginas[x].clit.DeactivateOmnibusClit();
			}
			return true;
		}

		public bool MakeMale()
		{
			if (vaginaCount == 0)
			{
				return false;
			}
			RemoveVagina(vaginaCount);
			RemoveBreastRow(breastRowCount);
			return true;
		}
	}

	internal class Femininity
	{
		public const int MIN_ANDROGENOUS = 35;
		public const int ANDROGENOUS = 50;
		public const int MAX_ANDROGENOUS = 65;

		public const int SLIGHTLY_FEMININE = 60;
		public const int FEMININE = 70;
		public const int HYPER_FEMININE = 90;

		public const int SLIGHTLY_MASCULINE = 40;
		public const int MASCULINE = 30;
		public const int HYPER_MASCULINE = 10;

		public int femininity
		{
			get => _femininity;
			private set
			{
				Utils.Clamp(ref value, 0, 100);
				_femininity = value;
			}
		}
		private int _femininity;

		protected Femininity(int fem)
		{
			femininity = fem;
		}

		public static Femininity Generate(Gender gender)
		{
			int fem = 50;
			switch (gender)
			{
				case Gender.MALE:
					fem = 30;
					break;
				case Gender.FEMALE:
					fem = 70;
					break;
				case Gender.HERM:
					fem = 60;
					break;
				case Gender.GENDERLESS:
				default:
					fem = 50;
					break;
			}
			return new Femininity(fem);
		}
		public static Femininity Generate(int fem)
		{
			return new Femininity(fem);
		}

		public static implicit operator int(Femininity femininity)
		{
			return femininity.femininity;
		}
		public int feminize(uint amount)
		{
			if (femininity == 100)
			{
				return 0;
			}
			int oldFemininity = femininity;
			femininity += (int)amount;
			return femininity - oldFemininity;
		}

		public int masculinize(uint amount)
		{
			if (femininity == 0)
			{
				return 0;
			}
			int oldFemininity = femininity;
			femininity -= (int)amount;
			return oldFemininity - femininity;
		}

		public bool isAndrogenous => femininity >= Femininity.MIN_ANDROGENOUS && femininity <= Femininity.MAX_ANDROGENOUS;

		public bool isSlightlyFeminine => femininity >= Femininity.SLIGHTLY_FEMININE && femininity < Femininity.FEMININE;
		public bool atLeastSlightlyFeminine => femininity >= Femininity.SLIGHTLY_FEMININE && femininity < Femininity.FEMININE;
		public bool isFeminine => femininity >= Femininity.FEMININE && femininity < Femininity.HYPER_FEMININE;
		public bool atLeastFeminine => femininity >= Femininity.FEMININE;
		public bool isHyperFeminine => femininity >= Femininity.HYPER_FEMININE;
		public bool isSlightlyMasculine => femininity <= Femininity.SLIGHTLY_MASCULINE && femininity > Femininity.MASCULINE;
		public bool atLeastSlightlyMasculine => femininity <= Femininity.SLIGHTLY_MASCULINE;
		public bool isMasculine => femininity <= Femininity.MASCULINE && femininity > Femininity.HYPER_MASCULINE;
		public bool atLeastMasculine => femininity <= Femininity.MASCULINE;
		public bool isHyperMasculine => femininity <= Femininity.HYPER_MASCULINE;
	}

	internal class FemininityData : EventArgs
	{
		public readonly int femininity;

		public FemininityData(int fem)
		{
			femininity = fem;
		}

		public bool isFemale => atLeastSlightlyFeminine;
		public bool isMale => atLeastSlightlyMasculine;

		public bool isAndrogenous => femininity >= Femininity.MIN_ANDROGENOUS && femininity <= Femininity.MAX_ANDROGENOUS;

		public bool isSlightlyFeminine => femininity >= Femininity.SLIGHTLY_FEMININE && femininity < Femininity.FEMININE;
		public bool atLeastSlightlyFeminine => femininity >= Femininity.SLIGHTLY_FEMININE && femininity < Femininity.FEMININE;
		public bool isFeminine => femininity >= Femininity.FEMININE && femininity < Femininity.HYPER_FEMININE;
		public bool atLeastFeminine => femininity >= Femininity.FEMININE;
		public bool isHyperFeminine => femininity >= Femininity.HYPER_FEMININE;
		public bool isSlightlyMasculine => femininity <= Femininity.SLIGHTLY_MASCULINE && femininity > Femininity.MASCULINE;
		public bool atLeastSlightlyMasculine => femininity <= Femininity.SLIGHTLY_MASCULINE;
		public bool isMasculine => femininity <= Femininity.MASCULINE && femininity > Femininity.HYPER_MASCULINE;
		public bool atLeastMasculine => femininity <= Femininity.MASCULINE;
		public bool isHyperMasculine => femininity <= Femininity.HYPER_MASCULINE;
	}
}
