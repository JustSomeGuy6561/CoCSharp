using CoC.Backend;
using CoC.Backend.Areas;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.Dungeons
{
	internal partial class SandWitchDungeon : DungeonBase
	{
		public SandWitchDungeon(SimpleDescriptor areaName) : base(areaName)
		{
		}

		public override bool isUnlocked { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public override bool isCompleted { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		public override bool disableOnCompletion => throw new NotImplementedException();

		public override bool isHidden => throw new NotImplementedException();

		public override int timesVisited { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		protected override SimpleDescriptor UnlockText => throw new NotImplementedException();
	}
}
