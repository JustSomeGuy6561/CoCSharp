//ConsumableBase.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:32 PM

using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;

namespace CoC.Backend.Items.Consumables
{
	//we can't enforce T->T replacement for consumables - it's just too varied, and that's really limiting. However, we don't need to - there's no storage for 'just consumables'
	//as that really wouldn't make sense anyway. The chest storage stores anything.
	public abstract class ConsumableBase : CapacityItem
	{


		protected ConsumableBase() : base()
		{ }

		//does this consumable count as liquid for slimes?
		public abstract bool countsAsLiquid { get; }
		//does this consumable count as cum for succubi?
		public abstract bool countsAsCum { get; }
		//how much hunger does consuming this sate? hunger is weird in that it counts down the hungrier you get. this is the amount less hungry you are after consuming it.
		public abstract byte sateHungerAmount { get; } //i'd make it virtual and set it to 0, but if it's virtual people are likely to miss it.
	}

	//variation of consumable that does not use a menu (unless it causes a bad end, of course) if you need a menu at all, use the consumable with menu base variant.
	public abstract class StandardConsumable : ConsumableBase
	{
		//Standard consumables break the normal convention - normally, any item that causes a bad end would have to override attempt to use themselves, and return the bad end
		//menu if necessary. However, since consumables do it so often (and as of this writing, are the only items to do so), it makes more sense to build it into the
		//standard consumable class. this means that the regular UseItem & UseItemInCombat don't give us enough information, so we can't use them. Instead,
		//we define two protected functions: ConsumeItem and CombatConsumeItem, which handle this with an extra isBadEnd out flag. they are exposed to the child classes outside
		//of this project, too.

		//unused.
		private protected override CapacityItem UseItem(Creature target, out string resultsOfUseText)
		{
			resultsOfUseText = null;
			return null;
		}

		//unused.
		private protected override CapacityItem UseItemInCombat(CombatCreature target, CombatCreature opponent, out bool resultsInLoss, out string resultsOfUseText)
		{
			resultsOfUseText = null;
			resultsInLoss = false;
			return null;
		}


		/// <summary>
		/// called when the item is actually used, or used unsuccessfully. Important distinction: this is not always immediately after the creature attempts to use it - for example, it may
		/// be delayed until after the player chooses HOW/WHERE to use it. If the implementing class overrides AttemptToUse and otherwise does not call this, it can be safely ignored.
		/// </summary>
		/// <param name="consumer"></param>
		/// <param name="resultsOfUse"></param>
		/// <returns></returns>
		protected abstract bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd);

		protected virtual bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			isCombatLoss = false;
			return OnConsumeAttempt(consumer, out resultsOfUse, out isBadEnd);
		}

		private protected override DisplayBase AttemptToUseItem(Creature target, UseItemCallback postItemUseCallback)
		{
			if (!CanUse(target, false, out string whyNot))
			{
				postItemUseCallback(false, whyNot, Author(), this);
				return null;
			}
			else
			{
				CapacityItem retVal = ConsumeItem(target, out string resultsOfUse, out bool isBadEnd);

				if (!isBadEnd)
				{
					postItemUseCallback(true, resultsOfUse, Author(), retVal);
					return null;
				}
				else
				{
					throw new System.NotImplementedException();
				}
			}
		}

		private protected override DisplayBase AttemptToUseItemInCombat(CombatCreature user, CombatCreature opponent, UseItemCombatCallback postItemUseCallback)
		{
			if (!CanUse(user, true, out string whyNot))
			{
				postItemUseCallback(false, false, whyNot, Author(), this);
				return null;
			}
			else
			{
				CapacityItem retVal = CombatConsumeItem(user, opponent, out string resultsOfUse, out bool causesCombatLoss, out bool isBadEnd);

				if (!isBadEnd)
				{
					postItemUseCallback(true, causesCombatLoss, resultsOfUse, Author(), retVal);
					return null;
				}
				else
				{
					throw new System.NotImplementedException();
				}
			}
		}

		protected virtual CapacityItem ConsumeItem(Creature target, out string resultsOfUseText, out bool isBadEnd)
		{
			bool result = OnConsumeAttempt(target, out resultsOfUseText, out isBadEnd);
			CapacityItem item = this;

			if (result)
			{
				if (target is PlayerBase player)
				{
					player.RefillHunger(sateHungerAmount);
				}
				item = null;
			}

			return item;
		}

		protected virtual CapacityItem CombatConsumeItem(CombatCreature user, CombatCreature opponent, out string resultsOfUseText, out bool causesCombatLoss, out bool isBadEnd)
		{
			bool result = OnCombatConsumeAttempt(user, opponent, out resultsOfUseText, out causesCombatLoss, out isBadEnd);
			CapacityItem item = this;

			if (result)
			{
				if (user is PlayerBase player)
				{
					player.RefillHunger(sateHungerAmount);
				}
				item = null;
			}

			return item;
		}

		public override byte maxCapacityPerSlot => 10;
	}
}
