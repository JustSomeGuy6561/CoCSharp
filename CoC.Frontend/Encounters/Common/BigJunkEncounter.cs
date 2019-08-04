//BigJunkEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:49 PM
using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using System;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Encounters.Common
{
	internal sealed class BigJunkEncounter : RandomEncounter
	{

		private readonly SimpleDescriptor bigJunkText;

		public BigJunkEncounter(SimpleDescriptor flavorText) : base()
		{
			bigJunkText = flavorText;
		}
		protected override int chances => bigJunkChance();

		private int bigJunkChance()
		{
			//if (player.largestCock > player.height && player.totalCockGirth >= 12)
			//{
			//	return (int)Math.Floor(20 * (1 + (player.LargestCock() - player.heuight) / 25.0));
			//}
			return 0;
		}

		protected override void Run()
		{
			//changeStats;
			AddOutput(bigJunkText);
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			//return player.largestCock > player.height && player.totalCockGirth >= 12;
			return false;
		}
	}
}
