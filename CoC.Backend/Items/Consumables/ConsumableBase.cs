//ConsumableBase.cs
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
		protected ConsumableBase(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		//does this consumable count as liquid for slimes?
		public abstract bool countsAsSlimeLiquid { get; }
		//does this consumable count as cum for succubi?
		public abstract bool countsAsCum { get; }
		//how much hunger does consuming this sate? hunger is weird in that it counts down the hungrier you get. this is the amount less hungry you are after consuming it.
		public abstract byte sateHungerAmount { get; } //i'd make it virtual and set it to 0, but if it's virtual people are likely to miss it. 

		protected abstract bool OnConsumeAttempt(Creature consumer, out string resultsOfUse);

		//note: in the rare event an item returns another item (kitsune gift is all i can think of) override this, with postItemUseCallback(true, returnedItem);
		public override void AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			if (OnConsumeAttempt(target, out string consumeResults))
			{
				if (target is Player player)
				{
					player.refillHunger(sateHungerAmount);
				}
				//RaiseItemUsed(target, this);
				postItemUseCallback(true, consumeResults, null);
			}
			else
			{
				postItemUseCallback(false, consumeResults, null);
			}
		}

		public override byte maxCapacityPerSlot => 10;
	}
}
