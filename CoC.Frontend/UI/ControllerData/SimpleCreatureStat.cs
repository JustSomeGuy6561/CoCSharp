using CoC.Backend;
using System;

namespace CoC.Frontend.UI.ControllerData
{
	public class SimpleCreatureStat : CreatureStatBase
	{
		public SimpleCreatureStat(SimpleDescriptor statName, string immutableValue, CreatureStatCategory statCategory) : base(statName, statCategory)
		{
			simpleValue = immutableValue ?? throw new ArgumentNullException(nameof(immutableValue));
		}

		public override string value => simpleValue;
		internal string simpleValue { get; set; }
	}
}
