//ConsumableBase.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:32 PM

namespace CoC.Backend.Items.Consumables
{
	public abstract class ConsumableBase
	{
		//does this consumable count as liquid for slimes?
		public abstract bool countsAsSlimeLiquid { get; }
		//how much hunger does consuming this sate? hunger is weird in that it counts down the hungrier you get. this is the amount less hungry you are after consuming it.
		public abstract byte sateHungerAmount { get; } //i'd make it virtual and set it to 0, but if it's virtual people are likely to miss it. 


	}
}
