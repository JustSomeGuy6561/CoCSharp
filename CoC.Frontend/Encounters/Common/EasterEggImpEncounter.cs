//EasterEggImpEncounter.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 3:36 AM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.Inventory;
using CoC.Frontend.Items.Consumables;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	partial class EasterEggImpEncounter : RandomEncounter
	{
		private StandardDisplay currentDisplay => DisplayManager.GetCurrentDisplay();

		protected override int chances => Utils.LerpRound(1, 20, (int)player.level, 2, 0);

		protected override bool EncounterDisabled()
		{
			return player.level >= 15;
		}

		protected override bool EncounterUnlocked()
		{
			return true;
		}

		protected override void RunEncounter()
		{
			currentDisplay.ClearOutput();
			currentDisplay.OutputText("A small imp bursts from behind a rock and buzzes towards you. You prepare for a fight, but it stays high and simply flies above you. " +
				"Suddenly another imp appears from nowhere and attacks the first. In the tussle one of them drops an item, which you handily catch, as the scrapping demons " +
				"fight their way out of sight. ");
			//unlockCodexImps();

#warning add remaining choices here.
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
			//imp food, incubus draft, succubus milk.
			//Func<ConsumableBase>[] choices = new Func<ConsumableBase>[] { () => new ImpFood() };

			//ConsumableBase item = Utils.RandomChoice(choices).Invoke();
			//GainItemHelper.GainItemWithCallback(player, item, context, () => currentDisplay.DoNext(() => GameEngine.UseHoursGoToBase(1)));
		}
	}
}
