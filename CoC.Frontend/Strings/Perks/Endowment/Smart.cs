//Smart.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Smart : EndowmentPerkBase
	{
		private static string SmartStr()
		{
			return "Smart";
		}
		private static string SmartBtn()
		{
			return "Smarts";
		}
		private static string SmartHint()
		{
			return "Are you a quick learner? (+5 Intellect)" + Environment.NewLine + Environment.NewLine + 
				"Intellect can help you avoid dangerous monsters or work with machinery. It will also boost the power of any spells you may learn in your travels.";
		}
		private static string SmartDesc()
		{
			return "Increases minimum intelligence. Gains intelligence 25% faster.";
		}
	}
}
