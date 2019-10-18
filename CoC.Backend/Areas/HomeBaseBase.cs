using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System.Collections.Generic;

namespace CoC.Backend.Areas
{
	//base class for a home base. it's basically a "place" but it also has saveable options - basically, when a game loads, we don't want to run the same checks as we would
	//when we are returning to base, because that would incorrectly assume we left the base and thus some checks or rng would be altered to return bad or incorrect data. 
	//Thus, "OnReload"
	public abstract class HomeBaseBase : AreaBase
	{
		public readonly List<Creature> Visitors = new List<Creature>();

		protected HomeBaseBase(SimpleDescriptor areaName) : base(areaName)
		{
		}

		/// <summary>
		/// Called when the game reloads the home base.This can occur after loading a Save, or when changing Languages. 
		/// </summary>
		protected internal abstract void OnReload();

		internal override DisplayBase RunArea()
		{
			var areaPage = pageMaker();
			//ToDo: add the buttons and all their magic.

			areaPage.OutputText("You're running the home base! yay!");


			//ToDo: handle any home base reaction parsing magic. 

			//foreach visitor: 


			foreach (var npc in Visitors)
			{
				SimpleDescriptor descriptor = OverrideDefaultIdleTextForCampNPC(npc, GameEngine.CurrentHour);
				if (descriptor is null)
				{
					descriptor = ((ICampNPC)npc).idleBaseDescription(GameEngine.CurrentHour);
				}
				areaPage.OutputText(descriptor());
			}

			//load the various buttons.
			return areaPage;
		}

		//if this location has special text for a given NPC, you have the opportunity to return a string pointer referring to that text.
		//otherwise, return null. if the return value is null, the default text will instead be used. 
		protected abstract SimpleDescriptor OverrideDefaultIdleTextForCampNPC(Creature creature, byte currentHour);

	}
}
