//HairDye.cs
//Description:
//Author: JustSomeGuy
//12/13/2019 1:05:59 AM

using System;
using System.Collections.Generic;
using System.Linq;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class HairDye : ConsumableWithMenuBase
	{
		private readonly HairFurColors color;

		public HairDye(HairFurColors dyeColor) : base()
		{
			color = dyeColor ?? throw new ArgumentNullException(nameof(dyeColor));
		}

		public override string AbbreviatedName()
		{
			return color.AsString().Truncate(6) + "Dye";
		}

		public override string ItemName()
		{
			return color.AsString() + " Dye";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string vialText = count != 1 ? "vials" : "vial";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of {color.AsString()} hair dye";
		}

		public override string AboutItem()
		{
			return "This bottle of dye will allow you to change the color of your hair. Of course if you don't have hair, using this would be a waste.";
		}

#warning Current (lazy) implementation does not display any flavor text. need to add that. also need to store menu string globally so it can be applied when going back to the menu
		//from a sub-menu.
		//warn the target in this text when they dye and the current color are the same.


		public override bool countsAsLiquid => false; //i mean, it is, but it's not, i guess? vanilla said no.
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 0;

		protected override int monetaryValue => color == HairFurColors.RAINBOW ? 100 : 6;

		public override bool Equals(CapacityItem other)
		{
			return other is HairDye hairDye && this.color == hairDye.color;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}


		private UseItemCallback itemCallback;
		private ButtonListMaker buttonMaker;
		private Creature source;
		private StandardDisplay display;
		IEnumerable<IBodyPart> listOfDyeableParts;


		protected override DisplayBase BuildMenu(Creature target, UseItemCallback postItemUseCallback)
		{
			listOfDyeableParts = target.bodyParts.Where(x => x is IDyeable || x is IMultiDyeable);

			display = new StandardDisplay();

			//set the globals.
			buttonMaker = new ButtonListMaker(display);
			itemCallback = postItemUseCallback;
			source = target;

			RunMainMenu();
			return display;
		}

		private void RunMainMenu()
		{
			display.ClearOutput();

			display.OutputText("Where do you want to apply the " + color.AsString() + " hair dye?");

			foreach (IBodyPart item in listOfDyeableParts)
			{
				if (item is IMultiDyeable multiDyeable)
				{
					bool active = Enumerable.Range(0, multiDyeable.numDyeableMembers).Select(x => multiDyeable.isDifferentColor(color, (byte)x)).Any(x => x);
					string tip = active ? "Dye part of your " + item.BodyPartName() + ". This will bring up more options." : "Your " + item.BodyPartName() + "cannot be dyed or is already the same color";
					buttonMaker.AddButtonToList(multiDyeable.buttonText() + "...", active, () => DoSubMenu(multiDyeable));
				}
				else if (item is IDyeable dyeable)
				{
					string tip;
					string location = dyeable.locationDesc(out bool plural);

					if (!dyeable.allowsDye())
					{
						tip = location.CapitalizeFirstLetter() + " cannot currently be dyed.";
					}
					else if (!dyeable.isDifferentColor(color))
					{
						tip = location.CapitalizeFirstLetter() + (plural ? " are" : " is") + " already " + color.AsString() + ".";
					}
					else
					{
						tip = null;
					}

					buttonMaker.AddButtonToList(dyeable.buttonText(), dyeable.allowsDye() && dyeable.isDifferentColor(color), () => ApplyDye(dyeable), tip, null);
				}
			}

			buttonMaker.CreateButtons(GlobalStrings.CANCEL(), true, PutBack);
		}

		//if (result)
		//{
		//	if (target is PlayerBase target)
		//	{
		//		target.refillHunger(sateHungerAmount);
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

#warning Implement IPatternable bs when you figure out how to do so and/or actually feel like it.

		private void DoSubMenu(IMultiDyeable multiDyeable)
		{

			display.OutputText(multiDyeable.LocationDesc() + " has several possible locations or combinations of locations. Where on " + multiDyeable.LocationDesc() +
				" would you like to apply the " + color.AsString() + " hair dye?" + Environment.NewLine + "You can also go back to the full menu by hitting back.");

			display.ClearOutput();


			buttonMaker.ClearList();
			for (byte x = 0; x < multiDyeable.numDyeableMembers; x++)
			{
				string tip;
				string location = multiDyeable.memberLocationDesc(x, out bool plural);
				if (!multiDyeable.allowsDye(x))
				{
					tip = location.CapitalizeFirstLetter() + " cannot currently be dyed.";
				}
				else if (!multiDyeable.isDifferentColor(color, x))
				{
					tip = location.CapitalizeFirstLetter() + (plural ? " are" : " is") + " already " + color.AsString() + ".";
				}
				else
				{
					tip = null;
				}
				buttonMaker.AddButtonToList(multiDyeable.memberButtonText(x), multiDyeable.allowsDye(x), () => ApplyDye(multiDyeable, x), tip, null);
			}

			buttonMaker.CreateButtons(new ButtonData(GlobalStrings.BACK(), true, RunMainMenu), new ButtonData(GlobalStrings.CANCEL(), true, PutBack));
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

			CleanupAndReturn(results);
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

			CleanupAndReturn(results);
		}

		private void CleanupAndReturn(string result)
		{
			if (source.relativeLust > 50)
			{
				result += LessLustyText();
				source.DecreaseLust(15);
			}

			//clear the globals.
			source = null;
			buttonMaker = null;
			display = null;

			UseItemCallback temp = itemCallback;
			itemCallback = null;

			//and resume normal execution.
			temp(true, result, Author(), null);
		}

		private void PutBack()
		{
			buttonMaker = null;
			display = null;
			source = null;

			itemCallback(false, PutBackItemText(), Author(), this);
			itemCallback = null;
		}

		private string PutBackItemText()
		{
			return "You put the dye away." + GlobalStrings.NewParagraph();
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
	}
}
