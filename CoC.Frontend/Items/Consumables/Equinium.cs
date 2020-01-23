﻿//HorseTransformations.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 7:00:51 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class Equinium : ConsumableBase
	{
		public Equinium() : base()
		{
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string Appearance()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}


		public override bool countsAsLiquid => true;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 15;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is Equinium;
		}

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new HorseTFs();
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);

			return true;
		}

		private class HorseTFs : HorseTransformations
		{
			protected override bool InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}