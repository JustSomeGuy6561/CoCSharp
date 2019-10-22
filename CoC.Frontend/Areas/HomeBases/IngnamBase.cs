using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.HomeBases
{
	//Base camp for grimdark. Not even remotely implemented.

	internal sealed partial class IngnamBase : HomeBaseBase
	{
		public IngnamBase() : base(IngnamBaseName)
		{
		}

		protected override void LoadUniqueCampActionsMenu(DisplayBase currentDisplay)
		{
			currentDisplay.OutputText("Not implemented :(");
			AddReturnButtonToDisplay();
		}

		protected override string CampDescription(bool isReload)
		{
			throw new NotImplementedException();
		}
	}
}
