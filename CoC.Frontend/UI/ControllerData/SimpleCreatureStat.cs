using System;

namespace CoC.Frontend.UI.ControllerData
{
	public class SimpleCreatureStat : CreatureStatBase
	{
		public SimpleCreatureStat(string immutableValue, CreatureStatCategory statCategory) : base(statCategory)
		{
			simpleValue = immutableValue ?? throw new ArgumentNullException(nameof(immutableValue));
		}

		public override string value => simpleValue;
		internal string simpleValue { get; set; }
	}
}
