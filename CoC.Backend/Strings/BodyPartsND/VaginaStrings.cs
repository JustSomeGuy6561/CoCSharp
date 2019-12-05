using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Vagina
	{
		public static string Name()
		{
			return "Vagina";
		}

		private string VaginaTightenedUpDueToInactivity(VaginalLooseness oldLooseness)
		{

			string recoverText;

			if (oldLooseness <= VaginalLooseness.ROOMY)
			{
				recoverText = " recovers from your ordeals, tightening up a bit.";
			}
			else if (oldLooseness == VaginalLooseness.GAPING)
			{
				recoverText = " recovers from your ordeals and becomes tighter.";
			}
			else //if (oldLooseness >= VaginalLooseness.CLOWN_CAR_WIDE)
			{
				string amount = looseness >= VaginalLooseness.ROOMY ? ", but not much." : ".";
				recoverText = " recovers from the brutal stretching it has received and tightens up a little bit" + amount;
			}
			return Environment.NewLine + "Your " + ShortDescription() + recoverText + Environment.NewLine;
		}
	}

	public partial class VaginaType
	{
		private static readonly string[] sfwVagDesc = new string[] { "vagina", "pussy", "cooter", "snatch", "muff" };
		private static readonly string[] allVagDesc = new string[] { "vagina", "pussy", "cooter", "twat", "cunt", "snatch", "fuck-hole", "muff" };

		private static readonly string[] sandTrapStrings = new string[] { "black", "onyx", "ebony", "dusky", "sable", "obsidian", "midnight-hued", "jet black" };


		public static string VaginaNoun()
		{
			string[] items = isSFW ? sfwVagDesc : allVagDesc;
			return Utils.RandomChoice(items);
		}

		private static bool isSFW => SaveData.BackendSessionSave.data.SFW_Mode;

		private static string VagHumanDesc()
		{
			return VaginaNoun();
		}
		private static string VagHumanLongDesc(VaginaData vagina)
		{
			return GenericDesc(vagina, false);

		}
		private static string VagHumanFullDesc(VaginaData vagina)
		{
			return GenericDesc(vagina, true);

		}
		private static string VagHumanPlayerStr(Vagina vagina, PlayerBase player)
		{
			return GenericPlayerStr(vagina, player);
		}
		private static string VagEquineDesc()
		{
			return "equine " + VaginaNoun();
		}
		private static string VagEquineLongDesc(VaginaData vagina)
		{
			return GenericDesc(vagina, false, "equine");
		}
		private static string VagEquineFullDesc(VaginaData vagina)
		{
			return GenericDesc(vagina, true, "equine");
		}
		private static string VagEquinePlayerStr(Vagina vagina, PlayerBase player)
		{
			return GenericPlayerStr(vagina, player);
		}
		private static string VagEquineTransformStr(VaginaData oldVagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineRestoreStr(VaginaData oldVagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapDesc()
		{
			return Utils.RandomChoice(sandTrapStrings) + VaginaNoun();
		}
		private static string VagSandTrapLongDesc(VaginaData vagina)
		{
			return GenericDesc(vagina, false, Utils.RandomChoice(sandTrapStrings));
		}

		private static string VagSandTrapFullDesc(VaginaData vagina)
		{
			return GenericDesc(vagina, true, Utils.RandomChoice(sandTrapStrings));
		}
		private static string VagSandTrapPlayerStr(Vagina vagina, PlayerBase player)
		{
			return GenericPlayerStr(vagina, player);
		}
		private static string VagSandTrapTransformStr(VaginaData oldVagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapRestoreStr(VaginaData oldVagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GenericDesc(VaginaData vagina, bool fullDesc, string optionalAdjective = null)
		{
			StringBuilder sb = new StringBuilder();

			//less confusing way to display values.

			bool showLooseness;
			bool showWetness;

			//guarenteed if virgin, tight, or forced.
			if (fullDesc || vagina.isVirgin || vagina.looseness == VaginalLooseness.TIGHT)
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
			if (vagina.labiaPiercings.isPierced && (fullDesc || Utils.Rand(3) == 0))
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
			else if (vagina.isVirgin)
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

		//if one: You have a (vagina desc) with a (clit desc) (virgin desc if applicable)
		// wetness, looseness. clitcock.

		//if two: your first is a vagina desc with a clit desc (virgin desc). Your second is ...
		//wetness/looseness first, while second wetness/looseness. clitcock.

		private static string GenericPlayerStr(Vagina vagina, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			bool onlyOneVag = player.vaginas.Count == 1;
			//start with intro for this vag. 
			if (onlyOneVag)
			{
				sb.Append("You have a " + vagina.ShortDescription());
			}
			else if (vagina.vaginaIndex == 0)
			{
				sb.Append("Your first is a" + vagina.ShortDescription());
			}
			else
			{
				sb.Append("Your second is a" + vagina.ShortDescription());
			}
			//clit
			sb.Append(", with a " + vagina.clit.LongDescription());
			//virgin.
			if (vagina.isVirgin)
			{
				sb.Append("and an intact hymen");
			}
			sb.Append(". ");
			return sb.ToString();
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

		public static string AsAdjective(this VaginalWetness vaginalWetness)
		{
			switch (vaginalWetness)
			{
				case VaginalWetness.SLAVERING:
					return "absolutely drenched in fem-spunk";
				case VaginalWetness.DROOLING:
					return "dripping natural lubricant";
				case VaginalWetness.SLICK:
					return "slick with fem-spunk";
				case VaginalWetness.WET:
					return "wetter than normal";
				case VaginalWetness.DRY:
					return "rather dry";
				case VaginalWetness.NORMAL:
				default:
					return "only a little wet";
			}
		}
	}
}
