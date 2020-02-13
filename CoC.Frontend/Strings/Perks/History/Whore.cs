//Whore.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Whore : HistoryPerkBase
	{
		private static string WhoreStr()
		{
			return "History: Whore";
		}
		private static string WhoreBtn()
		{
			return "Whoring";
		}
		private static string WhoreHint()
		{
			return "You managed to find work as a whore. Because of your time spent trading seduction for profit, you're more effective at teasing (+15% tease damage). Is this your history?";
		}
		private static string WhoreDesc()
		{
			return "Seductive experience causes your tease attacks to be 15% more effective.";
		}
	}
}
