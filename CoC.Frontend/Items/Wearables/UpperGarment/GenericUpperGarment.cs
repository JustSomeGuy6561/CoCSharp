using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.UpperGarment;

namespace CoC.Frontend.Items.Wearables.UpperGarment
{
	class GenericUpperGarment : UpperGarmentBase
	{
		private readonly Guid uniqueID;
		private readonly SimpleDescriptor abbreviated;
		private readonly SimpleDescriptor fullName;
		private readonly ItemDescriptor descriptor;
		private readonly SimpleDescriptor about;
		private readonly float defense;
		private readonly byte sexiness;
		private readonly int price;

		public GenericUpperGarment(Guid upperGarmentID, SimpleDescriptor abbreviated, SimpleDescriptor fullName, ItemDescriptor descriptor, SimpleDescriptor about, float defense, byte sexiness, int price)
		{
			uniqueID = upperGarmentID;
			this.abbreviated = abbreviated ?? throw new ArgumentNullException(nameof(abbreviated));
			this.fullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
			this.descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
			this.about = about ?? throw new ArgumentNullException(nameof(about));
			this.defense = defense;
			this.sexiness = sexiness;
			this.price = price;
		}

		public override double BonusTeaseRate(Creature wearer) => sexiness;

		public override double PhysicalDefensiveRating(Creature wearer)
		{
			return defense;
		}

		public override bool Equals(UpperGarmentBase other)
		{
			return other is GenericUpperGarment generic && generic.uniqueID == uniqueID;
		}

		public override string AbbreviatedName() => abbreviated();

		public override string ItemName() => fullName();

		public override string ItemDescription(byte count = 1, bool displayCount = false) => descriptor(count, displayCount);

		public override string AboutItem() => about();

		protected override int monetaryValue => price;
	}
}
