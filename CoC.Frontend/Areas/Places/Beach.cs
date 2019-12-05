using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Areas.Places
{
	//idea: beach is made/maintained by a tentacle creature, Nakborus, who has obvious alterior motives for its existence. At the beginning she's more or
	//less not really doing much, which makes the whole deserted beach thing a self-fulfilling prophecy. You have to get her back into it.
	//Nakborus: Female, but tentacles are more or less dicks - she gets pleasure from using them as such. But she's not really a huge tentacle pervert
	//monster, and she's actually a little sensitive to the fact everyone thinks so. It just so happens most people react in one of two ways: abject terror
	//at the rapey tentacle monster, or like huge sluts who are into that kind of thing. She kinda wants someone to see her as more than just a tentacle
	//monster, so there's the PC's in. Of course, if you're really into tentacle rape, i suppose another option would be to get her to accept her tentacle
	//monster-ness and just go crazy with the sexings. Originally some sort of squid-human (feel free to add splatoon reference), but corruption.

	//Lifeguard: Male, Mostly Human. Has webbed feet, gills. Got these from Nakborus - part of the deal for being the lifeguard.
	//If you ask, he'll say something along the lines of "...I'd rather not talk about it" and subconciously rub his backside.
	//3 Piercings in cock: Albert and 2 frenums at the top. His cock piercings can be updated by interractions: if you don't like them, he could be convinced
	//to remove all but the prince albert, though he'll complain half-heartedly. Alternatively, can be convinced to add frenum piercings over time, until full ladder.
	//he'll be hesitant at first, but will do so more readily later. Nipple piercings can be added at any time, could have scene with them if desired.
	//Bisexual, prefers to penetrate, but can convince him to take it in the butt if friendly enough. Rape is an option, i guess, but will disable most of the beach.
	//context is was straight, last beach goer was a trap but he was lonely and bored and so now he'll hit that too. If PC is trap, may have special scenes.
	//picked up weird quirk as a result of trap - defaults to penetrating ass, even on female/herms, and may accidently use the wrong hole when asking for vaginal.
	internal partial class Beach : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public Beach() : base(BeachName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static Beach()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.BeachUnlocked;
			protected set => FrontendSessionSave.data.BeachUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.BeachDisabled;
			protected set => FrontendSessionSave.data.BeachDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.BeachExplorationCount;
			protected set => FrontendSessionSave.data.BeachExplorationCount = value;
		}

		protected override void ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => BeachUnlockText;
	}
}
