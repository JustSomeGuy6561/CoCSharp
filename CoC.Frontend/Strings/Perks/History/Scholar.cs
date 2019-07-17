//Scholar.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

using CoC.Frontend.SaveData;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Scholar : HistoryPerkBase
	{
		private static bool silly => FrontendSessionSave.data.SillyMode;
		private static string ScholarStr()
		{
			return "History: Scholar";
		}
		private static string ScholarBtn()
		{
			return "Schooling";
		}
		private static string ScholarHint()
		{
			return "You spent much of your time in school, and even begged the richest man in town, Mr. " + (silly ? "Savin" : "Sellet") +
				", to let you read some of his books. You are much better at focusing, and spellcasting uses 20% less fatigue. Is this your history?";
		}
		private static string ScholarDesc()
		{
			return "Time spent focusing your mind makes spellcasting 20% less fatiguing.";
		}
	}
}
