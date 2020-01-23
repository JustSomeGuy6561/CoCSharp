//EchidnaCake.cs
//Description:
//Author: JustSomeGuy
//1/16/2020 5:19:26 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class EchidnaCake : ConsumableBase
	{
		public EchidnaCake() : base()
		{
		}

		public override string AbbreviatedName()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string ItemName()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string Appearance()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => false;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 40;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new EchidnaTfs();
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		private class EchidnaTfs : EchidnaTransformations
		{

		}
	}
}
