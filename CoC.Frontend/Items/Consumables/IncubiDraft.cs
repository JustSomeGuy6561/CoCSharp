//IncubiDraft.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:29:40 AM

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
	public sealed partial class IncubiDraft : ConsumableBase
	{
		private readonly bool isPurified;

		public IncubiDraft(bool purified) : base()
		{
			isPurified = purified;
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string ItemName()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//if your text uses "an" as an article instead of "a", be sure to change that here.
			string countText = displayCount ? (count == 1 ? "a" : Utils.NumberAsText(count)) : "";
			//when the text below is corrected, remove this throw.
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//update the text below to display what you need.
			return $"{count} <Your Text Here>";
		}

		public override string Appearance()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => true;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => true;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 10;
		protected override int monetaryValue => isPurified ? 20 : DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is IncubiDraft incubi && this.isPurified == incubi.isPurified;
		}

		//can we use this item on the given creature? if not, provide a valid string explaining why not. that text will be displayed as a hint to the user.
		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{

			var tf = new IncubiTFs(isPurified);
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);

			return true;
		}

		private class IncubiTFs : DemonTransformations
		{
			public IncubiTFs(bool purified) : base(Gender.MALE, purified)
			{
			}

			protected override bool InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
