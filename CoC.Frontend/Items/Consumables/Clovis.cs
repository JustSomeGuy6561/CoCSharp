//Clovis.cs
//Description:
//Author: JustSomeGuy
//11/3/2019 12:02:07 AM

using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class Clovis : ConsumableBase
	{
		public Clovis() : base(Short, Full, Desc)
		{
		}

		private static string Short()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Full()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Desc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string UseItemText()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => throw new NotImplementedException();
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => throw new NotImplementedException();
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => throw new NotImplementedException();

		protected override int monetaryValue => throw new NotImplementedException();

		public override bool CanUse(Creature target, out string whyNot)
		{
			throw new NotImplementedException();
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			throw new NotImplementedException();
			resultsOfUse = UseItemText();
			isBadEnd = false;
			return true;
		}
	}
}
