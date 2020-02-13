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
		protected const int DEFAULT_VALUE = 6;

		protected ConsumableBase() : base()
		{ }

		//does this consumable count as liquid for slimes?
		public abstract bool countsAsLiquid { get; }
		//does this consumable count as cum for succubi?
		public abstract bool countsAsCum { get; }
		//how much hunger does consuming this sate? hunger is weird in that it counts down the hungrier you get. this is the amount less hungry you are after consuming it.
		public abstract byte sateHungerAmount { get; } //i'd make it virtual and set it to 0, but if it's virtual people are likely to miss it.
	}

	//variation of consumable that does not use a menu.
	public abstract class StandardConsumable : ConsumableBase
	{
		/// <summary>
		/// called when the item is actually used, or used unsuccessfully. Important distinction: this is not always immediately after the creature attempts to use it - for example, it may
		/// be delayed until after the player chooses HOW/WHERE to use it. If the implementing class overrides AttemptToUse and otherwise does not call this, it can be safely ignored.
		/// </summary>
		/// <param name="consumer"></param>
		/// <param name="resultsOfUse"></param>
		/// <returns></returns>
		protected abstract bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd);

		protected virtual bool OnCombatConsumeAttempt(CombatCreature consumer, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			isCombatLoss = false;
			return OnConsumeAttempt(consumer, out resultsOfUse, out isBadEnd);
		}

		public override DisplayBase AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			bool result = OnConsumeAttempt(target, out string consumeResults, out bool isBadEnd);
			CapacityItem item = this;

			if (result)
			{
				if (target is PlayerBase player)
				{
					player.refillHunger(sateHungerAmount);
				}
				item = null;
			}

			if (isBadEnd)
			{
				throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
				//if we hit a bad end, don't resume whatever we were doing - we treat it as if it was a nightmare and nothing happened, except for the stuff that did happen, because continuity
				//is hard to enforce in this shit. Just lampshade it - it was a nightmare, but you still suffer the effects of it as if it happened, but only up until the point you realized it
				//was a bad end. so, you'll lose any items you were gonna get afterward, etc, but if your butt was stretched to gaping, it'll still be gaping after resuming from the bad end.
				//same with piercings, tfs, etc. No time is lost, however, just resume from camp as soon as possible.
				//example of lampshading: "it was just a nightmare... but it felt so real - and your eyebrow has a piercing in it, just like in the dream. strange..."

				//GameEngine.DoBadEnd();
			}
			else
			{
				postItemUseCallback(result, consumeResults, Author(), item);
				return null;
			}
		}

		public override DisplayBase AttemptToUseInCombat(CombatCreature target, UseItemCombatCallback postItemUseCallback)
		{
			bool result = OnCombatConsumeAttempt(target, out string consumeResults, out bool isLoss, out bool isBadEnd);
			CapacityItem item = this;

			if (result)
			{
				if (target is PlayerBase player)
				{
					player.refillHunger(sateHungerAmount);
				}
				item = null;
			}

			if (isBadEnd)
			{
				throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
				//if we hit a bad end, don't resume whatever we were doing - we treat it as if it was a nightmare and nothing happened, except for the stuff that did happen, because continuity
				//is hard to enforce in this shit. Just lampshade it - it was a nightmare, but you still suffer the effects of it as if it happened, but only up until the point you realized it
				//was a bad end. so, you'll lose any items you were gonna get afterward, etc, but if your butt was stretched to gaping, it'll still be gaping after resuming from the bad end.
				//same with piercings, tfs, etc. No time is lost, however, just resume from camp as soon as possible.
				//example of lampshading: "it was just a nightmare... but it felt so real - and your eyebrow has a piercing in it, just like in the dream. strange..."

				//GameEngine.DoBadEnd();
			}
			else
			{
				postItemUseCallback(result, isLoss, consumeResults, Author(), item);
				return null;
			}
		}

		public override byte maxCapacityPerSlot => 10;
	}
}
