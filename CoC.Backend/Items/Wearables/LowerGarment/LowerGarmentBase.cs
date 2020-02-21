using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Strings;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.LowerGarment
{
	public abstract class LowerGarmentBase : WearableItemBase<LowerGarmentBase>
	{
		protected LowerGarmentBase() : base()
		{}

		private protected override LowerGarmentBase UpdateCreatureEquipmentInternal(Creature target)
		{
			return target.ChangeLowerGarment(this);
		}

		private protected override DisplayBase AttemptToUseSafe(Creature target, UseItemCallbackSafe<LowerGarmentBase> postItemUseCallbackSafe)
		{
			return AttemptToUse(target, postItemUseCallbackSafe);
		}

		//you can now give this a menu if you really want. for a valid example, see bimbo skirt.
		protected virtual DisplayBase AttemptToUse(Creature creature, UseItemCallbackSafe<LowerGarmentBase> useItemCallback)
		{
			if (!CanUse(creature, false, out string whyNot))
			{
				useItemCallback(false, whyNot, Author(), this);
				return null;
			}
			else
			{
				LowerGarmentBase retVal = ChangeEquipment(creature, out string resultsOfUse);
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
			if (creature.armor?.CanWearWithLowerGarment(creature, this, out whyNot) == false)
			{
				return false;
			}
			whyNot = null;
			return true;
		}


		//by default, we assume that the item cannot support non-bipedal creatures. override this to support them.
		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			if (creature.isBiped || creature.lowerBody.type == LowerBodyType.GOO)
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = GenericRequireBipedText(creature);
				return false;
			}
		}

		protected string GenericRequireBipedText(Creature creature)
		{
			throw new NotImplementedException();
		}


		public override string AboutItemWithStats(Creature target)
		{
			string defenseDifference = DefenseDifference(target.lowerGarment);


			return AboutItem() + GlobalStrings.NewParagraph() + "Type: Lower Garment" + Environment.NewLine
				+ "Defense: " + PhysicalDefensiveRating(target) + defenseDifference + Environment.NewLine
				+ (BonusTeaseRate(target) > 0 ? "Sexiness: " + BonusTeaseRate(target) + Environment.NewLine : "")
				+ "Base value: " + monetaryValue;
		}


		public static LowerGarmentBase NOTHING { get; } = new Nothing();

		public bool isNothing => this is Nothing;

		public static bool IsNullOrNothing(LowerGarmentBase lowerGarment)
		{
			return lowerGarment is null || lowerGarment.isNothing;
		}

		private sealed class Nothing : LowerGarmentBase
		{
			public Nothing()
			{
			}

			public override double PhysicalDefensiveRating(Creature wearer) => 0;

			public override bool Equals(LowerGarmentBase other)
			{
				return other is null || other is Nothing;
			}

			public override string AbbreviatedName() => "";

			public override string ItemName() => "";

			public override string ItemDescription(byte count = 1, bool displayCount = false) => "";
			public override string AboutItem() => "";

			protected override int monetaryValue => 0;

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

			protected override string RemoveText(Creature wearer)
			{
				return "";
			}

			protected override LowerGarmentBase OnRemove(Creature wearer)
			{
				return null;
			}

			public override byte maxCapacityPerSlot => 5;


		}
	}
}
