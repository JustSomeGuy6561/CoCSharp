﻿//ImpFood.cs
//Description:
//Author: JustSomeGuy
//10/25/2019 11:27:34 PM

#warning override the combat versions of all remaining transformation items. implement naga tfs. implement remaining items fully. die on inside

using System;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class ImpFood : StandardConsumable
	{
		public ImpFood() : base() { }



		public override bool countsAsLiquid => false;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 20;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is ImpFood;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			ImpFoodTFs transform = new ImpFoodTFs();
			consumeItem = true;
			return transform.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			ImpFoodTFs transform = new ImpFoodTFs();
			consumeItem = true;
			return transform.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class ImpFoodTFs : ImpTransformation
		{
			public ImpFoodTFs()
			{
			}

			protected override string ChangeSkinColorText(Creature target, Tones oldSkinTone)
			{
				return ImpFood.ChangeSkinColorText(target, oldSkinTone);
			}

			protected override string CurrentBreastRowChanged(Creature target, int index, byte cupSizesShrunk, byte rowsPreviouslyModified)
			{
				return ImpFood.CurrentBreastRowChanged(target, index, cupSizesShrunk, rowsPreviouslyModified);
			}

			protected override string GainVitalityText(Creature target)
			{
				return ImpFood.GainVitalityText(target);
			}

			protected override string GetShorterText(Creature target, byte heightDelta)
			{
				return ImpFood.GetShorterText(target, heightDelta);
			}

			protected override string HairChangedText(Creature target, HairData oldHairData)
			{
				return ImpFood.HairChangedText(target, oldHairData);
			}

			protected override string ImpifiedText(Creature target, GenitalsData previousGenitals)
			{
				return ImpFood.ImpifyText(target, previousGenitals);
			}

			protected override string InitialTransformText(Creature target)
			{
				return ImpFood.InitialTransformText(target);
			}

			protected override string OneCockGrewLarger(Creature target, CockData oldData, double delta)
			{
				return target.genitals.allCocks.GenericChangeOneCockLengthText(oldData);
			}

			protected override string RemovedQuadNippleText(Creature target)
			{
				return ImpFood.RemovedQuadNippleText(target);
			}
		}

	}
}
