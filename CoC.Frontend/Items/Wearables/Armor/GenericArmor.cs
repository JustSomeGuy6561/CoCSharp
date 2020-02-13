using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Perks;

namespace CoC.Frontend.Items.Wearables.Armor
{
	//A generic format for armors that does not require a new class. i'm not overly fond of this approach, but whatever. it uses a Guid to determine if two items are equal.
	class GenericArmor : ArmorBase, ISluttySeductionItem, IWizardEnduranceItem
	{
		protected readonly float defense;
		protected readonly Guid id;
		protected readonly SimpleDescriptor abbr;
		protected readonly SimpleDescriptor full;
		protected readonly ItemDescriptor desc;
		protected readonly SimpleDescriptor about;
		protected readonly bool worksWithUnderclothes;
		protected readonly int value;

		protected readonly byte wizardsEndurance;
		protected readonly byte sluttySeduction;

		public GenericArmor(Guid uniqueID, ArmorType armorRating, SimpleDescriptor abbreviatedName, SimpleDescriptor fullName, ItemDescriptor description,
			SimpleDescriptor aboutText, float defenseRating, int price, byte seductiveModifier = 0, byte wizardModifier = 0, bool allowsUnderGarments = true) : base(armorRating)
		{
			this.id = uniqueID;
			this.defense = defenseRating;
			this.abbr = abbreviatedName ?? throw new ArgumentNullException(nameof(abbreviatedName));
			this.full = fullName ?? throw new ArgumentNullException(nameof(fullName));
			this.desc = description ?? throw new ArgumentNullException(nameof(description));
			this.about = aboutText ?? throw new ArgumentNullException(nameof(aboutText));
			this.worksWithUnderclothes = allowsUnderGarments;

			value = price;

			sluttySeduction = seductiveModifier;
			wizardsEndurance = wizardModifier;
		}


		public override float DefensiveRating(Creature wearer) => defense;

		public override bool Equals(ArmorBase other)
		{
			return other is GenericArmor generic && generic.id == id;
		}

		public override string AbbreviatedName() => abbr();

		public override string ItemName() => full();

		public override string ItemDescription(byte count = 1, bool displayCount = false) => desc(count, displayCount);

		public override string AboutItem() => about();

		protected override int monetaryValue => value;

		public byte SluttySeductionModifier(Creature wearer) => this.sluttySeduction;
		public byte WizardsEnduranceModifier(Creature wearer) => this.wizardsEndurance;

		public override bool CanWearWithUpperGarment(UpperGarmentBase upperGarment, out string whyNot)
		{
			if (!worksWithUnderclothes && !(upperGarment is null))
			{
				whyNot = GenericArmorIncompatText(upperGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override bool CanWearWithLowerGarment(LowerGarmentBase lowerGarment, out string whyNot)
		{
			if (!worksWithUnderclothes && !(lowerGarment is null))
			{
				whyNot = GenericArmorIncompatText(lowerGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}
	}

	internal delegate string BulgeAwareItemDescription(bool isBulged, byte count, bool displayCount);
	internal delegate string BulgeAwareStateChange(Creature wearer, bool isBulged);

	class GenericArmorWithBulge : ArmorBase, IBulgeArmor, ISluttySeductionItem, IWizardEnduranceItem
	{
		protected readonly float defense;
		protected readonly Guid id;
		protected readonly SimpleDescriptor abbr;
		protected readonly DescriptorWithArg<bool> full;
		protected readonly BulgeAwareItemDescription desc;
		protected readonly DescriptorWithArg<bool> about;


		protected readonly BulgeAwareStateChange bulgeStateChangeText;

		protected readonly bool worksWithUnderclothes;
		protected readonly int value;

		protected readonly byte wizardsEndurance;
		protected readonly byte sluttySeduction;

		protected bool bulged;

		public GenericArmorWithBulge(Guid uniqueID, ArmorType armorRating, SimpleDescriptor abbreviatedName, DescriptorWithArg<bool> fullName,
			BulgeAwareItemDescription description, DescriptorWithArg<bool> aboutText, BulgeAwareStateChange onBulgeChange,
			float defenseRating, int price, byte seductiveModifier = 0, byte wizardModifier = 0, bool allowsUnderGarments = true) : base(armorRating)
		{
			this.id = uniqueID;
			this.defense = defenseRating;
			this.abbr = abbreviatedName ?? throw new ArgumentNullException(nameof(abbreviatedName));
			this.full = fullName ?? throw new ArgumentNullException(nameof(fullName));
			this.desc = description ?? throw new ArgumentNullException(nameof(description));
			this.about = aboutText ?? throw new ArgumentNullException(nameof(aboutText));

			this.worksWithUnderclothes = allowsUnderGarments;

			value = price;

			sluttySeduction = seductiveModifier;
			wizardsEndurance = wizardModifier;

			bulgeStateChangeText = onBulgeChange ?? throw new ArgumentNullException(nameof(onBulgeChange));
		}


		public override float DefensiveRating(Creature wearer) => defense;

		public override bool Equals(ArmorBase other)
		{
			return other is GenericArmorWithBulge generic && generic.id == id;
		}

		public override string AbbreviatedName() => abbr();

		public override string ItemName() => full(bulged);

		public override string ItemDescription(byte count = 1, bool displayCount = false) => desc(bulged, count, displayCount);

		public override string AboutItem() => about(bulged);

		protected override int monetaryValue => value;

		public byte SluttySeductionModifier(Creature wearer) => this.sluttySeduction;
		public byte WizardsEnduranceModifier(Creature wearer) => this.wizardsEndurance;

		public override bool CanWearWithUpperGarment(UpperGarmentBase upperGarment, out string whyNot)
		{
			if (!worksWithUnderclothes && !(upperGarment is null))
			{
				whyNot = GenericArmorIncompatText(upperGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override bool CanWearWithLowerGarment(LowerGarmentBase lowerGarment, out string whyNot)
		{
			if (!worksWithUnderclothes && !(lowerGarment is null))
			{
				whyNot = GenericArmorIncompatText(lowerGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public bool supportsBulgeArmor => true;

		string IBulgeArmor.SetBulgeState(Creature wearer, bool bulgified)
		{
			if (bulged == bulgified)
			{
				return "";
			}
			else
			{
				bulged = bulgified;
				return bulgeStateChangeText(wearer, bulged);
			}
		}

		bool IBulgeArmor.isBulged => bulged;
	}
}
