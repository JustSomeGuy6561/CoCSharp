//Slut.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM


using CoC.Backend.Perks;

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
			AddModifierToPerk(baseModifiers.perkBasedBonusAnalCapacity, new ValueModifierStore<ushort>(ValueModifierType.FLAT_ADD, 20));
			AddModifierToPerk(baseModifiers.perkBasedBonusVaginalCapacity, new ValueModifierStore<ushort>(ValueModifierType.FLAT_ADD, 20));

			if (hasExtraModifiers)
			{
				AddModifierToPerk(extraModifiers.lustRequiredForSexOffset, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, -5));
				AddModifierToPerk(extraModifiers.isASlut, true);
			}
		}

		protected override void OnRemoval()
		{ }
	}
}
