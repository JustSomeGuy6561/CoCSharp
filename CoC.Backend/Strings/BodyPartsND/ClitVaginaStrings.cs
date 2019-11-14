//ClitVaginaStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 10:11 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Clit
	{
		public static string Name()
		{
			return "Clit";
		}

		public string ShortDescription()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public string LongDescription()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class Vagina
	{
		public static string Name()
		{
			return "Vagina";
		}

		private string VaginaTightenedUpDueToInactivity(VaginalLooseness currentLooseness)
		{
			string recoverText;

			if (currentLooseness <= VaginalLooseness.ROOMY)
			{
				recoverText = " recovers from your ordeals, tightening up a bit.";
			}
			else if (currentLooseness == VaginalLooseness.GAPING)
			{
				recoverText = " recovers from your ordeals and becomes tighter.";
			}
			else //if (currentLooseness >= VaginalLooseness.CLOWN_CAR_WIDE)
			{
				recoverText = " recovers from the brutal stretching it has received and tightens up a little bit, but not much.";
			}
			return Environment.NewLine + "Your " + ShortDescription() + recoverText + Environment.NewLine;

		}

		public string FullDescription()
		{
			return LongDescription() + " with a " + clit.LongDescription() + (virgin ? " and an intact hymen" : "") + ".";
		}
	}

	public partial class VaginaType
	{
		private static readonly string[] sfwVagDesc = new string[] { "vagina", "pussy", "cooter", "snatch", "muff" };
		private static readonly string[] allVagDesc = new string[] { "vagina", "pussy", "cooter", "twat", "cunt", "snatch", "fuck-hole", "muff" };

		private static bool isSFW => SaveData.BackendSessionSave.data.SFW_Mode;

		private static string VagHumanDesc()
		{
			return Utils.RandomChoice("vagina, pussy");
		}
		private static string VagHumanLongDesc(Vagina vagina)
		{
			return GenericLongDesc(vagina, false);

		}
		private static string VagHumanPlayerStr(Vagina vagina, PlayerBase player)
		{
			return GenericPlayerStr(vagina, player);
		}
		private static string VagEquineDesc()
		{
			return "equine pussy";
		}
		private static string VagEquineLongDesc(Vagina vagina)
		{
			return GenericLongDesc(vagina, false, "equine");
		}
		private static string VagEquinePlayerStr(Vagina vagina, PlayerBase player)
		{
			return GenericPlayerStr(vagina, player);
		}
		private static string VagEquineTransformStr(Vagina vagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineRestoreStr(Vagina vagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapDesc()
		{
			return "jet black pussy";
		}
		private static string VagSandTrapLongDesc(Vagina vagina)
		{
			var sandTrapOptions = new string[]
			{
				"black",
				"onyx",
				"ebony",
				"dusky",
				"sable",
				"obsidian",
				"midnight-hued",
				"jet black"
			};

			return GenericLongDesc(vagina, false, Utils.RandomChoice(sandTrapOptions));
		}
		private static string VagSandTrapPlayerStr(Vagina vagina, PlayerBase player)
		{
			return GenericPlayerStr(vagina, player);
		}
		private static string VagSandTrapTransformStr(Vagina vagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapRestoreStr(Vagina vagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GenericLongDesc(Vagina vagina, bool forceLongDesc, string optionalAdjective = null)
		{
			StringBuilder sb = new StringBuilder();

			//less confusing way to display values.

			bool showLooseness;
			bool showWetness;

			//guarenteed if virgin, tight, or forced.
			if (forceLongDesc || vagina.virgin || vagina.looseness == VaginalLooseness.TIGHT)
			{
				showLooseness = true;
				showWetness = true;
			}
			//50% each if gaping or looser
			else if (vagina.looseness == VaginalLooseness.GAPING || vagina.looseness == VaginalLooseness.CLOWN_CAR_WIDE)
			{
				showLooseness = Utils.RandBool();
				showWetness = Utils.RandBool();
			}
			//else variable.
			else
			{
				//tightness descript - 40% display rate
				showLooseness = Utils.Rand(5) < 2;
				//wetness descript - 30% display rate
				showWetness = Utils.Rand(10) < 3;
			}


			if (showLooseness)
			{
				sb.Append(vagina.looseness.AsDescriptor());
			}
			if (showWetness && vagina.wetness != VaginalWetness.NORMAL)
			{
				if (sb.Length != 0) sb.Append(", ");
				sb.Append(vagina.wetness.AsDescriptor());
			}
			if (vagina.labiaPiercings.isPierced && (forceLongDesc || Utils.Rand(3) == 0))
			{
				if (sb.Length != 0) sb.Append(", ");
				sb.Append("pierced");
			}

			var creature = CreatureStore.GetCreatureClean(vagina.creatureID);

			if (sb.Length == 0 && creature?.body.type == BodyType.GOO)
			{
				if (sb.Length != 0) sb.Append(", ");
				sb.Append(Utils.RandomChoice("gooey", "slimy"));
			}
			if (optionalAdjective != null)
			{
				if (sb.Length != 0) sb.Append(", ");
				sb.Append(optionalAdjective);
			}

			if (!vagina.everPracticedVaginal)
			{
				if (sb.Length != 0)
				{
					sb.Append(" ");
				}
				sb.Append("true virgin");
			}
			else if (vagina.virgin)
			{
				if (sb.Length != 0)
				{
					sb.Append(" ");
				}
				sb.Append("virgin");
			}

			if (sb.Length != 0)
			{
				sb.Append(" ");
			}

			if (isSFW)
			{
				sb.Append(Utils.RandomChoice(sfwVagDesc));

			}
			else
			{
				sb.Append(Utils.RandomChoice(allVagDesc));
			}//Something that would be nice to have but needs a variable in Creature or Character.
			 //if (i_creature.bunnyScore() >= 3) sb.Append("rabbit hole");

			return sb.ToString();

		}

		private static string GenericPlayerStr(Vagina vagina, PlayerBase player)
		{
			return "You have a " + vagina.FullDescription();
		}
	}

	public static class VaginaHelpers
	{
		public static string AsText(this VaginalLooseness vaginalLooseness)
		{
			switch (vaginalLooseness)
			{
				case VaginalLooseness.CLOWN_CAR_WIDE:
					return "clown-care wide";
				case VaginalLooseness.GAPING:
					return "gaping";
				case VaginalLooseness.ROOMY:
					return "roomy";
				case VaginalLooseness.LOOSE:
					return "loose";
				case VaginalLooseness.TIGHT:
					return "tight";
				case VaginalLooseness.NORMAL:
				default:
					return "normal";
			}
		}

		public static string AsDescriptor(this VaginalLooseness vaginalLooseness)
		{
			switch (vaginalLooseness)
			{
				case VaginalLooseness.CLOWN_CAR_WIDE:
					return "clown-car wide";
				case VaginalLooseness.GAPING:
					return "gaping";
				case VaginalLooseness.ROOMY:
					return "roomy";
				case VaginalLooseness.LOOSE:
					return "loose";
				case VaginalLooseness.TIGHT:
					return "tight";
				case VaginalLooseness.NORMAL:
				default:
					return "";
			}
		}

		public static string AsText(this VaginalWetness vaginalWetness)
		{
			switch (vaginalWetness)
			{
				case VaginalWetness.SLAVERING:
					return "slavering";
				case VaginalWetness.DROOLING:
					return "drooling";
				case VaginalWetness.SLICK:
					return "slick";
				case VaginalWetness.WET:
					return "wet";
				case VaginalWetness.DRY:
					return "dry";
				case VaginalWetness.NORMAL:
				default:
					return "normal";
			}
		}

		public static string AsDescriptor(this VaginalWetness vaginalWetness)
		{
			switch (vaginalWetness)
			{
				case VaginalWetness.SLAVERING:
					return "slavering";
				case VaginalWetness.DROOLING:
					return "drooling";
				case VaginalWetness.SLICK:
					return "slick";
				case VaginalWetness.WET:
					return "wet";
				case VaginalWetness.DRY:
					return "dry";
				case VaginalWetness.NORMAL:
				default:
					return "";
			}
		}
	}
}
