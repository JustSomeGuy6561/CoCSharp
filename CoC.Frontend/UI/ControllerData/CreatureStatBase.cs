using CoC.Backend;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoC.Frontend.UI.ControllerData
{
	public enum CreatureStatCategory { CORE, COMBAT, ADVANCEMENT, PRISON, GENERAL, OTHER }

	public abstract class CreatureStatBase
	{
		public readonly SimpleDescriptor statName;

		public readonly CreatureStatCategory category;

		public abstract string value { get; }

		public bool enabled { get; internal set; } = true;

		protected CreatureStatBase(SimpleDescriptor nameCallback, CreatureStatCategory statCategory)
		{
			statName = nameCallback ?? throw new ArgumentNullException(nameof(nameCallback));
			category = statCategory;
		}

	}
}