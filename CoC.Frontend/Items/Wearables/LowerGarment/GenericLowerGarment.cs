using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.LowerGarment;

namespace CoC.Frontend.Items.Wearables.LowerGarment
{
	class GenericLowerGarment : LowerGarmentBase
	{
		private readonly Guid uniqueID;
		private readonly SimpleDescriptor abbreviated;
		private readonly SimpleDescriptor fullName;
		private readonly ItemDescriptor descriptor;
		private readonly SimpleDescriptor about;
		private readonly double defense;
		private readonly byte sexiness;
		private readonly int price;

		private readonly bool allowsNonBipeds;

		public GenericLowerGarment(Guid lowerGarmentID, SimpleDescriptor abbreviated, SimpleDescriptor fullName, ItemDescriptor descriptor, SimpleDescriptor about, double defense, byte sexiness, int price, bool supportsAllLegCounts)
		{
			uniqueID = lowerGarmentID;
			this.abbreviated = abbreviated ?? throw new ArgumentNullException(nameof(abbreviated));
			this.fullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
			this.descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
			this.about = about ?? throw new ArgumentNullException(nameof(about));
			this.defense = defense;
			this.sexiness = sexiness;
			this.price = price;

			allowsNonBipeds = supportsAllLegCounts;
		}

		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			if (allowsNonBipeds)
			{
				return base.CanWearWithBodyData(creature, out whyNot);
			}
			else
			{
				whyNot = GenericRequireBipedText(creature);
				return false;
			}
		}

		public override double BonusTeaseRate(Creature wearer) => sexiness;

		public override string AboutItemWithStats(Creature target)
		{
			return base.AboutItemWithStats(target) +
				(allowsNonBipeds ? Environment.NewLine + "This type of lower undergarment is not limited to bipeds; Nagas and Centaurs, for example, can wear this without issue." : "");
		}

		public override double PhysicalDefensiveRating(Creature wearer)
		{
			return defense;
		}

		public override bool Equals(LowerGarmentBase other)
		{
			return other is GenericLowerGarment generic && generic.uniqueID == uniqueID;
		}

		public override string AbbreviatedName() => abbreviated();

		public override string ItemName() => fullName();

		public override string ItemDescription(byte count = 1, bool displayCount = false) => descriptor(count, displayCount);

		public override string AboutItem() => about();

		protected override int monetaryValue => price;
	}

	class GenericWellSpringAwareLowerGarment : GenericLowerGarment, ILustWellspring
	{
		public GenericWellSpringAwareLowerGarment(Guid lowerGarmentID, SimpleDescriptor abbreviated, SimpleDescriptor fullName, ItemDescriptor descriptor,
			SimpleDescriptor about, double defense, byte sexiness, int price, bool supportsAllLegCounts, bool hasWellspring = true)
			: base(lowerGarmentID, abbreviated, fullName, descriptor, about, defense, sexiness, price, supportsAllLegCounts)
		{
			hasWellspringOfLust = hasWellspring;
		}

		public bool hasWellspringOfLust { get; }
	}
}
