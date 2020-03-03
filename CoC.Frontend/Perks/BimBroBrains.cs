using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks
{
	class BimBroBrains : ConditionalPerk
	{
		protected override void SetupActivationConditions()
		{
			sourceCreature.perks.perksChanged -= PerksChanged;
			sourceCreature.perks.perksChanged += PerksChanged;
		}

		private void PerksChanged(object sender, EventArgs e)
		{
			currentlyEnabled = sourceCreature.HasPerk<BimBro>() && sourceCreature.perks.GetPerkData<BimBro>().bimbroBrains;
		}

		protected override void RemoveActivationConditions()
		{
			sourceCreature.perks.perksChanged -= PerksChanged;
		}

		public override string Name()
		{
			var bimbro = sourceCreature.GetPerkData<BimBro>();
			if (bimbro is null || bimbro.bimboBody)
			{
				return "Bimbo Brains";
			}
			else if (bimbro.broBody)
			{
				return "Bro Brains";
			}
			else
			{
				return "Futa Faculties";
			}
		}

		public override string HasPerkText()
		{
			var bimbro = sourceCreature.GetPerkData<BimBro>();
			if (bimbro is null || !bimbro.bimbroBrains)
			{
				return "(inactive)";
			}
			else if (bimbro.bimboBody)
			{
				return "Now that you've drank bimbo liquer, you'll never, like, have the attention span and intelligence you once did! But it's okay, 'cause you get to be so horny an' stuff!";
			}
			else if (bimbro.broBody)
			{
				return "Makes thou... thin... fuck, that shit's for nerds.";
			}
			else
			{
				return "It's super hard to think about stuff that like, isn't working out or fucking!";
			}
		}
	}
}
