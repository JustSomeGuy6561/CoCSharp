using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoC.Frontend.UI.ControllerData
{
	public enum CreatureStatCategory { CORE, COMBAT, ADVANCEMENT, PRISON, GENERAL, OTHER }

	public abstract class CreatureStatBase
	{
		public readonly CreatureStatCategory category;

		public abstract string value { get; }

		public bool enabled { get; internal set; } = true;

		protected CreatureStatBase(CreatureStatCategory statCategory)
		{
			category = statCategory;
		}

	}
}