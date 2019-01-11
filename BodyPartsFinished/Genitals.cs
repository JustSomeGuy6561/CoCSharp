//Genitals.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:16 AM
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	//Container class for all cocks and vags the creature has. why? because it's easier this way.
	//I can do all the logic in here instead of in creature.
	//nearly everything here will get a courtesy alias in the creature class so you can be lazy.
	class Genitals
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
		private readonly PiercingFlags flags;

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

		//allows players with clit-dicks (an omnibus trait granted by ceraph or something, idk - NYI.
		//to appear female, and do female scenes. 
		//NYI, but also allows players to "surprise" NPCs expecting lesbian sex (or males expecting straight sex)
		//Not to be confused with a "traps" check - this is a check for your junk. a femininity/masculinity check is for that
		//though it should probably also take into account what you're packing and how visible it is. i.e. - you can't appear male
		//when packing huge tits, and you can't appear female if your dick is hanging out. 
		public Gender appearsAs
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


		protected Genitals(PiercingFlags piercingFlags, Gender gender)
		{
			flags = piercingFlags;
			//Female or Herm:
			if ((gender & Gender.FEMALE) != Gender.GENDERLESS)
			{
				AddVagina(VaginaType.HUMAN);
				//MUST BE IN THE END RESULT IN SOME WAY.
				breasts[breastRowCount++] = Breasts.GenerateFemale(flags);
			}
			//Male or Genderless
			else
			{
				//SEE ABOVE
				breasts[breastRowCount++] = Breasts.GenerateMale(flags);
			}
			//Male or Herm.
			if ((gender & Gender.MALE) != Gender.GENDERLESS)
			{
				AddCock(CockType.HUMAN);
			}
		}

		public static Genitals Generate(PiercingFlags flags, Gender gender)
		{
			return new Genitals(flags, gender);
		}

#error Add Message Helpers or Something, idk

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
				double? avgLength = breasts.Average((x) => (double?)(x?.nipples.length));
				double length = avgLength == null ? 0.25 : (double)avgLength;
				breasts[breastRowCount++] = Breasts.GenerateFemale(flags, (float)length);
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
			cocks[cockCount++] = Cock.Generate(newCockType, flags);
			return true;
		}

		public bool AddCock(CockType newCockType, float length, float girth)
		{
			if (cockCount == MAX_COCKS)
			{
				return false;
			}
			cocks[cockCount++] = Cock.Generate(newCockType, flags, length, girth);
			return true;
		}

		public bool AddVagina(VaginaType newVaginaType)
		{
			if (vaginaCount == MAX_VAGINAS)
			{
				return false;
			}
			vaginas[vaginaCount++] = Vagina.Generate(flags, newVaginaType);
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
}
