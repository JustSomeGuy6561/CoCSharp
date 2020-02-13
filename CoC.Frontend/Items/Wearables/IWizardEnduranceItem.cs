using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Frontend.Items.Wearables
{
	interface IWizardEnduranceItem
	{
		byte WizardsEnduranceModifier(Creature wearer);
	}
}
