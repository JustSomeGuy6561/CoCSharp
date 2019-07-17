//Slacker.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Slacker : HistoryPerkBase
	{
		private static string SlackerStr()
		{
			return "History: Slacker";
		}
		private static string SlackerBtn()
		{
			return "Slacking";
		}
		private static string SlackerHint()
		{
			return "You spent a lot of time slacking, avoiding work, and otherwise making a nuisance of yourself. Your efforts at slacking have made" +
				" you quite adept at resting, meaning you recover your stamina 20% faster.  Is this your history?";
		}
		private static string SlackerDesc()
		{
			return "Time spent relaxing means you recover your stamina 20% faster.";
		}
	}
}
