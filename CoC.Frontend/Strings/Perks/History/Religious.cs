//Religious.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Religious : HistoryPerkBase
	{
		private static string ReligiousStr()
		{
			return "History: Religious";
		}
		private static string ReligiousBtn()
		{
			return "Religion";
		}
		private static string ReligiousHint()
		{
			return "You spent a lot of time at the village temple, and learned how to meditate. The 'masturbation' option is replaced with 'meditate' when corruption is at or below 66. Is this your history?";
		}
		private static string ReligiousDesc()
		{
			return "Replaces masturbate with meditate when corruption less than or equal to 66. Reduces minimum libido slightly.";
		}
	}
}
