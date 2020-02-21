using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Perks;
using CoC.Frontend.Items.Wearables.Armor;

namespace CoC.Frontend.Perks.ArmorPerks
{
	class LustyMaidenPerk : ConditionalPerk
	{
		protected override void SetupActivationConditions()
		{
			throw new NotImplementedException();
		}

		protected override void RemoveActivationConditions()
		{
			throw new NotImplementedException();
		}

		private void ArmorChanged(object sender, object e)
		{
			//
			if (sourceCreature.armor is LustyMaidensArmor)
			{
				currentlyEnabled = true;
				//this.baseModifiers.minLibido = 50;
				//this.baseModifiers.minLust = 30;
			}
			else
			{
				currentlyEnabled = false;
			}
		}

		public override string Name()
		{
			throw new NotImplementedException();
		}

		public override string HasPerkText()
		{
			throw new NotImplementedException();
		}

	}
}
