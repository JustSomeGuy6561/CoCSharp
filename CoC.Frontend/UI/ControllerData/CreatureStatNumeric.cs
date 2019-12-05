using CoC.Backend;
using System;

namespace CoC.Frontend.UI.ControllerData
{
	//Note: The Creature stat has a "stat Name". The GUI uses this ONLY AS A LOOKUP/FALLBACK VALUE.
	//English text is hard-coded in every function so if something is not translated, it will fallback to displaying it in English - it's
	//much easier to debug and find what wasn't translated when you can search for the text that did display and see why it wasn't translated.
	//but this is all it's used for.

	public class CreatureStatNumeric : CreatureStatBase
	{
		public uint current { get; internal set; } = 0;

		/// <summary>
		/// Should the UI tell the player that this stat changed? note that this is only a hint; the UI can ignore it.
		/// </summary>
		public bool notifyPlayerOfChange { get; internal set; } = true;

		public override string value => current.ToString();

		internal CreatureStatNumeric(SimpleDescriptor statName, CreatureStatCategory statCategory) : base(statName, statCategory) { }

	}
}
