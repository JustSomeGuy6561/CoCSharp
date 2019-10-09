//ConsumableBase.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:32 PM

using CoC.Backend.Creatures;

namespace CoC.Backend.Items.Consumables
{
	public abstract class ConsumableBase : CapacityItem
	{
		protected ConsumableBase(SimpleDescriptor shortName, SimpleDescriptor fullName) : base(shortName, fullName)
		{
		}

		//does this consumable count as liquid for slimes?
		public abstract bool countsAsSlimeLiquid { get; }
		//does this consumable count as cum for succubi?
		public abstract bool countsAsCum { get; }
		//how much hunger does consuming this sate? hunger is weird in that it counts down the hungrier you get. this is the amount less hungry you are after consuming it.
		public abstract byte sateHungerAmount { get; } //i'd make it virtual and set it to 0, but if it's virtual people are likely to miss it. 

		protected abstract void OnConsume(Creature consumer);

		//note: in the rare event an item returns another item (kitsune gift is all i can think of) override this, with postItemUseCallback(true, returnedItem);
		public override void AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			OnConsume(target);

			postItemUseCallback(true, null);
		}

		public override byte maxCapacityPerSlot => 10;
	}
}
