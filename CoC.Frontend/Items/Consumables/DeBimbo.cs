/**
 * Created by aimozg on 10.01.14.
 */
using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Perks;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class DeBimbo : StandardConsumable
	{

		public override string AbbreviatedName() => "Debimbo";
		public override string ItemName() => "Debimbo";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "bottles" : "bottle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} marked as \"Debimbo\"";
		}
		public override string AboutItem()
		{
			if (CreatureStore.activePlayer.HasPerk<BimBro>() && CreatureStore.activePlayer.GetPerkData<BimBro>().bimbroBrains)
			{
				return "This should totally like, fix your brain and stuff. You don't really think anything is wrong with your head - it feels all pink and giggly all the time.";
			}
			else
			{
				return "This draft is concocted from five scholar's teas and who knows what else. Supposedly it will correct the stupidifying effects of Bimbo Liqueur. " + GlobalStrings.NewParagraph() + "Type: Consumable";
			}
		}

		protected override int monetaryValue => 250;

		public override bool countsAsCum => false;
		public override bool countsAsLiquid => true;

		public override byte sateHungerAmount => 0;

		public DeBimbo() : base()
		{ }

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			if (target.GetPerkData<BimBro>()?.bimbroBrains == true)
			{
				whyNot = null;
				return true;
			}

			whyNot = "You can't use this right now, and it's too expensive to waste!" + GlobalStrings.NewParagraph();
			return false;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			BimBro bimbo = consumer.GetPerkData<BimBro>();
			BimBroBrains brains = consumer.GetConditionalPerkData<BimBroBrains>(false);

			isBadEnd = false;
			if (bimbo is null || !brains.isEnabled)
			{
				resultsOfUse = "Your brain is functioning normally. What a waste of such an expensive drug.";
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				if (bimbo.broBody)
				{
					sb.Append("You pinch your nose and swallow the foul-tasting mixture with a grimace. Oh, that's just <i>nasty!</i> You drop the vial, which shatters on the ground, clutching at your head as a wave of nausea rolls over you. Stumbling back against a rock for support, you close your eyes. A constant, pounding ache throbs just behind your temples, and for once, you find yourself speechless. A pained groan slips through your lips as thoughts and memories come rushing back. One after another, threads of cognizant thought plow through the simple matrices of your bro mind, shredding and replacing them.");
					sb.Append(GlobalStrings.NewParagraph() + "You... you were a brute who constantly thinks of working out and fucking! You shudder as your faculties return, the pain diminishing with each passing moment.");
				}
				else
				{
					sb.Append("Well, time to see what this smelly, old rat was on about! You pinch your nose and swallow the foul-tasting mixture with a grimace. Oh, that's just <i>nasty!</i> You drop the vial, which shatters on the ground, clutching at your head as a wave of nausea rolls over you. Stumbling back against a rock for support, you close your eyes. A constant, pounding ache throbs just behind your temples, and for once, you find yourself speechless. A pained groan slips through your lips as thoughts and memories come rushing back. One after another, threads of cognizant thought plow through the simple matrices of your bimbo mind, shredding and replacing them.");
					sb.Append(GlobalStrings.NewParagraph() + "You... you were an air-headed ditz! A vacuous, idiot-girl with nothing between her ears but hunger for dick and pleasure! You shudder as your faculties return, the pain diminishing with each passing moment.");
				}

				string removed = brains.Name();
				string perkType = bimbo.futaBody ? "futanari bimbo" : (bimbo.broBody ? "male who constantly works out" : "bimbo");

				sb.Append("(<b>Perk Removed: " + removed + " - Your intelligence and speech patterns are no longer limited to that of a " + perkType + ".</b>)");

				bimbo.NegateBimbroBrains();

				resultsOfUse = sb.ToString();
			}
			return true;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is DeBimbo;
		}
	}
}
