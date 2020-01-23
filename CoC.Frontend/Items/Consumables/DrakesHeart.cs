//DrakesHeart.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 5:50:32 PM

using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class DrakesHeart : ConsumableBase
	{
		public DrakesHeart() : base()
		{
		}

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

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => false;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 1;

		protected override int monetaryValue => 50;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is DrakesHeart;
		}
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
#warning when Ember implemented, set these to the correct values.
			var tf = new DragonTF(false, true);

			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		private sealed class DragonTF : DragonTransformations
		{
			public DragonTF(bool allowsDraconicFace, bool backUsesMane) : base(false, allowsDraconicFace, backUsesMane)
			{
			}

			protected override bool InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}

	}
}
