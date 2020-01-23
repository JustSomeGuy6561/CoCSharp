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
	public sealed partial class SuccubiMilk : ConsumableBase
	{
		private readonly bool isPurified;

		public SuccubiMilk(bool purified) : base()
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

		public override bool countsAsLiquid => true;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 20;

		protected override int monetaryValue => isPurified ? 20 : DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is SuccubiMilk succubi && this.isPurified == succubi.isPurified;
		}

		public override bool CanUse(Creature target, out string whyNot)
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

		private class SuccubiTFs : DemonTransformations
		{
			public SuccubiTFs(bool purified) : base(Gender.FEMALE, purified)
			{
			}

			protected override bool InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
