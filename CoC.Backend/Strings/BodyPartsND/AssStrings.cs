//ButtHipStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:05 AM


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
	}

	public sealed partial class Ass
	{
		private string shortDesc()
		{
			return AssDesc(false);
		}

		private string fullDesc()
		{
			return AssDesc(true);
		}

		private string AssDesc(bool full)
		{
			StringBuilder sb = new StringBuilder();
			//virgin looseness: 100%

			if (full || (looseness == AnalLooseness.NORMAL && (virgin || Utils.Rand(4) != 0)) || Utils.Rand(4) == 0)
			{
				sb.Append(looseness.AsDescriptor());
			}
			if ((full || Utils.Rand(3) != 0) && wetness > AnalWetness.DAMP)
			{
				if (sb.Length != 0)
				{
					sb.Append(", ");
				}
				sb.Append(wetness.AsText());
			}
			if (SaveData.BackendSessionData.data.SFW_Mode)
			{
				sb.Append(Utils.RandomChoice("rear end", "backdoor"));
			}
			else
			{
				sb.Append(Utils.RandomChoice("ass", "anus", "pucker", "backdoor", "asshole", "butthole"));
			}
			return sb.ToString();

		}

		//private string assFullDescription()
		//{

		//}
		//private string assPlayerStr(Player player)
		//{

		//}

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
			return Environment.NewLine + SafelyFormattedString.FormattedText("Your " + shortDesc() + recoverText, StringFormats.BOLD) + Environment.NewLine;
		}
	}
}
