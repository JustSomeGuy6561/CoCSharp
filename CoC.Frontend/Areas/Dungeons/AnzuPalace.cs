using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Places
{
	//internal partial class AnzuPalace : DungeonBase
	//{
	//	public AnzuPalace() : base(AnzuPalaceName, GetTriggeredEncounters())
	//	{
	//	}

	//	private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
	//	{
	//		return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
	//	}

	//	static AnzuPalace()
	//	{

	//	}

	//	public override bool isUnlocked
	//	{
	//		get => FrontendSessionSave.data.AnzuPalaceUnlocked;
	//		protected set => FrontendSessionSave.data.AnzuPalaceUnlocked = value;
	//	}

	//	public override bool isDisabled
	//	{
	//		get => FrontendSessionSave.data.AnzuPalaceDisabled;
	//		protected set => FrontendSessionSave.data.AnzuPalaceDisabled = value;
	//	}

	//	public override int timesExplored
	//	{
	//		get => FrontendSessionSave.data.AnzuPalaceExplorationCount;
	//		protected set => FrontendSessionSave.data.AnzuPalaceExplorationCount = value;
	//	}

	//	protected override void ExplorePlace()
	//	{
	//		throw new NotImplementedException();
	//	}

	//	protected override SimpleDescriptor UnlockText => AnzuPalaceUnlockText;
	//}
}
