//ClitVaginaStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 10:11 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	public partial class ClitPiercingLocation
	{
		private static string ChristinaButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ChristinaLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VerticalHoodButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VerticalHoodLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorizontalHoodButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorizontalHoodLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string TriangleButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string TriangleLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughClitButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughClitLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughLargeClit1Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughLargeClit1Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughLargeClit2Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughLargeClit2Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughLargeClit3Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ThroughLargeClit3Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	internal interface IClit
	{
		float length { get; }

		ReadOnlyPiercing<ClitPiercingLocation> piercings { get; }

		Guid creatureID { get; }
	}

	public partial class Clit : IClit
	{
		private string RequiresPiercingFetish()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllClitPiercingsShort(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllClitPiercingsLong(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string RequiresClitBeAtLeastThisLong(float length)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		float IClit.length => length;


		ReadOnlyPiercing<ClitPiercingLocation> IClit.piercings => piercings.AsReadOnlyData();

		Guid IClit.creatureID => creatureID;

		public static string Name()
		{
			return "Clit";
		}

	}
	public partial class ClitData : IClit
	{


		float IClit.length => length;

		ReadOnlyPiercing<ClitPiercingLocation> IClit.piercings => clitPiercings;

		Guid IClit.creatureID => creatureID;
	}

	internal static class ClitStrings
	{
		public static string ClitNoun(bool withArticle = false)
		{
			if (SFW_Settings.SFW_Enabled) return (withArticle ? "a " : "") + Utils.RandomChoice("bump", "button");
			else return (withArticle ? "a " : "") + Utils.RandomChoice("clit", "clitty", "button", "pleasure-buzzer", "clit", "clitty", "button", "clit", "clit", "button");
		}

		public static string PluralClitNoun()
		{
			if (SFW_Settings.SFW_Enabled) return Utils.RandomChoice("bumps", "buttons");
			else return Utils.RandomChoice("clits", "buttons", "pleasure-buzzers", "clits", "buttons", "clits", "clits", "love-buttons");
		}

		internal static string ShortDesc(float length)
		{
			return Measurement.ToNearestHalfSmallUnit(length, false, true, false) + " clit";
		}

		//some of these get an oxford comma on full description, some don't. Sue me.
		internal static string Desc(IClit clit, bool alternateFormat, bool full)
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
			//this is not a pretty solution, but i'm just going to cheat it anyway. basically, i'm going to check and see if article is null, and if it is, replace it.
			//by setting it to empty when we don't need it, it'll never be null and thus always skipped.
			string article = alternateFormat ? null : "";

			//Length Adjective - 50% chance
			if (Utils.RandBool() || full)
			{
				//small clits!
				if (clit.length <= .5)
				{
					if (article is null) article = "a ";
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
					if (article is null) article = "a ";
					size = Utils.RandomChoice("large", "large", "substantial", "substantial", "considerable ");
				}
				//'Uge
				else //if (clit.length >= 4)
				{
					size = Utils.RandomChoice("monster", "tremendous", "colossal", "enormous", "bulky ");
					if (size == "enormous" && article is null) article = "an ";
					else if (article is null) article = "a";
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
					if (article is null) article = "a ";
					adjective = separator + Utils.RandomChoice("throbbing", "pulsating", "hard");
				}
				//High libido - always use if no other descript
				else if (relativeLibido > 50 && Utils.Rand(2) == 0)
				{
					if (Utils.Rand(4) == 0)
					{
						adjective = "insatiable";
						if (article is null) article = "an ";
					}
					else
					{
						adjective = separator + Utils.RandomChoice("greedy", "demanding", "rapacious");
						if (article is null) article = "a ";
					}
				}
				//else if (clit.clitCockActive && Utils.RandBool())
				//{
				//	adjective = separator + Utils.RandomChoice("mutated", "corrupted");
				//	if (article is null) article = "a ";
				//}

				if (!full)
				{
					return article + size + adjective + " " + ClitNoun();
				}
				//else if (clit.clitCockActive)
				//{
				//	adjective += separator + Utils.RandomChoice("mutated ", "corrupted ");
				//}
				else
				{
					adjective += " ";
				}
			}

			//100% display rate if pierced and we've fallen through to this point, so we dont need to check for full.
			if (clit.piercings.isPierced)
			{
				if (article is null) article = "a ";
				return article + size + adjective + "pierced " + ClitNoun();
			}


			//fall through. will rarely hit this, if ever.
			return ClitNoun(article is null);
		}
	}
}
