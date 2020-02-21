using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Strings;
using CoC.Backend.UI;

namespace CoC.Backend.Items.Wearables.UpperGarment
{
	public abstract class UpperGarmentBase : WearableItemBase<UpperGarmentBase>
	{
		protected UpperGarmentBase() : base()
		{ }

		private protected override UpperGarmentBase UpdateCreatureEquipmentInternal(Creature target)
		{
			return target.ChangeUpperGarment(this);
		}

		private protected override DisplayBase AttemptToUseSafe(Creature target, UseItemCallbackSafe<UpperGarmentBase> postItemUseCallbackSafe)
		{
			return AttemptToUse(target, postItemUseCallbackSafe);
		}

		public override string AboutItemWithStats(Creature target)
		{
			string defenseDifference = DefenseDifference(target.upperGarment);


			return AboutItem() + GlobalStrings.NewParagraph() + "Type: Upper Garment" + Environment.NewLine
				+ "Defense: " + PhysicalDefensiveRating(target) + defenseDifference + Environment.NewLine
				+ (BonusTeaseRate(target) > 0 ? "Sexiness: " + BonusTeaseRate(target) + Environment.NewLine : "")
				+ "Base value: " + monetaryValue;
		}


		//you can now give this a menu if you really want. for a valid example, see bimbo skirt.
		protected virtual DisplayBase AttemptToUse(Creature creature, UseItemCallbackSafe<UpperGarmentBase> useItemCallback)
		{
			if (!CanUse(creature, false, out string whyNot))
			{
				useItemCallback(false, whyNot, Author(), this);
				return null;
			}
			else
			{
				UpperGarmentBase retVal = ChangeEquipment(creature, out string resultsOfUse);
				useItemCallback(true, resultsOfUse, Author(), retVal);
				return null;
			}
		}

		public override bool CanUse(Creature creature, bool isInCombat, out string whyNot)
		{
			if (!base.CanUse(creature, isInCombat, out whyNot))
			{
				return false;
			}
			if (creature.armor?.CanWearWithUpperGarment(creature, this, out whyNot) == false)
			{
				return false;
			}
			whyNot = null;
			return true;
		}


		public static UpperGarmentBase NOTHING { get; } = new Nothing();

		public bool isNothing => this is Nothing;

		public static bool IsNullOrNothing(UpperGarmentBase upperGarment)
		{
			return upperGarment is null || upperGarment.isNothing;
		}

		private sealed class Nothing : UpperGarmentBase
		{
			public Nothing()
			{
			}

			public override double PhysicalDefensiveRating(Creature wearer) => 0;

			public override bool Equals(UpperGarmentBase other)
			{
				return other is null || other is Nothing;
			}

			public override string AbbreviatedName() => "";

			public override string ItemName() => "";

			public override string ItemDescription(byte count = 1, bool displayCount = false) => "";
			public override string AboutItem() => "";

			protected override int monetaryValue => 0;

			public override double BonusTeaseRate(Creature wearer) => 0;

			public override bool canBuy => false;

			public override bool canSell => false;

			public override string AboutItemWithStats(Creature target) => "";

			public override bool CanUse(Creature creature, bool isInCombat, out string whyNot)
			{
				whyNot = "Error: Does Not Exist";
				return false;
			}

			protected override string EquipText(Creature wearer)
			{
				return "";
			}

			protected override UpperGarmentBase OnRemove(Creature wearer)
			{
				return null;
			}

			protected override string RemoveText(Creature wearer)
			{
				return "";
			}

			public override byte maxCapacityPerSlot => 5;


		}
	}
}
