using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * Body lotions, courtesy of Foxxling.
	 * @author Kitteh6660
	 */
	public abstract class BodyLotionBase : ConsumableWithMenuBase, IEquatable<BodyLotionBase>

	{
		private readonly SkinTexture targetTexture;

		public BodyLotionBase(SkinTexture texture)
		{
			targetTexture = texture;
		}

		public override string AbbreviatedName()
		{
			return targetTexture.AsAbbreviatedString() + "Ltn";
		}

		public override string ItemName()
		{
			return targetTexture.AsString() + " Lotion";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string vialText = count == 1 ? "flask" : "flasks";

			return $"{countText}{vialText} of " + targetTexture.AsString() + " lotion";
		}

		public override string AboutItem()
		{
			return "A small wooden flask filled with a " + LiquidDesc(false) + " . A label across the front says, \"" + targetTexture.AsString() + " Lotion.\"";
		}

		protected override int monetaryValue => DEFAULT_VALUE;


		protected abstract string LiquidDesc(bool shortForm = true);

		public abstract bool Equals(BodyLotionBase other);

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 0;

		private void LotionCancel(Creature consumer, UseItemCallback postItemUseCallback)
		{
			postItemUseCallback(false, "You decided not to use the body lotion", null, this);
			CleanupItems();
		}

		private List<IBodyPart> validMembers = null;
		private StandardDisplay display = null;
		private ButtonListMaker listMaker = null;
		protected override DisplayBase BuildMenu(Creature consumer, UseItemCallback postItemUseCallback)
		{
			validMembers = consumer.bodyParts.Where(x => x is ILotionable || x is IMultiLotionable).ToList();
			display = new StandardDisplay();
			listMaker = new ButtonListMaker(display);

			if (validMembers.Count == 0)
			{
				LotionCancel(consumer, postItemUseCallback);
				return null;
			}
			if (validMembers.Count == 1 && validMembers[0] is ILotionable lotionable)
			{
				DoLotion(consumer, lotionable, postItemUseCallback);
			}

			return StandardMenu(consumer, postItemUseCallback);
		}

		private StandardDisplay StandardMenu(Creature consumer, UseItemCallback postItemUseCallback)
		{
			if (validMembers.Count == 1)
			{
				return DoMultiLotionMenu(consumer, (IMultiLotionable)validMembers[0], postItemUseCallback);
			}

			display.ClearOutput();
			listMaker.ClearList();

			display.OutputText("Where do you want to apply the " + targetTexture.AsString() + " body lotion?");

			foreach (IBodyPart part in consumer.bodyParts)
			{
				if (part is ILotionable lotionable)
				{
					listMaker.AddButtonToList(part.BodyPartName(), lotionable.CanLotion(), () => DoLotion(consumer, lotionable, postItemUseCallback));
				}
				else if (part is IMultiLotionable multiLotionable)
				{
					listMaker.AddButtonToList(multiLotionable.ButtonText() + "...", multiLotionable.numLotionableMembers > 0, () => DoMultiLotionMenu(consumer, multiLotionable, postItemUseCallback));

				}
			}
			listMaker.CreateButtons(GlobalStrings.CANCEL(), true, () => LotionCancel(consumer, postItemUseCallback));

			return display;
		}

		private StandardDisplay DoMultiLotionMenu(Creature consumer, IMultiLotionable multiLotionable, UseItemCallback postItemUseCallback)
		{
			bool canReturnToMenu = display != null;

			display.ClearOutput();
			listMaker.ClearList();

			display.OutputText(multiLotionable.LocationDesc() + " has several possible locations or combinations of locations. Where on " + multiLotionable.LocationDesc() +
				" would you like to apply the " + targetTexture.AsString() + " body lotion?" + Environment.NewLine + "You can also go back to the main menu by hitting back.");

			for (byte x = 0; x < multiLotionable.numLotionableMembers; x++)
			{
				listMaker.AddButtonToList(multiLotionable.MemberButtonText(x), multiLotionable.CanLotion(x), () => DoLotion(consumer, multiLotionable, x, postItemUseCallback));
			}


			listMaker.CreateButtons(new ButtonData(GlobalStrings.BACK(), true, () => StandardMenu(consumer, postItemUseCallback)),
				new ButtonData(GlobalStrings.CANCEL(), true, () => LotionCancel(consumer, postItemUseCallback)));

			return display;
		}

		private void DoLotion(Creature consumer, IMultiLotionable multiLotionable, byte index, UseItemCallback postItemUseCallback)
		{
			StringBuilder sb = new StringBuilder();

			if (multiLotionable is IMultiLotionableCustomText customText)
			{
				bool succeeded = multiLotionable.AttemptToLotion(targetTexture, index);
				sb.Append(customText.DisplayResults(targetTexture, index, succeeded));
			}
			else if (!multiLotionable.IsDifferentTexture(targetTexture, index))
			{
				sb.Append("You " + (consumer.wearingAnything ? "take a second to disrobe before uncorking the flask of lotion and rubbing"
				: "uncork the flask of lotion and rub") + " the " + LiquidDesc() + " across your " + multiLotionable.MemberLocationDesc(index, out bool isPlural)
				+ ". Once you've finished you feel reinvigorated. ");
			}
			else
			{
				string location = multiLotionable.MemberLocationDesc(index, out bool isPlural);
				sb.Append((consumer.wearingAnything ? "Once you've disrobed you take the lotion and" : "You take the lotion and") + " begin massaging it into your "
					+ location + ". As you do so," + location +
					(isPlural ? " begin" : " begins") + " to tingle pleasantly.");
				if (multiLotionable.AttemptToLotion(targetTexture, index))
				{
					sb.Append(SuccessfullyAppliedLotion(location, isPlural));
				}
				else
				{
					sb.Append(LotionDidNothing(location, isPlural));
				}
			}

			postItemUseCallback(true, sb.ToString(), null, null);
			CleanupItems();
		}
		private void DoLotion(Creature consumer, ILotionable lotionable, UseItemCallback postItemUseCallback)
		{
			StringBuilder sb = new StringBuilder();

			if (lotionable is ILotionableCustomText customText)
			{
				bool succeeded = lotionable.AttemptToLotion(targetTexture);
				sb.Append(customText.DisplayResults(targetTexture, succeeded));
			}
			else if (!lotionable.IsDifferentTexture(targetTexture))
			{
				sb.Append("You " + (consumer.wearingAnything ? "take a second to disrobe before uncorking the flask of lotion and rubbing"
				: "uncork the flask of lotion and rub") + " the " + LiquidDesc() + " across your " + lotionable.LocationDesc(out bool isPlural)
				+ ". Once you've finished you feel reinvigorated. ");
			}
			else
			{
				string location = lotionable.LocationDesc(out bool isPlural);
				sb.Append((consumer.wearingAnything ? "Once you've disrobed you take the lotion and" : "You take the lotion and") + " begin massaging it into your "
					+ location + ". As you do so," + location +
					(isPlural ? " begin" : " begins") + " to tingle pleasantly.");
				if (lotionable.AttemptToLotion(targetTexture))
				{
					sb.Append(SuccessfullyAppliedLotion(location, isPlural));
				}
				else
				{
					sb.Append(LotionDidNothing(location, isPlural));
				}
			}

			postItemUseCallback(true, sb.ToString(), null, null);
			CleanupItems();
		}

		private void CleanupItems()
		{
			display = null;
			listMaker = null;
			validMembers = null;
		}

		protected abstract bool SuccessfullyAppliedLotion(string location, bool isPlural);
		protected virtual string LotionDidNothing(string location, bool isPlural)
		{
			return "Aside from feeling very warm and pleasant for a while, nothing seems to happen. Well, that's disappointing.";
		}

		public override bool Equals(CapacityItem other)
		{
			return other is BodyLotionBase lotionBase && Equals(lotionBase);
		}
	}
}
