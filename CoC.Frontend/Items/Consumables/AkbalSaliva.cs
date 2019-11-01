using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class AkbalSaliva : ConsumableBase
	{
		public AkbalSaliva() : base(Short, Full, Desc)
		{

		}
		
		private static string Short()
		{
			return "AkbalSlv";
		}

		private static string Full()
		{
			return "a vial of Akbal's saliva";
		}

		private static string Desc()
		{
			return "This corked vial of Akbal's saliva is said to contain healing properties. ";
		}
		private string UseThatShit()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		public override bool countsAsSlimeLiquid => true;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 5;

		protected override int monetaryValue => 0;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			if (consumer is CombatCreature combat)
			{
				combat.AddHP(combat.maxHealth / 4);
				resultsOfUse = UseThatShit();
			}
			else
			{
				resultsOfUse = "";
			}
			isBadEnd = false;
			return true;
		}

		public override byte maxCapacityPerSlot => 5;
	}
}
