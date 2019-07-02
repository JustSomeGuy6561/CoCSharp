//CorruptedGrowthEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:46 PM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using CoC.Frontend.SaveData;
using System;

namespace CoC.Frontend.Encounters.Forest
{
	//Corrupted growth is a combination of tentacle beast and corrupted glades. 
	//tentacle beast is getting nuked b/c that license is cancer. 

	//20% of the time, it will be the satyr scene. You may now free the satyr, in addition to taking advantage of him. 
	//	if the satyr is particularly into tentacle rape (random chance), he may be mad and attack you. if he does, he will have high initial lust and some health loss.
	//	otherwise the vines will attack you for stealing their plaything. you may get a reward from satyr.

	//The remaining 80% is split between attacking you and normal interaction, according to your corruption:
	//Of course, with high enough intellect and the plant book, you have the option to simply leave and ignore all of these. 
	//0-33: 40% attack, 40% normal interract (destroy, leave, or explore further)
	//34-66: 30% attack, 25% normal, 25% sexy time
	//68+ : 20% attack, 60% sexy time

	//if you decide to destroy the glade, it will attack you 33% of the time, though it will take damage before combat based on what you've done. 
	//if you decide to explore further, you will always hit the corrupted dryad. 
	//if you go for sex, it will have varying effects based on gender and stats, as was the case before/

	//dryads also have a small chance of procing on their own, completely independant of the glades.
	
	internal sealed class CorruptedGrowthEncounter : RandomEncounter
	{
		private static FrontendSessionSave data = FrontendSessionSave.data;
		public static byte amountDestroyed
		{
			get => data.corruptedGladesDestroyed;
			private set => data.corruptedGladesDestroyed = value;
		}

		public CorruptedGrowthEncounter() : base() { }

		protected override int chances => amountDestroyed >= 100 ? 0 : (int)Math.Floor(20 - 9 * amountDestroyed / 50.0 );

		protected override void Run()
		{
			int rando = Utils.Rand(20);
			if (rando < 4)
			{
				runSatyrEncounter();
			}
			else
			{
				int combatTrigger;
				int normalTrigger = 20;
				if (player.corruption < 33)
				{
					combatTrigger = 2 / 5 * 20 + 4;
				}
				else if (player.corruption < 67)
				{
					combatTrigger = 3 / 10 * 20 + 4;
					normalTrigger = 1 / 4 * 20 + combatTrigger;
				}
				else
				{
					combatTrigger = 1 / 5 * 20 + 4;
					normalTrigger = 0;
				}
				if (rando < combatTrigger)
				{
					runCombatEncounter();
				}
				else if (rando < normalTrigger)
				{
					runRegularEncounter();
				}
				else
				{
					runSexEncounter();
				}
			}
		}

		private void runSatyrEncounter()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private void runCombatEncounter(float baseHealth = 1.0f)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private void runSexEncounter()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private void runRegularEncounter()
		{
			//if (destroy && Utils.Rand(3) == 0) runCombatEncounter(percentHealthBasedOnAttack);
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override bool encounterDisabled()
		{
			return amountDestroyed >= 100;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 2;
		}
	}
}
