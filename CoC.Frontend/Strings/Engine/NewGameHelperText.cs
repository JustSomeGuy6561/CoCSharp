//NewGameHelperText.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 9:28 PM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Strings.Engine
{
	internal static class NewGameHelperText
	{
		internal static string IntroText()
		{
			return "You grew up in the small village of Ingnam, a remote village with rich traditions, buried deep in the wilds. " +
				"Every year for as long as you can remember, your village has chosen a champion to send to the cursed Demon Realm. " +
				"Legend has it that in years Ingnam has failed to produce a champion, chaos has reigned over the countryside. " +
				"Children disappear, crops wilt, and disease spreads like wildfire. This year, <b>you</b> have been selected to be the champion." +
				Environment.NewLine + Environment.NewLine + "What is your name?";
		}

		internal static string PromptSpecial()
		{
			return "This name, like you, is special. Do you live up to your name or continue on, assuming it to be coincidence?" +
				" Note that some special characters may limit your initial customization options, though these can be changed during gameplay.";
		}

		internal static string ContinueOn()
		{
			return "Continue On";
		}

		internal static string SpecialName()
		{

			return "SpecialName";
		}


	}
}
