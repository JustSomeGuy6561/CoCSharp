using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoC.Backend.Areas
{
	//base class for a home base. it's basically a "place" but it also has saveable options - basically, when a game loads, we don't want to run the same checks as we would
	//when we are returning to base, because that would incorrectly assume we left the base and thus some checks or rng would be altered to return bad or incorrect data.
	//Thus, "OnReload"

	public abstract class HomeBaseBase : AreaBase
	{
#warning visitors will be lost if the home base is changed. perhaps implement a way to transfer them? should be in area engine when changing home base.
		//home base is always kept in scope (because it's almost always in use), so we don't need to worry about losing them like we normally would with area changes.
		public IEnumerable<ICampNPC> visitors => _slaves.Union(_followers.ConvertAll(x=>(ICampNPC)x)).Union(_lovers.ConvertAll(x=>(ICampNPC)x));
		public bool hasAnyVisitors => _slaves.Count > 0 || _followers.Count > 0 || _lovers.Count > 0;
		private readonly List<ICampSlaveCreature> _slaves = new List<ICampSlaveCreature>();
		public readonly ReadOnlyCollection<ICampSlaveCreature> slaves;

		private readonly List<ICampFollowerCreature> _followers = new List<ICampFollowerCreature>();
		public readonly ReadOnlyCollection<ICampFollowerCreature> followers;

		private readonly List<ICampLoverCreature> _lovers = new List<ICampLoverCreature>();
		public readonly ReadOnlyCollection<ICampLoverCreature> lovers;

		private DisplayBase currentDisplay => GetCurrentDisplay();

		protected HomeBaseBase(SimpleDescriptor areaName) : base(areaName)
		{
			lovers = new ReadOnlyCollection<ICampLoverCreature>(_lovers);
			slaves = new ReadOnlyCollection<ICampSlaveCreature>(_slaves);
			followers = new ReadOnlyCollection<ICampFollowerCreature>(_followers);
		}

		/// <summary>
		/// Called when the game reloads the home base.This can occur after loading a Save, or when changing Languages.
		/// </summary>
		protected internal virtual void OnReload()
		{
			RunAreaWithCurrentDisplay(true);
		}

		protected Action lastAction;

		internal override void RunArea()
		{
			RunAreaWithCurrentDisplay(false);
		}

		protected virtual void RunAreaWithCurrentDisplay(bool isReload)
		{
			currentDisplay.ClearOutput();

			//first the buttons.
			currentDisplay.AddButton(0, new ButtonData(ExploreString(), true, DoExplore));
			currentDisplay.AddButton(1, new ButtonData(PlaceString(), placeMenuUnlocked, DoPlaceMenu));
			currentDisplay.AddButton(2, new ButtonData(InventoryString(), true, DoInventory));
			currentDisplay.AddButton(3, new ButtonData(StashString(), anyStashesUnlocked, DoStashMenu));
			currentDisplay.AddButton(4, new ButtonData(CampActionString(), true, DoCampActions));
			if (anyLoversUnlocked)
			{
				currentDisplay.AddButton(5, new ButtonData(LoversString(), true, DoLoversMenu));
			}
			if (anyFollowersUnlocked)
			{
				currentDisplay.AddButton(6, new ButtonData(FollowersString(), true, DoFollowersMenu));
			}
			if (anySlavesUnlocked)
			{
				currentDisplay.AddButton(7, new ButtonData(SlavesString(), true, DoSlavesMenu));
			}

			//add the masturbate button.

			//add the sleep button.

			//then the display data.
			currentDisplay.OutputText(CampDescription(isReload));
		}

		private void DoSlavesMenu()
		{
			currentDisplay.ClearOutput();

			//display generic slaves text.

			AddReturnButtonToDisplay(); //add the return button.


			var maker = new ButtonListMaker(currentDisplay);

			foreach (var slave in slaves)
			{
				maker.AddButtonToList(slave.Name(), true, slave.OnSelect);
			}

			maker.CreateButtons(false);
		}



		private void DoFollowersMenu()
		{
			currentDisplay.ClearOutput();

			//display generic follower text.

			AddReturnButtonToDisplay(); //add the return button.

			var maker = new ButtonListMaker(currentDisplay);

			foreach (var follower in followers)
			{
				maker.AddButtonToList(follower.Name(), true, follower.OnSelect);
			}

			maker.CreateButtons(true);
		}

		private void DoLoversMenu()
		{
			currentDisplay.ClearOutput();
			//display generic lovers text.

			AddReturnButtonToDisplay(); //add the return button.


			var maker = new ButtonListMaker(currentDisplay);

			foreach (var lover in lovers)
			{
				maker.AddButtonToList(lover.Name(), true, lover.OnSelect);
			}

			maker.CreateButtons(true);
		}

		private void DoCampActions()
		{
			currentDisplay.ClearOutput();
			LoadUniqueCampActionsMenu(currentDisplay);
		}

		protected abstract void LoadUniqueCampActionsMenu(DisplayBase currentDisplay);

		//
		protected abstract string CampDescription(bool isReload);

		private void DoStashMenu()
		{

		}

		private void DoInventory()
		{

		}

		private void DoPlaceMenu()
		{
			currentDisplay.ClearOutput();
			//append the places text to the current display.

			AddReturnButtonToDisplay(); //add the return button.

			ButtonListMaker listMaker = new ButtonListMaker(currentDisplay);

			foreach (var item in GameEngine.GetUnlockedPlaces())
			{
				listMaker.AddButtonToList(item.Value, true, () => GameEngine.GoToAreaAndRun(item.Key));
			}

			listMaker.CreateButtons(true); //create the list of buttons, with a reserved final button as given.
		}

		private bool anyStashesUnlocked => GameEngine.currentlyControlledCharacter.hasAnyStashes;

		private bool anyAreasUnlocked => anyLocationsUnlocked || placeMenuUnlocked;

		private bool placeMenuUnlocked => GameEngine.anyUnlockedPlaces || GameEngine.anyUnlockedDungeons;
		private bool anyLocationsUnlocked => GameEngine.anyUnlockedLocations;

		private bool anyLoversUnlocked => visitors.Any(x => x is ICampLoverCreature);

		private bool anyFollowersUnlocked => visitors.Any(x => x is ICampFollowerCreature);

		private bool anySlavesUnlocked => visitors.Any(x => x is ICampSlaveCreature);

		private string SlavesString()
		{
			return "Slaves";
		}

		private string FollowersString()
		{
			return "Followers";
		}

		private string LoversString()
		{
			return "Lovers";
		}

		private string CampActionString()
		{
			return "Camp Actions";
		}

		private string StashString()
		{
			return "Stash";
		}

		private string InventoryString()
		{
			return "Inventory";
		}

		private string PlaceString()
		{
			return "Places";
		}

		private string ExploreString()
		{
			return "Explore";
		}

		private void DoExplore()
		{
			currentDisplay.ClearOutput();

			if (!anyLocationsUnlocked)
			{
				Exploration.RunArea(currentDisplay);
			}
			else
			{
				//append the explore text to the current display.

				AddReturnButtonToDisplay(); //add the return button.

				ButtonListMaker listMaker = new ButtonListMaker(currentDisplay);
				listMaker.AddButtonToList(ExploreString(), true, () => Exploration.RunArea(currentDisplay));

				foreach (var item in GameEngine.GetUnlockedLocations())
				{
					listMaker.AddButtonToList(item.Value, true, () => GameEngine.GoToAreaAndRun(item.Key));
				}

				listMaker.CreateButtons(true); //create the list of buttons, with a reserved final button as given.
			}
		}

		protected void AddReturnButtonToDisplay()
		{
			currentDisplay.AddButton(14, new ButtonData(GlobalStrings.BACK(), true, ()=> RunAreaWithCurrentDisplay(false)), true);
		}
	}
}
