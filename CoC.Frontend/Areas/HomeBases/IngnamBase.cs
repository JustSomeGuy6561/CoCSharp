using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.HomeBases
{
	public sealed partial class IngnamBase : HomeBaseBase
	{
		public IngnamBase() : base(IngnamBaseName)
		{
		}

		public override bool isUnlocked { get => true; protected set => ; }
		public override int timesExplored { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		protected override SimpleDescriptor UnlockText => GlobalStrings.None;

		protected override void OnReload()
		{
			throw new NotImplementedException();
		}

		//guarenteed to derive ICampNPC.
		protected override SimpleDescriptor OverrideDefaultIdleTextForCampNPC(Creature creature)
		{
			return null;
#warning ToDo: Consider adding custom descriptors for 
		}
	}
}
