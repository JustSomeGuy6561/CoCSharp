using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.HomeBases
{
	internal sealed partial class Camp : HomeBaseBase
	{
		public Camp() : base(CampName)
		{
		}

		protected override void OnReload()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor OverrideDefaultIdleTextForCampNPC(Creature creature, byte currentHour)
		{
			throw new NotImplementedException();
		}
	}
}
