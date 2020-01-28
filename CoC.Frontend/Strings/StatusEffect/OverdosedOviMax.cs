using CoC.Backend;
using CoC.Backend.StatusEffect;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.StatusEffect
{
	public partial class OverdosedOviMax
	{
		private static string OviMaxODText()
		{
			return "OviMax Overdose";
		}

		private string OverdoseText()
		{
			return "You're now suffering from the effects of overdosing on OviMax Elixirs!";
		}
		private string ODShort()
		{
			string warning;
			if (overdoseCount < 2)
			{
				warning = " Egg pregnancies have a chance of additional effects";
			}
			else if (overdoseCount < 3)
			{
				warning = " Egg pregnancies have a good chance of additional effects";
			}
			else
			{
				warning = " Egg pregnancies will have additional effects";
			}
			return SafelyFormattedString.FormattedText("You are under the effects of excess OviMax intake.", StringFormats.BOLD) + warning + ".";
		}
	}
}
