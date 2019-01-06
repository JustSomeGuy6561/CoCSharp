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
		readonly Cock[] cocks = new Cock[MAX_COCKS];
		readonly Vagina[] vaginas = new Vagina[MAX_VAGINAS];
		Balls balls;
		int cockCount = 0;
		int vaginaCount = 0;

		Gender gender
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
		Gender appearsAs
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

		bool hasClitCock
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

		public bool AddCock(CockType newCockType)
		{
			if (cockCount == MAX_COCKS)
			{
				return false;
			}
			cocks[cockCount++] = Cock.Generate(newCockType);
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
				cocks[cockCount--] = null;
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
				vaginas[vaginaCount--] = null;
			}
			return oldCount - vaginaCount;
		}

		public bool MakeFemale()
		{
			if (cockCount == 0 && !hasClitCock)
			{
				return false;
			}
			removeCock(cockCount);
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
			removeVagina(vaginaCount);
			return true;
		}
	}
}
