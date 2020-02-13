//FoxBerry.cs
//Description:
//Author: JustSomeGuy
//1/19/2020 3:00:33 AM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class FoxBerry : StandardConsumable
	{
		private readonly bool potent;

		public FoxBerry(bool enhanced) : base()
		{
			potent = enhanced;
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return potent ? "VixVigr" : "Fox Berry";
		}

		public override string ItemName()
		{
			return potent ? "Vixen's Vigor" : "Fox Berry";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			if (potent)
			{
				string vialText = count != 1 ? "bottles" : "bottle";

				string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

				return $"{count}{vialText} labeled \"Vixen's Vigor\"";
			}
			else
			{
				string berryText = count != 1 ? "berries" : "berry";

				string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

				return $"{count} fox {berryText}";
			}
		}

		public override string AboutItem()
		{
			if (potent)
			{
				return "This small medicine bottle contains something called \"Vixen's Vigor\", supposedly distilled from common fox-berries. It is supposed to be " +
				"a great deal more potent, and a small warning label warns of \"extra boobs\", whatever that means.";
			}
			else
			{
				return "This large orange berry is heavy in your hands. It may have gotten its name from its bright orange coloration. You're certain it is no mere fruit.";
			}
		}

		public override bool countsAsLiquid => potent;
		public override bool countsAsCum => false;
		//apparently both are 15. could change that to vary by item. whatever.
		public override byte sateHungerAmount => 15;

		protected override int monetaryValue => potent ? 30 : DEFAULT_VALUE;

		//can we use this item on the given creature? if not, provide a valid string explaining why not. that text will be displayed as a hint to the user.
		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//what happens when we try to use this item? note that it's unlikely, but possible, for this to be called if CanUse returns false.
		//you need to handle that, yourself, and you'll probably want to return some unique text saying you cant do it, you tried anyway,
		//and looked really dumb or something of the like
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new FoxTFs(potent);
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			var tf = new FoxTFs(potent);
			resultsOfUse = tf.DoTransformationFromCombat(consumer, out isCombatLoss, out isBadEnd);
			return true;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is FoxBerry foxBerry && this.potent == foxBerry.potent;
		}

		private sealed class FoxTFs : FoxTransformations
		{
			public FoxTFs(bool potent) : base(potent)
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
