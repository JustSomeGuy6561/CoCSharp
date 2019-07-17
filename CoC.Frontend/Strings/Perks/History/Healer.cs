//Healer.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Healer : HistoryPerkBase
	{
		private static string HealerStr()
		{
			return "History: Healer";
		}
		private static string HealerBtn()
		{
			return "Healing";
		}
		private static string HealerHint()
		{
			return "You often spent your free time with the village healer, learning how to tend to wounds. Healing items and effects are 20% more effective. Is this your history?";
		}
		private static string HealerDesc()
		{
			return "Healing experience increases HP gains from recovery items or spells by 20%.";
		}
	}
}
