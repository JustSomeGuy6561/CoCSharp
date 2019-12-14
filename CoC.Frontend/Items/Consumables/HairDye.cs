//HairDye.cs
//Description:
//Author: JustSomeGuy
//12/13/2019 1:05:59 AM

using CoC.Backend;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class HairDye : ConsumableBase
	{
		private readonly HairFurColors color;

		public HairDye(HairFurColors dyeColor) : base(Short(dyeColor), Full(dyeColor), Desc)
		{
			color = dyeColor ?? throw new ArgumentNullException(nameof(dyeColor));
		}

		private static SimpleDescriptor Short(HairFurColors dyeColor)
		{
			return () => dyeColor.AsString() + " dye";
		}

		private static SimpleDescriptor Full(HairFurColors dyeColor)
		{
			return () => $"a vial of {dyeColor.AsString()} hair dye";
		}

		private static string Desc()
		{
			return "This bottle of dye will allow you to change the color of your hair. Of course if you don't have hair, using this would be a waste.";
		}

#warning Current (lazy) implementation does not display any flavor text. need to add that. also need to store menu string globally so it can be applied when going back to the menu
		//from a sub-menu.
		//warn the player in this text when they dye and the current color are the same.


		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => false; //i mean, it is, but it's not, i guess? vanilla said no.
													  //does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 0;

		protected override int monetaryValue => color == HairFurColors.RAINBOW ? 100 : 6;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}


		private UseItemCallback itemCallback;
		private ButtonListMaker primaryMenu;
		private Creature source;

		public override void AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			IEnumerable<object> listOfDyeableParts = target.bodyParts.Where(x => x is IDyeable || x is IMultiDyeable);

			DisplayManager.GetCurrentDisplay().ClearOutput();

			//set the globals.
			primaryMenu = new ButtonListMaker(DisplayManager.GetCurrentDisplay());
			itemCallback = postItemUseCallback;
			source = target;


			foreach (var item in listOfDyeableParts)
			{
				if (item is IMultiDyeable multiDyeable)
				{
					bool active = Enumerable.Range(0, multiDyeable.numDyeableMembers).Select(x => multiDyeable.isDifferentColor(color, (byte)x)).Any(x => x);
					primaryMenu.AddButtonToList(multiDyeable.buttonText() + "...", active, () => DoSubMenu(multiDyeable));
				}
				else if (item is IDyeable dyeable)
				{
					primaryMenu.AddButtonToList(dyeable.buttonText(), dyeable.allowsDye(), () => ApplyDye(dyeable));
				}
			}

			RunMainMenu();
		}

		private void RunMainMenu()
		{
			primaryMenu.CreateButtons(GlobalStrings.CANCEL(), true, PutBack);
		}

			//if (result)
			//{
			//	if (target is PlayerBase player)
			//	{
			//		player.refillHunger(sateHungerAmount);
			//	}
			//	item = null;
			//}

			//if (isBadEnd)
			//{
			//	throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
			//	//if we hit a bad end, don't resume whatever we were doing - we treat it as if it was a nightmare and nothing happened, except for the stuff that did happen, because continuity
			//	//is hard to enforce in this shit. Just lampshade it - it was a nightmare, but you still suffer the effects of it as if it happened, but only up until the point you realized it
			//	//was a bad end. so, you'll lose any items you were gonna get afterward, etc, but if your butt was stretched to gaping, it'll still be gaping after resuming from the bad end.
			//	//same with piercings, tfs, etc. No time is lost, however, just resume from camp as soon as possible.
			//	//example of lampshading: "it was just a nightmare... but it felt so real - and your eyebrow has a piercing in it, just like in the dream. strange..."

			//	//GameEngine.DoBadEnd();
			//}
			//else
			//{
			//	postItemUseCallback(result, consumeResults, item);
			//}
		//}

		private void DoSubMenu(IMultiDyeable multiDyeable)
		{
			DisplayManager.GetCurrentDisplay().ClearOutput();

			ButtonListMaker subMenu = new ButtonListMaker(DisplayManager.GetCurrentDisplay());

			for (byte x = 0; x < multiDyeable.numDyeableMembers; x++)
			{
				subMenu.AddButtonToList(multiDyeable.memberButtonText(x), multiDyeable.allowsDye(x), () => ApplyDye(multiDyeable, x));
			}

			subMenu.CreateButtons(GlobalStrings.BACK(), true, RunMainMenu);
		}

		private void ApplyDye(IMultiDyeable multiDyeable, byte index)
		{
			string results;
			bool success = multiDyeable.attemptToDye(color, index);
			if (multiDyeable is IMultiDyeableCustomText customText)
			{
				results = customText.DisplayResults(color, index, success);
			}
			else
			{
				results = ResultsOfDyeText(multiDyeable, index, success);
			}

			finalize(results);
		}

		private void ApplyDye(IDyeable dyeable)
		{
			string results;
			bool success = dyeable.attemptToDye(color);
			if (dyeable is IDyeableCustomText customText)
			{
				results = customText.DisplayResults(color, success);
			}
			else
			{
				results = ResultsOfDyeText(dyeable, success);
			}

			finalize(results);
		}

		private void finalize(string result)
		{
			if (source.relativeLust > 50)
			{
				result += LessLustyText();
				source.DecreaseLust(15);
			}

			//clear the globals.
			source = null;
			primaryMenu = null;

			var temp = itemCallback;
			itemCallback = null;

			//and resume normal execution.
			temp(true, result, null);
		}

		private void PutBack()
		{
			primaryMenu = null;

			itemCallback(false, PutBackItemText(), this);
		}

		private string PutBackItemText()
		{
			throw new NotImplementedException();
		}

		private string ResultsOfDyeText(IDyeable dyeable, bool succeeded)
		{
			return ResultsOfDyeText(dyeable.locationDesc(out bool _), dyeable.postDyeDescription(), succeeded);
		}

		private string ResultsOfDyeText(IMultiDyeable dyeable, byte index, bool succeeded)
		{
			return ResultsOfDyeText(dyeable.memberLocationDesc(index, out bool _), dyeable.memberPostDyeDescription(index), succeeded);
		}

		private string ResultsOfDyeText(string location, string locationPostDye, bool success)
		{
			string intro = "You rub the dye into your " + location + ", then use a bucket of cool lakewater to rinse clean a few minutes later. ";
			if (success)
			{
				return intro + "You now have " + locationPostDye;
			}
			else
			{
				return intro + "... Nothing happened. Well, that was a waste!";
			}
		}

		private string LessLustyText()
		{
			return GlobalStrings.NewParagraph() + "The cool water calms your urges somewhat, letting you think more clearly.";
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			throw new NotSupportedException();
		}
	}
}
