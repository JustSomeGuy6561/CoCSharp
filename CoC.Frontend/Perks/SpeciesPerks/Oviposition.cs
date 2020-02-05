using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Perks;
using CoC.Frontend.Creatures.PlayerData;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	//note: oviposition will now work with non-NPCs who have a womb that allows oviposition.

	internal sealed class Oviposition : ConditionalPerk
	{
		protected override void SetupActivationConditions()
		{
			sourceCreature.womb.dataChange -= Womb_dataChange;
			sourceCreature.womb.dataChange += Womb_dataChange;

			enabled = sourceCreature.womb.hasOviposition;
		}

		private void Womb_dataChange(object sender, SimpleDataChangeEvent<Womb, WombData> e)
		{
			if (e.newValues.hasOviposition)
			{
				enabled = e.newValues.hasOviposition;
			}
			else
			{
				enabled = false;
			}
		}

		protected override void RemoveActivationConditions()
		{
			sourceCreature.womb.dataChange -= Womb_dataChange;
		}

		public override string Name()
		{
			return "Oviposition";
		}

		public override string HasPerkText()
		{
			if (sourceCreature.gender.HasFlag(Gender.FEMALE))
			{
				return "Your body will now periodically produce unfertilized eggs. You will eventually lay these eggs, but sex with certain species may " +
					"fertilize them, causing a full pregnancy instead.";
			}
			else
			{
				return "Though currently dormant, your reproductive systems would periodically produce (and subsequently lay) eggs if you have a vagina.";
			}
		}
	}
}
