using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Frontend.Transformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
	public partial class CaninePepperGeneric : ConsumableBase
	{
		protected readonly CanineModifiers modifiers;
		public CaninePepperGeneric(CanineModifiers canineModifiers, SimpleDescriptor shortName, SimpleDescriptor fullName,
			SimpleDescriptor shortDescription, SimpleDescriptor appearance, int value = 10) : base(shortName, fullName, shortDescription, appearance)
		{
			modifiers = canineModifiers;
			monetaryValue = value;
		}

		public override bool countsAsLiquid => false;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 10; //not originally there. but it makes sense.

		protected override int monetaryValue { get; }

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var transform = new CanineTFs(modifiers);
			resultsOfUse = transform.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		private class CanineTFs : CanineTransformations
		{
			public CanineTFs(CanineModifiers canineModifiers) : base(canineModifiers) { }

			protected override string InitialTransformationText(CanineModifiers modifiers, bool crit)
			{
				throw new NotImplementedException();
			}

			protected override string BadEndText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string DoggoWarningText(Creature target, bool wasPreviouslyWarned)
			{
				throw new NotImplementedException();
			}

			protected override string StatChangeText(Creature target, float strengthIncrease, float speedIncrease, float intelligenceDecrease)
			{
				throw new NotImplementedException();
			}

			protected override string EnlargedSmallestKnotText(Creature target, int indexOfSmallestKnotCock, float knotMultiplierDelta, bool hasOtherDogCocks)
			{
				throw new NotImplementedException();
			}

			protected override string GrewTwoDogCocksHadNone(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string GrewSecondDogCockHadOne(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string ConvertedFirstDogCockGrewSecond(Creature target, CockData oldCockData)
			{
				throw new NotImplementedException();
			}

			protected override string ConvertedTwoCocksToDog(Creature target, CockData firstOldData, CockData secondOldData)
			{
				throw new NotImplementedException();
			}

			protected override string GrewBallsText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string EnlargedBallsText(Creature target, BallsData oldData)
			{
				throw new NotImplementedException();
			}

			protected override string ConvertedOneCockToDog(Creature target, int index, CockData oldData)
			{
				throw new NotImplementedException();
			}

			protected override string CouldntConvertDemonCockThickenedInstead(Creature target, int index, float delta)
			{
				throw new NotImplementedException();
			}

			protected override string AddedCumText(Creature target, float delta, bool deltaIsMultiplier)
			{
				throw new NotImplementedException();
			}

			protected override string GrewSmallestCockText(Creature target, int index, CockData oldData)
			{
				throw new NotImplementedException();
			}

			protected override string GrewAdditionalBreastRowText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string GrowCurrentBreastRowText(Creature target, int index, byte delta)
			{
				throw new NotImplementedException();
			}

			protected override string NormalizedBreastSizeText(Creature target, int index, short cupSizeDelta)
			{
				throw new NotImplementedException();
			}

			protected override string EnterOrIncreaseHeatText(Creature target, bool isIncrease)
			{
				throw new NotImplementedException();
			}

			protected override string DoggoFantasyText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string ChangedFurText(Creature target, ReadOnlyFurColor oldFurColor)
			{
				throw new NotImplementedException();
			}

			protected override string FallbackToughenUpText(Creature target)
			{
				throw new NotImplementedException();
			}

			protected override string NothingHappenedGainHpText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
