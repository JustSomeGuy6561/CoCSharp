//Slut.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM


namespace CoC.Frontend.Perks.History
{
	//minor rework/buff: in addition to bonus capacity, it now allows voluntary sex at lower lust levels (-5).

	public sealed partial class Slut : HistoryPerkBase
	{
		public Slut() : base(SlutStr, SlutBtn, SlutHint, SlutDesc)
		{
		}

		protected override void OnActivation()
		{
			baseModifiers.PerkBasedBonusAnalCapacity += 20;
			baseModifiers.PerkBasedBonusVaginalCapacity += 20;
			extraModifiers.lustRequiredForSexOffset -= 5;
		}

		protected override void OnRemoval()
		{
			baseModifiers.PerkBasedBonusAnalCapacity -= 20;
			baseModifiers.PerkBasedBonusVaginalCapacity -= 20;
			extraModifiers.lustRequiredForSexOffset += 5;
		}
	}
}
