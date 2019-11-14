//ImpFood.cs
//Description:
//Author: JustSomeGuy
//Note: date follows MMDDYYYY format.
//10/25/2019 11:27:34 PM

using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class ImpFood : ConsumableBase
	{
		public ImpFood() : base(Short, Full, Desc)
		{
		}

		private static string Short()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Full()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Desc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//does this consumable count as liquid for slimes?
		public override bool countsAsLiquid => false;
		//does this consumable count as cum for succubi?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 20;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var transform = new ImpFoodTFs();
			resultsOfUse = transform.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		private class ImpFoodTFs : ImpTransformation
		{
			public ImpFoodTFs()
			{
			}

			protected override string ChangeArmText(Creature target, ArmType oldType)
			{
				throw new NotImplementedException();
			}

			protected override string ChangeEarsText(Creature target, EarType oldType)
			{
				throw new NotImplementedException();
			}

			protected override string ChangeLowerBodyText(Creature target, LowerBodyType oldType)
			{
				throw new NotImplementedException();
			}

			protected override string ChangeOrGrowHornsText(Creature target, EarType oldType)
			{
				throw new NotImplementedException();
			}

			protected override string ChangeSkinColorText(Creature target, Tones oldSkinTone, Tones tone)
			{
				throw new NotImplementedException();
			}

			protected override string CurrentBreastRowChanged(Creature target, Breasts breast, byte delta, byte rowsModified, bool wasLargeChange)
			{
				throw new NotImplementedException();
			}

			protected override string EnlargenedImpWingsText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string GainVitalityText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string GetShorterText(Creature target, byte heightDelta)
			{
				throw new NotImplementedException();
			}

			protected override string GrowOrChangeTailText(Creature target, TailType oldType)
			{
				throw new NotImplementedException();
			}

			protected override string GrowOrChangeWingsText(Creature target, WingType oldType)
			{
				throw new NotImplementedException();
			}

			protected override string HairChangedText(Creature target, HairData oldHairData, HairFurColors hairColor, float hairLength)
			{
				throw new NotImplementedException();
			}

			protected override string ImpifiedText(Creature target, int breastRowsRemoved, bool madeFinalBreastRowMale, bool grewCock, bool grewBalls)
			{
				throw new NotImplementedException();
			}

			protected override string InitialTransformText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string OneCockGrewLarger(Creature target, Cock cock, float temp)
			{
				throw new NotImplementedException();
			}

			protected override string RemovedAdditionalBreasts(Creature target, int removeCount)
			{
				throw new NotImplementedException();
			}

			protected override string RemovedOvipositionText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string RemovedQuadNippleText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string RestoredBackText(Creature target, NeckType oldType)
			{
				throw new NotImplementedException();
			}

			protected override string RestoredNeckText(Creature target, NeckType oldType)
			{
				throw new NotImplementedException();
			}
		}

	}
}
