using System;
using System.Text;
using CoC.Backend;
using CoC.Backend.BodyParts;
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
* ...
* @author ...
*/
	public class GirthoPlus : ConsumableWithMenuBase
	{

		public GirthoPlus() : base()
		{ }

		public override string AbbreviatedName() => "GirthoP";
		public override string ItemName() => "GirthoPlus";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "needles" : "needle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} filled with Girtho+";
		}
		public override string AboutItem() => "This is a small needle with a reservoir full of blue liquid with white lines spiraling along its length. " +
			"A faded label marks it as \"GirthoPlus\". If it's of any indication, it would focus on thickening cocks.";
		protected override int monetaryValue => 100;

		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 0;
		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			bool retVal = target.hasCock;
			whyNot = HasCockText(retVal);
			return retVal;
		}

		private string HasCockText(bool hasCock)
		{
			if (!hasCock)
			{
				return "You need to have a cock to increase its thickness!";
			}
			else
			{
				return null;
			}
		}

		protected override DisplayBase BuildMenu(Creature consumer, UseItemCallback postItemUseCallback)
		{

			if (!consumer.hasCock)
			{
				string results = "As you look over the needle, you realize that its intended purpose is to thicken a penis which you do not possess. Sighing dejectedly, you put away the needle.";
				postItemUseCallback(false, results, null, this);
			}
			StandardDisplay display = new StandardDisplay();

			display.OutputText("You ponder the needle in your hand knowing it will thicken a cock it's injected into.");
			if (consumer.cocks.Count == 1)
			{
				display.AddButton(0, "Inject", () => ChooseSingleCock(consumer, postItemUseCallback));
				display.AddButton(1, "Don't", () => GirthPlusCancel(consumer, postItemUseCallback));
			}
			else
			{
				for (int i = 0; i < consumer.cocks.Count; i++)
				{
					string tip = consumer.cocks[i].LongDescription() + Environment.NewLine + "Length: " + consumer.cocks[i].length + Environment.NewLine
						+ "Thickness: " + consumer.cocks[i].girth;

					display.AddButtonWithToolTip((byte)i, "Cock #" + (i + 1), () => ChooseSingleCock(consumer, postItemUseCallback, i), tip);
				}
				display.AddButton(10, GlobalStrings.ALL(), () => ChooseAllCocks(consumer, postItemUseCallback));
				display.AddButton(14, GlobalStrings.NEVERMIND(), () => GirthPlusCancel(consumer, postItemUseCallback));
			}

			return display;
		}

		private void ChooseSingleCock(Creature consumer, UseItemCallback postItemUseCallback, int cockNum = 0)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("You sink the needle into the base of your " + consumer.cocks[cockNum].LongDescription() + ". It hurts like hell, " +
				"but as you depress the plunger, the pain vanishes, replaced by a tingling pleasure as the chemicals take effect." + GlobalStrings.NewParagraph());
			sb.Append("Your " + consumer.cocks[cockNum].LongDescription() + " twitches and thickens, looking to be ");
			if (consumer.cocks[cockNum].girth < consumer.cocks[cockNum].length / 10)
			{
				consumer.cocks[cockNum].IncreaseThickness(consumer.cocks[cockNum].girth * 0.5);
				sb.Append("fattening significantly!");
			}
			else if (consumer.cocks[cockNum].girth < consumer.cocks[cockNum].length / 6)
			{
				consumer.cocks[cockNum].IncreaseThickness(consumer.cocks[cockNum].girth * 0.3);
				sb.Append("thickening quite noticably.");
			}
			else if (consumer.cocks[cockNum].girth < consumer.cocks[cockNum].length / 4)
			{
				consumer.cocks[cockNum].IncreaseThickness(consumer.cocks[cockNum].girth * 0.2);
				sb.Append("thickening noticably.");
			}
			else if (consumer.cocks[cockNum].girth < consumer.cocks[cockNum].length / 2)
			{
				consumer.cocks[cockNum].IncreaseThickness(consumer.cocks[cockNum].girth * 0.1);
				sb.Append("widening slightly.");
			}
			else
			{
				sb.Append("widening slightly, only to shrink back to how it was before you even injected. Seems like your extremely thick cock is already fat enough.");
			}
			sb.Append(GlobalStrings.NewParagraph() + "Once the growth has stopped, you discard the now-empty syringe.");

			postItemUseCallback(true, sb.ToString(), null, null);
		}

		private void ChooseAllCocks(Creature consumer, UseItemCallback postItemUseCallback)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("You sink the needle into the base of your " + consumer.genitals.AllCocksShortDescription() + ". It hurts like hell, but as you depress the plunger, " +
				"the pain vanishes, replaced by a tingling pleasure as the chemicals take effect." + GlobalStrings.NewParagraph());
			int cocksAffected = 0;

			//1 - 2/3log(count). when count is 10, it's at 33% efficiency. when at ~3, it's at 66% efficiency. log makes it drop faster early, but
			//drops slower later, making it feel more useful with more than 5 cocks. you could make that 1/2 or something instead of 2/3, as desired.

			double drugEffectiveness = 1 - (2 / 3.0 * Math.Log10(consumer.cocks.Count));
			foreach (Cock cock in consumer.cocks)
			{
				if (cock.length > cock.girth * 2)
				{
					//offset = 0.5 if > 10, .4 if >8, .3 if > 6, .2 if > 4, .1 otherwise.
					double growthMultiplier = Math.Floor(cock.length / cock.girth) / 20.0;
					//the amount grown is the offset above, multiplied by the effectiveness of the drug as a percent of the total girth.
					double delta = growthMultiplier * drugEffectiveness * cock.girth;

					cocksAffected++;
					cock.IncreaseThickness(delta);
				}
			}
			if (cocksAffected == consumer.cocks.Count)
			{
				sb.Append("Your " + consumer.genitals.AllCocksShortDescription() + " twitch and thicken, each of them looking to be fattening and becoming more consistent in thickness.");
			}
			else if (cocksAffected > 1)
			{
				sb.Append(Utils.NumberAsText(cocksAffected) + " of your " + consumer.genitals.AllCocksShortDescription() + " twitch and thicken, each of the affected cocks looking to be fattening and becoming more consistent in thickness.");
			}
			else if (cocksAffected == 1)
			{
				sb.Append("One of your " + consumer.genitals.AllCocksShortDescription() + " twitches and thickens, looking to be fattening.");
			}
			else
			{
				sb.Append("But alas, nothing happens. Looks like all of them are too thick to begin with and you've already wasted it. How disappointing.");
			}
			sb.Append(GlobalStrings.NewParagraph() + "Once the growth has stopped, you discard the now-empty syringe.");

			postItemUseCallback(true, sb.ToString(), null, null);
		}

		private void GirthPlusCancel(Creature consumer, UseItemCallback postItemUseCallback)
		{
			string results = "You put the vial away." + GlobalStrings.NewParagraph();

			postItemUseCallback(false, results, null, this);
		}

		public override bool Equals(CapacityItem other)
		{
			return other is GirthoPlus;
		}

	}

}