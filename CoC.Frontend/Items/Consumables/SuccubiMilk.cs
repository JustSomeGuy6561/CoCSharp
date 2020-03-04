//SuccubiMilk.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:29:52 AM

using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class SuccubiMilk : StandardConsumable
	{
		private readonly bool isPurified;

		public SuccubiMilk(bool purified) : base()
		{
			isPurified = purified;
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return isPurified ? "P.S.Milk" : "SucMilk";
		}

		public override string ItemName()
		{
			return isPurified ? "Purified Succubi Milk" : "Succubi Milk";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{

			string bottleText = (isPurified ? "untainted " : "") + (count != 1 ? "bottles" : "bottle");

			string countText = "";
			if (displayCount && count == 1)
			{
				countText = isPurified ? "an " : "a ";
			}
			else if (displayCount)
			{
				countText = Utils.NumberAsPlace(count);
			}
			return $"{count}{bottleText} of Succubi milk";
		}

		public override string AboutItem()
		{
			return "This milk-bottle is filled to the brim with a creamy white milk of dubious origin. A pink label proudly labels it as \"" +
				SafelyFormattedString.FormattedText("Succubi Milk", StringFormats.ITALIC) + ".\" In small text at the bottom of the label it reads: \"" +
				SafelyFormattedString.FormattedText("To bring out the succubus in YOU!", StringFormats.ITALIC) + "\"" +
				(isPurified ? " Purified by Rathazul to prevent corruption." : "");
		}

		public override bool countsAsLiquid => true;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 20;

		protected override int monetaryValue => isPurified ? 20 : DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is SuccubiMilk succubi && this.isPurified == succubi.isPurified;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new SuccubiTFs(isPurified);
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);

			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new SuccubiTFs(isPurified);
			resultsOfUse = tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);

			return true;
		}

		private class SuccubiTFs : DemonTransformations
		{
			public SuccubiTFs(bool purified) : base(Gender.FEMALE, purified)
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
