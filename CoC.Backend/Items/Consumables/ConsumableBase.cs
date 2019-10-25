﻿//ConsumableBase.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:32 PM

using CoC.Backend.Creatures;

namespace CoC.Backend.Items.Consumables
{
	//we can't enforce T->T replacement for consumables - it's just too varied, and that's really limiting. However, we don't need to - there's no storage for 'just consumables'
	//as that really wouldn't make sense anyway. The chest storage stores anything. 
	public abstract class ConsumableBase : CapacityItem
	{
		protected const int DEFAULT_VALUE = 6;

		protected ConsumableBase(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		//does this consumable count as liquid for slimes?
		public abstract bool countsAsSlimeLiquid { get; }
		//does this consumable count as cum for succubi?
		public abstract bool countsAsCum { get; }
		//how much hunger does consuming this sate? hunger is weird in that it counts down the hungrier you get. this is the amount less hungry you are after consuming it.
		public abstract byte sateHungerAmount { get; } //i'd make it virtual and set it to 0, but if it's virtual people are likely to miss it. 

		/// <summary>
		/// called when the item is actually used, or used unsuccessfully. Important distinction: this is not always immediately after the creature attempts to use it - for example, it may 
		/// be delayed until after the player chooses HOW/WHERE to use it. If the implementing class overrides AttemptToUse and otherwise does not call this, it can be safely ignored. 
		/// </summary>
		/// <param name="consumer"></param>
		/// <param name="resultsOfUse"></param>
		/// <returns></returns>
		protected abstract bool OnConsumeAttempt(Creature consumer, out string resultsOfUse);

		//note: consumables that require a menu will need to overwrite this. items that return another item will, as well. 
		public override void AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			bool result = OnConsumeAttempt(target, out string consumeResults);
			CapacityItem item = this;

			if (result)
			{
				if (target is PlayerBase player)
				{
					player.refillHunger(sateHungerAmount);
				}
				item = null;
			}

			postItemUseCallback(result, consumeResults, item);
		}

		public override byte maxCapacityPerSlot => 10;
	}
}
