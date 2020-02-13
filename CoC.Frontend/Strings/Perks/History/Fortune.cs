//Fortune.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM


namespace CoC.Frontend.Perks.History
{
	public sealed partial class Fortune : HistoryPerkBase
	{
		private static string FortuneStr()
		{
			return "History: Fortune";
		}
		private static string FortuneBtn()
		{
			return "Fortune";
		}
		private static string FortuneHint()
		{
			return "You always feel lucky when it comes to fortune. Because of that, you have always managed " +
				"to save up gems until whatever's needed and how to make the most out it (+15% gems on victory). " +
				"You will also start out with 250 gems.Is this your history?";
		}
		private static string FortuneDesc()
		{
			return "Your luck and skills at gathering currency allows you to get 15% more gems from victories.";
		}
	}
}
