using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks.History;
using System;

namespace CoC.Frontend.Perks
{
	//any common variables for perks can go here. 
	public sealed class ExtendedPerkModifiers
	{
		private readonly Creature source;
		private CombatCreature combatSource => source as CombatCreature;
		private PlayerBase playerSource => source as PlayerBase;

		public ExtendedPerkModifiers(Creature parent)
		{
			source = parent ?? throw new ArgumentNullException(nameof(parent));
			parent.womb.onBirth += Womb_onBirth;
		}

		public sbyte numTransformsDelta;

		public byte itemForgeCostReduction;

		public float gemGainMultiplier = 1.0f;

		public float healingMultiplier = 1.0f;

		public bool replaceMasturbateWithMeditate = false;

		public sbyte lustRequiredForSexOffset = 0; //isHornyEnough(value) will be offset by this value
		public sbyte corruptionRequiredOffset = 0; //isCorruptEnough(value) will be offset by this value

		public float teaseStrengthMultiplier = 1.0f;

		public bool IsASlut = false;

		private void Womb_onBirth(object sender, Backend.Pregnancies.BirthEvent e)
		{
			if (e.totalBirthCount >= 10 && !source.perks.HasPerk<BroodMotherPerk>())
			{
				source.perks.AddPerk<BroodMotherPerk>();
			}
		}
	}

	public static class ExtraPerkHelper
	{
		//it's not uncommon (or unexpected, i suppose) to want to see if the player has slut perks.
		//right now these are the main two in that, but i suppose others could be added. 
		public static bool HasASlutPerk(this PerkCollection perkCollection)
		{
			return perkCollection.HasPerk<Slut>() || perkCollection.HasPerk<Whore>(); 
		}
	}
}
