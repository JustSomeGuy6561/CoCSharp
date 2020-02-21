using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	//kangaroo perk. unlike most species perks, it is not lost when the creature loses their kangaroo-like traits; it is gender-locked to females - when the creature no longer
	//has a vagina, they lose the perk.
	class Diapause : ConditionalPerk
	{
		public Diapause() : base()
		{
		}

		protected override void SetupActivationConditions()
		{
			sourceCreature.womb.dataChange -= Womb_dataChange;
			sourceCreature.womb.dataChange += Womb_dataChange;

			currentlyEnabled = sourceCreature.womb.hasDiapause;
		}

		private void Womb_dataChange(object sender, SimpleDataChangeEvent<Womb, WombData> e)
		{
			if (e.newValues.hasDiapause)
			{
				currentlyEnabled = true;
			}
			else
			{
				currentlyEnabled = false;
			}
		}

		protected override void RemoveActivationConditions()
		{
			currentlyEnabled = false;
			sourceCreature.womb.dataChange -= Womb_dataChange;
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
