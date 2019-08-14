using CoC.Backend.Creatures;
using CoC.Backend.Engine;
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

		internal override void RunArea()
		{
			//ToDo: add the buttons and all their magic.
			//foreach visitor: 
			foreach (var npc in Visitors)
			{
				SimpleDescriptor descriptor = OverrideDefaultIdleTextForCampNPC(npc, GameEngine.CurrentHour);
				if (descriptor is null)
				{
					descriptor = ((ICampNPC)npc).idleBaseDescription(GameEngine.CurrentHour);
				}

			}
		}

		//if this location has special text for a given NPC, you have the opportunity to return a string pointer referring to that text.
		//otherwise, return null. if the return value is null, the default text will instead be used. 
		protected abstract SimpleDescriptor OverrideDefaultIdleTextForCampNPC(Creature creature, byte currentHour);

	}
}
