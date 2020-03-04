using System;
using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks.History;

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

			numTransformsDelta = new SignedBytePerkModifier(0, -2, 2);

			itemForgeCostReduction = new BytePerkModifier(0, 0, 2);


			replaceMasturbateWithMeditate = new ConditionalPerkModifier(false);

			lustRequiredForSexOffset = new SignedBytePerkModifier(0, -50, 50);
			corruptionRequiredOffset = new SignedBytePerkModifier(0, -50, 50);
			purityRequiredOffset = new SignedBytePerkModifier(0, -50, 50);

			teaseStrengthMultiplier = new DoublePerkModifier(1, 0.5, 3);

			isASlut = new ConditionalPerkModifier(false);

			resistsTFBadEnds = new ConditionalPerkModifier(false);

		}

		public SignedBytePerkModifier numTransformsDelta;

		public BytePerkModifier itemForgeCostReduction;



		public ConditionalPerkModifier replaceMasturbateWithMeditate;

		public SignedBytePerkModifier lustRequiredForSexOffset; //isHornyEnough(value) will be offset by this value
		public SignedBytePerkModifier corruptionRequiredOffset; //IsCorruptEnough(value) will be offset by this value
		public SignedBytePerkModifier purityRequiredOffset; //isPureEnough(value) will be offset by this value

		public DoublePerkModifier teaseStrengthMultiplier;

		public ConditionalPerkModifier isASlut;

		public ConditionalPerkModifier resistsTFBadEnds;

		private void Womb_onBirth(object sender, Backend.Pregnancies.BirthEvent e)
		{
			if (e.totalBirthCount >= 10 && !source.perks.HasPerk<BroodMotherPerk>())
			{
				source.perks.AddPerk<BroodMotherPerk>();
			}
		}
	}
}
