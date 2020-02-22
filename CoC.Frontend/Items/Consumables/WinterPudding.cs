using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * Holiday festive item that might give you antlers.
	 */
	public class WinterPudding : StandardConsumable
	{

		public WinterPudding() : base()
		{ }

		public override string AbbreviatedName() => "W.Pddng";
		public override string ItemName() => "Winter Pudding";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string itemText = count != 1 ? "slices" : "slice";
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{itemText} of winter pudding";
		}
		public override string AboutItem() => "A slice of Winter Pudding. It smells delicious. ";
		protected override int monetaryValue => 35;
		public override bool countsAsCum => false;
		public override bool countsAsLiquid => false;

		public override bool Equals(CapacityItem other)
		{
			return other is WinterPudding;
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tfs = new ReindeerTFs();
			resultsOfUse = tfs.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			var tfs = new ReindeerTFs();
			resultsOfUse = tfs.DoTransformationFromCombat(consumer, out isCombatLoss, out isBadEnd);
			return true;
		}

		public override byte sateHungerAmount => 30;


		private class ReindeerTFs : ReindeerTransformations
		{
			protected override string InitialTransformationText(Creature target)
			{
				return "You stuff the stodgy pudding down your mouth, the taste of brandy cream sauce and bitter black treacle sugar combining in your mouth." +
					" You can tell by its thick spongy texture that it's far from good for you, so its exclusivity is more than likely for the best.";
			}

			protected override string PostBuildChangesText(Creature target, double lustGain)
			{
				return GlobalStrings.NewParagraph() + "You lick your lips clean, savoring the taste of the Winter Pudding. You feel kinda antsy...";
			}

			protected override string UpdatedHornsText(Creature target, HornData oldData)
			{
				if (oldData.hornCount == 0)
				{
					return GlobalStrings.NewParagraph() + "You hear the sound of cracking branches erupting from the tip of your skull. Small bulges on either side of your head advance outwards in a straight line, eventually spreading out in multiple directions like a miniature tree. Investigating the exotic additions sprouting from your head, the situation becomes clear. <b>You've grown antlers!</b>";
				}
				else
				{
					return GlobalStrings.NewParagraph() + "You hear the sound of cracking branches erupting from the tip of your skull. The horns on your head begin to twist and turn fanatically, their texture and size morphing considerably until they resemble something more like trees than anything else. Branching out rebelliously, you've come to the conclusion that <b>you've somehow gained antlers!</b>";
				}

			}
		}
	}

}