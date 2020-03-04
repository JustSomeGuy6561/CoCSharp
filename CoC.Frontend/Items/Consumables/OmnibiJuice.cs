//OmnibiJuice.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:30:05 AM

using System;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class OmnibiJuice : StandardConsumable
	{
		private readonly bool isPurified;

		public OmnibiJuice(bool purified) : base()
		{
			isPurified = purified;
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return isPurified ? "P.OmniJc" : "OmniJuc";
		}

		public override string ItemName()
		{
			return isPurified ? "purified Omnibi juice" : "Omnibi juice";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string vialText = (isPurified ? "untainted " : "") + (count != 1 ? "vials" : "vial");

			string countText = "";
			if (displayCount && count == 1)
			{
				countText = isPurified ? "an " : "a ";
			}
			else if (displayCount)
			{
				countText = Utils.NumberAsPlace(count);
			}
			return $"{count}{vialText} of Omnibi juice";
		}

		public override string AboutItem()
		{
			return "This concoction, in what appears to be a crystalline vial, a translucent, milky brew seen nothing but a myth till now, is a flask of Omnibi Juice." +
				(isPurified ? " This has been purified by Rathazul to prevent corruption." : "");
		}

		public override bool countsAsLiquid => true;

		//i mean, partially, but i don't think it's pure enough to count it, idk.
		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 10;

		protected override int monetaryValue => isPurified ? 40 : 20;

		public override bool Equals(CapacityItem other)
		{
			return other is OmnibiJuice omnibi && this.isPurified == omnibi.isPurified;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			OmnibiTFs tf = new OmnibiTFs(isPurified);

			consumeItem = true;
			return tf.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			OmnibiTFs tf = new OmnibiTFs(isPurified);

			consumeItem= true;
			return tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class OmnibiTFs : DemonTransformations
		{
			public OmnibiTFs(bool purified) : base(Gender.HERM, purified)
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
