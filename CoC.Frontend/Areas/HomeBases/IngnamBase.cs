using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.HomeBases
{
	internal sealed partial class IngnamBase : HomeBaseBase
	{
		public IngnamBase() : base(IngnamBaseName)
		{
		}

		protected override void OnReload()
		{
			throw new NotImplementedException();
		}

		//guarenteed to derive ICampNPC.
		protected override SimpleDescriptor OverrideDefaultIdleTextForCampNPC(Creature creature, byte currentHour)
		{
			return null;
#warning ToDo: Consider adding custom descriptors for 
		}
	}
}
