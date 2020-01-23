//OmnibiJuice.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:30:05 AM

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
	public sealed partial class OmnibiJuice : ConsumableBase
	{
		private readonly bool isPurified;

		public OmnibiJuice(bool purified) : base()
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

		//this can be removed safely if the item does a transformation. transformations handle bad end and results text for you.
		private string UseItemText()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
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

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{

			var tf = new OmnibiTFs(isPurified);
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);

			return true;
		}

		private class OmnibiTFs : DemonTransformations
		{
			public OmnibiTFs(bool purified) : base(Gender.HERM, purified)
			{
			}

			protected override bool InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
