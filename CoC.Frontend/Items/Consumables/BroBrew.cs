//BroBrew.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:32 PM

using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Perks;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class BroBrew : ConsumableBase
	{
		public BroBrew() : base(Short, Full, Desc)
		{
		}

		private static string Short()
		{
			return "BroBrew";
		}

		private static string Full()
		{
			return "a can of Bro Brew";
		}

		private static string Desc()
		{
			return "This aluminum can is labelled as 'Bro Brew'. It even has a picture of a muscly, " +
				"bare-chested man flexing on it.  A small label in the corner displays: \"Demon General's Warning: " +
				"Bro Brew's effects are as potent (and irreversible) as they are refreshing.\"";
		}

		private string OutputThatShit()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//does this consumable count as liquid for slimes?
		public override bool countsAsSlimeLiquid => true;
		//does this consumable count as cum for succubi?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 30;

		protected override int monetaryValue => 1000;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse)
		{
			if (consumer.perks.HasPerk<BimBro>())
			{
				consumer.perks.GetPerk<BimBro>().Broify();
			}
			else
			{
				consumer.perks.AddPerk(new BimBro(Gender.MALE));
			}

			resultsOfUse = OutputThatShit();
			return true;
		}
	}
}
