//Smith.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Smith : HistoryPerkBase
	{
		private static string SmithStr()
		{
return "History: Smith";
		}
		private static string SmithBtn()
		{
			return "Smithing";
		}
		private static string SmithHint()
		{
			return "You managed to get an apprenticeship with the local blacksmith. Because of your time spent at the blacksmith's side, " +
				"you've learned how to fit armor for maximum protection. Is this your history?";
		}
		private static string SmithDesc()
		{
			return "Knowledge of armor and fitting increases armor effectiveness by roughly 10%.";
		}
	}
}
