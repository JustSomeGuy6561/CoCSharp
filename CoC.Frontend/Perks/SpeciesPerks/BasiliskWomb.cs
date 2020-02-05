using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Perks;
using CoC.Frontend.Creatures.PlayerData;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	internal sealed class BasiliskWomb : ConditionalPerk
	{
		protected override void SetupActivationConditions()
		{
			sourceCreature.womb.dataChange -= Womb_dataChange;
			sourceCreature.womb.dataChange += Womb_dataChange;

			enabled = sourceCreature.womb is PlayerWomb playerWomb && playerWomb.basiliskWomb;
		}

		private void Womb_dataChange(object sender, SimpleDataChangeEvent<Womb, WombData> e)
		{
			if (e.newValues is PlayerWombData playerWombData)
			{
				enabled = playerWombData.basiliskWomb;
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
			return "Basilisk Womb";
		}

		public override string HasPerkText()
		{
			if (sourceCreature.gender.HasFlag(Gender.FEMALE))
			{
				return "Your womb is now permanently attuned to basilisk physiology, allowing you to carry basilisk children. As a result, your body will periodically produce " +
				"unfertilized eggs, regardless of your current species.";

			}
			else
			{
				return "Your reproductive systems are now permanently attuned to basilisk physiology. Though currently dormant, your body will always produce unfertilized eggs " +
					"should you ever obtain a vagina, regardless of your current species.";
			}
		}
	}
}
