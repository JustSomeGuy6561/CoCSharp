using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Fetishes
{
	public sealed partial class PiercingFetish : SimpleFetish
	{
		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public PiercingFetish() : base(PiercingFetishName)
		{}

		public override bool enabled
		{
			get => session.piercingFetish;
			protected set => session.piercingFetish = value;
		}
		public override bool? enabledGlobal
		{
			get => glob.PiercingFetishGlobal;
			protected set => glob.PiercingFetishGlobal = value;
		}

		public override SimpleDescriptor enabledHint => EnabledHintFn;

		public override SimpleDescriptor disabledHint => DisabledHintFn;

	}
}
