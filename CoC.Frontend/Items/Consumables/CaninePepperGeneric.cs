using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;

namespace CoC.Frontend.Items.Consumables
{
	public partial class CaninePepperGeneric : StandardConsumable
	{
		protected readonly CanineModifiers modifiers;
		private readonly SimpleDescriptor abbreviated;
		private readonly SimpleDescriptor name;
		private readonly ItemDescriptor description;
		private readonly SimpleDescriptor appearText;

		public CaninePepperGeneric(CanineModifiers canineModifiers, SimpleDescriptor abbreviatedName, SimpleDescriptor itemName, ItemDescriptor itemDescription,
			SimpleDescriptor appearance, int value = 10) : base()
		{
			modifiers = canineModifiers;
			monetaryValue = value;

			abbreviated = abbreviatedName ?? throw new ArgumentNullException(nameof(abbreviatedName));
			name = itemName ?? throw new ArgumentNullException(nameof(itemName));
			description = itemDescription ?? throw new ArgumentNullException(nameof(itemDescription));
			appearText = appearance ?? throw new ArgumentNullException(nameof(appearance));
		}



		public override string AbbreviatedName() => abbreviated();

		public override string ItemName() => name();

		public override string ItemDescription(byte count = 1, bool displayCount = false) => description(count, displayCount);

		public override string AboutItem() => appearText();


		public override bool countsAsLiquid => false;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 10; //not originally there. but it makes sense.

		protected override int monetaryValue { get; }

		public override bool Equals(CapacityItem other)
		{
			return other is CaninePepperGeneric caninePepper && caninePepper.modifiers == this.modifiers;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			CanineTFs transform = new CanineTFs(modifiers);
			consumeItem = true;
			return transform.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			CanineTFs transform = new CanineTFs(modifiers);
			consumeItem = true;
			return transform.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		private class CanineTFs : CanineTransformations
		{
			public CanineTFs(CanineModifiers canineModifiers) : base(canineModifiers) { }

			protected override string AddedCumText(Creature target, double delta, bool deltaIsMultiplier) => CaninePepperGeneric.AddedCumText(target, delta, deltaIsMultiplier);

			protected override string BadEndText(Creature target) => CaninePepperGeneric.BadEndText(target);

			protected override string ChangedFurText(Creature target, ReadOnlyFurColor oldFurColor) => CaninePepperGeneric.ChangedFurText(target, oldFurColor);


			protected override string ConvertedFirstDogCockGrewSecond(Creature target, CockData oldCockData) =>
				CaninePepperGeneric.ConvertedFirstDogCockGrewSecond(target, oldCockData);


			protected override string ConvertedOneCockToDogHadOne(Creature target, int index, CockData oldData) =>
				CaninePepperGeneric.ConvertedOneCockToDogHadOne(target, index, oldData);


			protected override string ConvertedTwoCocksToDog(Creature target, CockData firstOldData, CockData secondOldData) =>
				CaninePepperGeneric.ConvertedTwoCocksToDog(target, firstOldData, secondOldData);


			protected override string CouldntConvertDemonCockThickenedInstead(Creature target, int index, double delta) =>
				CaninePepperGeneric.CouldntConvertDemonCockThickenedInstead(target, index, delta);


			protected override string DoggoFantasyText(Creature target) => CaninePepperGeneric.DoggoFantasyText(target);


			protected override string DoggoWarningText(Creature target, bool wasPreviouslyWarned) => CaninePepperGeneric.DoggoWarningText(target, wasPreviouslyWarned);


			protected override string EnlargedBallsText(Creature target, BallsData oldData) => CaninePepperGeneric.EnlargedBallsText(target, oldData);


			protected override string EnlargedSmallestKnotText(Creature target, int indexOfSmallestKnotCock, double knotMultiplierDelta, bool hasOtherDogCocks) =>
				CaninePepperGeneric.EnlargedSmallestKnotText(target, indexOfSmallestKnotCock, knotMultiplierDelta, hasOtherDogCocks);


			protected override string FallbackToughenUpText(Creature target) => CaninePepperGeneric.FallbackToughenUpText(target);


			protected override string GrewBallsText(Creature target) => CaninePepperGeneric.GrewBallsText(target);


			protected override string GrewSecondDogCockHadOne(Creature target) => CaninePepperGeneric.GrewSecondDogCockHadOne(target);


			protected override string GrewSmallestCockText(Creature target, int index, CockData oldData) => CaninePepperGeneric.GrewSmallestCockText(target, index, oldData);


			protected override string GrewTwoDogCocksHadNone(Creature target) => CaninePepperGeneric.GrewTwoDogCocksHadNone(target);


			protected override string InitialTransformationText(CanineModifiers modifiers, bool crit) => CaninePepperGeneric.InitialTransformationText(modifiers, crit);


			protected override string NormalizedBreastSizeText(Creature target, BreastCollectionData oldBreasts) => CaninePepperGeneric.NormalizedBreastSizeText(target, oldBreasts);


			protected override string NothingHappenedGainHpText(Creature target) => CaninePepperGeneric.NothingHappenedGainHpText(target);


			protected override string StatChangeText(Creature target, double strengthIncrease, double speedIncrease, double intelligenceDecrease) =>
				CaninePepperGeneric.StatChangeText(target, modifiers, strengthIncrease, speedIncrease, intelligenceDecrease);


			protected override string UpdateAndGrowAdditionalRowText(Creature target, BreastCollectionData oldBreasts, bool crit, bool uberCrit) =>
				CaninePepperGeneric.UpdateAndGrowAdditionalRowText(target, oldBreasts, crit, uberCrit);


			protected override string WastedKnottyText(Creature target) => CaninePepperGeneric.WastedKnottyText(target);

		}
	}
}
