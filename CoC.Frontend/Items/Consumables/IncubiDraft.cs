//IncubiDraft.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:29:40 AM

using System;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class IncubiDraft : StandardConsumable
	{
		private readonly bool isPurified;

		public IncubiDraft(bool purified) : base()
		{
			isPurified = purified;
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return isPurified ? "P.Draft" : "IncubiD";
		}

		public override string ItemName()
		{
			return isPurified ? "purified Incubi draft" : "Incubi draft";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string taintText = isPurified ? "untainted " : "";
			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{taintText}Incubi draft";
		}

		public override string AboutItem()
		{
			return "The cork-topped flask swishes with a slimy looking off-white fluid, purported to give incubi-like powers. A stylized picture " +
				"of a humanoid with a huge penis is etched into the glass." + (isPurified ? " Rathazul has purified this to prevent corruption upon use." : "");
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => true;
		public override byte sateHungerAmount => 10;
		protected override int monetaryValue => isPurified ? 20 : DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is IncubiDraft incubi && this.isPurified == incubi.isPurified;
		}

		//can we use this item on the given creature? if not, provide a valid string explaining why not. that text will be displayed as a hint to the user.
		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			IncubiTFs tf = new IncubiTFs(isPurified);
			consumeItem = true;
			return tf.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			IncubiTFs tf = new IncubiTFs(isPurified);
			consumeItem = true;
			return tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class IncubiTFs : DemonTransformations
		{
			public IncubiTFs(bool purified) : base(Gender.MALE, purified)
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
