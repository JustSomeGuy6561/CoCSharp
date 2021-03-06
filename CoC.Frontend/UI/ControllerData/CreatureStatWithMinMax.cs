﻿using CoC.Backend;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed class CreatureStatWithMinMax : CreatureStatNumeric
	{
		public uint maximum { get; internal set; } = 100;

		public uint? minimum { get; internal set; } = null;

		public bool isRatio { get; internal set; } = false;

		internal CreatureStatWithMinMax(SimpleDescriptor statName, CreatureStatCategory statCategory) : base(statName, statCategory)
		{

		}
	}
}
