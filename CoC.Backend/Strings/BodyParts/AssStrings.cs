//ButtHipStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:05 AM


using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public static class AssHelpers
	{
		public static string AsText(this AnalLooseness analLooseness)
		{
			switch (analLooseness)
			{
				case AnalLooseness.GAPING:
					return "gaping";
				case AnalLooseness.STRETCHED:
					return "stretched";
				case AnalLooseness.ROOMY:
					return "roomy";
				case AnalLooseness.LOOSE:
					return "loose";
				case AnalLooseness.NORMAL:
				default:
					return "normal";
			}
		}

		public static string AsDescriptor(this AnalLooseness analLooseness)
		{
			switch (analLooseness)
			{
				case AnalLooseness.GAPING:
					return "gaping";
				case AnalLooseness.STRETCHED:
					return "stretched";
				case AnalLooseness.ROOMY:
					return "roomy";
				case AnalLooseness.LOOSE:
					return "loose";
				case AnalLooseness.NORMAL:
				default:
					return "tight";
			}
		}

		public static string AsText(this AnalWetness analWetness)
		{
			switch (analWetness)
			{
				case AnalWetness.SLIME_DROOLING:
					return "slime-drooling";
				case AnalWetness.DROOLING:
					return "drooling";
				case AnalWetness.SLIMY:
					return "slimy";
				case AnalWetness.MOIST:
					return "moist";
				case AnalWetness.DAMP:
					return "damp";
				case AnalWetness.NORMAL:
				default:
					return "dry";
			}
		}

		public static string AsDescriptor(this AnalWetness analWetness)
		{
			switch (analWetness)
			{
				case AnalWetness.SLIME_DROOLING:
					return "slime-drooling";
				case AnalWetness.DROOLING:
					return "drooling";
				case AnalWetness.SLIMY:
					return "slimy";
				case AnalWetness.MOIST:
					return "moist";
				case AnalWetness.DAMP:
					return "damp";
				case AnalWetness.NORMAL:
				default:
					return "dry";
			}
		}
	}

	public sealed partial class Ass : IAss
	{
		public static string Name()
		{
			return "Ass";
		}

		public string PlayerString(PlayerBase player)
		{
			return Environment.NewLine + "You have " + FullDescription(true) + ", placed between your butt-cheeks where it belongs." + Environment.NewLine;
		}

		private string AssTightenedUpDueToInactivity(AnalLooseness currentLooseness)
		{
			string recoverText;
			if (currentLooseness <= AnalLooseness.ROOMY)
			{
				recoverText = " recovers from your ordeals, tightening up a bit.";
			}
			else if (currentLooseness == AnalLooseness.STRETCHED)
			{
				recoverText = " recovers from your ordeals and becomes tighter.";
			}
			else
			{
				recoverText = " recovers from the brutal stretching it has received and tightens up.";
			}
			return Environment.NewLine + SafelyFormattedString.FormattedText("Your " + LongDescription() + recoverText, StringFormats.BOLD) + Environment.NewLine;
		}
	}

	internal interface IAss
	{
		bool virgin { get; }
		AnalLooseness looseness { get; }
		AnalWetness wetness { get; }

		AssholeLocation location { get; }

		bool everPracticedAnal { get; }


	}

	internal static class AssStrings
	{
		//for consistency, you can set it to single item format for special formatting. afaik this probably wont be used but whatever.
		public static string ShortDescription(bool singleItemFormat = false)
		{
			if (SaveData.BackendSessionSave.data.SFW_Mode == true)
			{
				string intro = singleItemFormat ? "a " : "";
				return intro + Utils.RandomChoice("rear end", "backdoor");
			}
			else if (!singleItemFormat)
			{
				return Utils.RandomChoice("ass", "anus", "pucker", "backdoor", "asshole", "butthole");
			}
			else
			{
				return Utils.RandomChoice("an ass", "an anus", "a pucker", "a backdoor", "an asshole", "a butthole");
			}
		}

		public static string LongDescription(IAss ass, bool alternateFormat)
		{
			return AssDesc(ass, alternateFormat, false);
		}

		public static string FullDescription(IAss ass, bool alternateFormat)
		{
			return AssDesc(ass, alternateFormat, true);
		}

		private static string AssDesc(IAss ass, bool alternateFormat, bool full)
		{
			StringBuilder sb = new StringBuilder();
			//virgin looseness: 100%

			if (full || ass.virgin || Utils.Rand(4) == 0)
			{
				sb.Append(Utils.AddArticleIf(ass.looseness.AsDescriptor(), alternateFormat));
			}
			if (ass.wetness > AnalWetness.DAMP && (full || ass.virgin || Utils.Rand(3) != 0))
			{
				if (sb.Length != 0)
				{
					sb.Append(", ");
					sb.Append(ass.wetness.AsDescriptor());
				}
				else
				{
					sb.Append(Utils.AddArticleIf(ass.wetness.AsDescriptor(), alternateFormat));
				}
			}
			if (!ass.everPracticedAnal)
			{
				if (sb.Length != 0)
				{
					sb.Append(" ");
				}
				else if (alternateFormat)
				{
					sb.Append("a ");
				}
				sb.Append("true virgin");
			}
			else if (ass.virgin)
			{
				if (sb.Length != 0)
				{
					sb.Append(" ");
				}
				else if (alternateFormat)
				{
					sb.Append("a ");
				}
				sb.Append("virgin");
			}
			if (sb.Length != 0)
			{
				sb.Append(" ");
			}
			else if (alternateFormat)
			{
				sb.Append("a ");
			}
			sb.Append(ShortDescription());
			return sb.ToString();

		}
	}
}
