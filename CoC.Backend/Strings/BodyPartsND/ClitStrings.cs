//ClitVaginaStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 10:11 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	internal interface IClit
	{
		float length { get; }

		bool clitCockActive { get; }

		ReadOnlyPiercing<ClitPiercings> piercings { get; }

		Guid creatureID { get; }
	}

	public partial class Clit : IClit
	{
		float IClit.length => length;


		bool IClit.clitCockActive => omnibusClit && omnibusActive;

		ReadOnlyPiercing<ClitPiercings> IClit.piercings => clitPiercings.AsReadOnlyData();

		Guid IClit.creatureID => creatureID;

		public static string Name()
		{
			return "Clit";
		}

		public string ShortDescription() => ClitData.ShortDesc(length);
		public string LongDescription() => ClitData.Desc(this, false);
		public string FullDescription() => ClitData.Desc(this, true);

	}
	public partial class ClitData : IClit
	{
		float IClit.length => length;

		bool IClit.clitCockActive => clitCockActive;

		ReadOnlyPiercing<ClitPiercings> IClit.piercings => clitPiercings;

		Guid IClit.creatureID => CreatureID;

		public static string ClitNouns()
		{
			if (CoC.Backend.SaveData.BackendSessionSave.data.SFW_Mode) return Utils.RandomChoice("bump", "button");
			else return Utils.RandomChoice("clit", "clitty", "button", "pleasure-buzzer", "clit", "clitty", "button", "clit", "clit", "button");
		}

		public string ShortDescription() => ShortDesc(length);
		public string LongDescription() => Desc(this, false);
		public string FullDescription() => Desc(this, true);

		internal static string ShortDesc(float length)
		{
			return Measurement.ToNearestHalfSmallUnit(length, false, true, false) + " clit";
		}

		//some of these get an oxford comma on full description, some don't. Sue me. 
		internal static string Desc(IClit clit, bool full)
		{
			float relativeLibido, relativeLust;

			if (CreatureStore.TryGetCreature(clit.creatureID, out Creature source))
			{
				relativeLibido = source.relativeLibido;
				relativeLust = source.relativeLust;
			}
			else
			{
				relativeLibido = 15;
				relativeLust = 15;
			}

			string size = "";
			//Length Adjective - 50% chance
			if (Utils.RandBool() || full)
			{
				//small clits!
				if (clit.length <= .5)
				{
					size = Utils.RandomChoice("tiny", "little", "petite", "diminutive", "miniature ");
				}
				//"average". no comment
				else if (clit.length < 1.5)
				{
					//no size comment
				}
				//Biggies!
				else if (clit.length < 4)
				{
					size = Utils.RandomChoice("large", "large", "substantial", "substantial", "considerable ");
				}
				//'Uge
				else //if (clit.length >= 4)
				{
					size = Utils.RandomChoice("monster", "tremendous", "colossal", "enormous", "bulky ");
				}
			}

			string adjective = "";

			//Descriptive descriptions - 50% chance of being called
			if (Utils.Rand(2) == 0 || full)
			{
				string separator = full ? ", " : " ";
				//Doggie descriptors - 50%
				//TODO Conditionals don't make sense, need to introduce a class variable to keep of "something" or move race or Creature/Character
				//if (creature.hasAnyFur > 2  && Utils.Rand(2) == 0)
				//{
				//	description += "bitch-";
				//	haveDescription = true;
				//}
				/*Horse descriptors - 50%
				 if (creature.hasAnyFur > 2 && !descripted && Utils.Rand(2) == 0) {
				 descripted = true;
				 descript += "mare-";
				 }*/
				//Horny descriptors - 75% chance
				if (relativeLust > 70 && Utils.Rand(4) < 3)
				{
					adjective = separator + Utils.RandomChoice("throbbing", "pulsating", "hard") ;
				}
				//High libido - always use if no other descript
				else if (relativeLibido > 50 && Utils.Rand(2) == 0)
				{
					adjective = separator + Utils.RandomChoice("insatiable", "greedy", "demanding", "rapacious") ;
				}
				else if (clit.clitCockActive && Utils.RandBool())
				{
					adjective = separator + Utils.RandomChoice("mutated", "corrupted") ;
				}



				if (!full)
				{
					return size + adjective + " " + ClitNouns();
				}
				else if (clit.clitCockActive)
				{
					adjective += separator + Utils.RandomChoice("mutated ", "corrupted ");
				}
				else
				{
					adjective += " ";
				}
			}

			//100% display rate if pierced and we've fallen through to this point, so we dont need to check for full.
			if (clit.piercings.isPierced)
			{
				return size + adjective + "pierced " + ClitNouns();
			}


			//fall through. will rarely hit this, if ever. 
			return ClitNouns();
		}
	}
}
