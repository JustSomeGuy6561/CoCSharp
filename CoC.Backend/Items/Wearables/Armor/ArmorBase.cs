using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Strings;
using CoC.Backend.UI;

namespace CoC.Backend.Items.Wearables.Armor
{
	public enum ArmorType { CLOTHING, LIGHT_ARMOR, MEDIUM_ARMOR, HEAVY_ARMOR }

	public abstract class ArmorBase : WearableItemBase<ArmorBase>
	{
		public readonly ArmorType armorType;

		protected ArmorBase(ArmorType armor) : base()
		{
			armorType = armor;
		}

		private protected override ArmorBase UpdateCreatureEquipmentInternal(Creature target)
		{
			return target.ChangeArmor(this);
		}

		private protected override DisplayBase AttemptToUseSafe(Creature target, UseItemCallbackSafe<ArmorBase> postItemUseCallbackSafe)
		{
			return AttemptToUse(target, postItemUseCallbackSafe);
		}

		public virtual bool isNearlyNaked => false;

#warning implement combat tease related armor thing either here or as interface in backend, idk.
		//protected internal virtual CombatTeast combatTeast => null;


		//you can now give this a menu if you really want. for a valid example, see bimbo skirt.
		protected virtual DisplayBase AttemptToUse(Creature creature, UseItemCallbackSafe<ArmorBase> useItemCallback)
		{
			if (!CanUse(creature, false, out string whyNot))
			{
				useItemCallback(false, whyNot, Author(), this);
				return null;
			}
			else
			{
				ArmorBase retVal = ChangeEquipment(creature, out string resultsOfUse);
				useItemCallback(true, resultsOfUse, Author(), retVal);
				return null;
			}
		}



		//This is virtual in case you want to alter how the checks are done or provide some random text explaining why it fails so hard.
		public virtual bool CanWearWithUndergarments(Creature wearer, UpperGarmentBase upperGarment, LowerGarmentBase lowerGarment, out string whyNot)
		{
			bool allowsUpper = CanWearWithUpperGarment(wearer, upperGarment, out string _);
			bool allowsLower = CanWearWithLowerGarment(wearer, lowerGarment, out string _);

			if (allowsLower && allowsUpper)
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = GenericCantWearWithUnderGarmentText(upperGarment, lowerGarment, allowsUpper, allowsLower);
				return false;
			}
		}

		public virtual bool CanWearWithUpperGarment(Creature wearer, UpperGarmentBase upperGarment, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public virtual bool CanWearWithLowerGarment(Creature wearer, LowerGarmentBase lowerGarment, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//by default, body data is given priority, and if it fails the why not is just for that. then, if that passes, we check the garments and return them.
		//if you prefer more control, override this.
		public override bool CanUse(Creature creature, bool isInCombat, out string whyNot)
		{
			if (!CanWearWithBodyData(creature, out whyNot))
			{
				return false;
			}
			else
			{
				return CanWearWithUndergarments(creature, creature.upperGarment, creature.lowerGarment, out whyNot);
			}
		}

		//this is the basic format. feel free to override this if needed.

		public override string AboutItemWithStats(Creature target)
		{
			string defenseDifference = DefenseDifference(target.armor);


			return AboutItem() + GlobalStrings.NewParagraph() + "Type: " + armorType.AsText() + Environment.NewLine
				+ "Defense: " + PhysicalDefensiveRating(target) + defenseDifference + Environment.NewLine
				+ "Base value: " + monetaryValue;
		}

		//this is a generic text describing why you cannot equip an armor, based on the current upper and lower garments you have on.
		//This is modified to work with any cases, though it's highly likely you'll want more control than this if you have weird edge cases.
		protected string GenericCantWearWithUnderGarmentText(UpperGarmentBase upperGarment, LowerGarmentBase lowerGarment, bool allowsUpperGarment = false, bool allowsLowerGarment = false)
		{
			if (allowsUpperGarment && allowsLowerGarment)
			{
				return "";
			}
			bool both = !allowsLowerGarment && !allowsLowerGarment;

			StringBuilder sb = new StringBuilder();

			sb.Append("It would be awkward to put on the" + ItemDescription() + " when you're currently wearing ");

			if (!UpperGarmentBase.IsNullOrNothing(upperGarment) && !allowsUpperGarment)
			{
				sb.Append(upperGarment.ItemDescription(1,true));
			}

			if (!LowerGarmentBase.IsNullOrNothing(lowerGarment) && !allowsLowerGarment)
			{
				if (both)
				{
					sb.Append(" and ");
				}
				sb.Append(lowerGarment.ItemDescription(1, true));
			}

			sb.Append(". You should consider removing " +( both ? "them" : "it" ) + ". You put the " + ItemDescription() + " back into your inventory.");

			return sb.ToString();
		}

		//the following are provided as a sort of generic text describing why you cannot equip a lower or upper garment with this armor.
		//when can wear with upper/lower garment is overridden to return false, these can be used instead of writing your own, though if you have edge cases that forced you to
		//override the normal can use, you will likely want to not use these, either.
		protected string GenericArmorIncompatText(UpperGarmentBase upperGarment)
		{
			return "It would be awkward to wear this with the " + ItemDescription() + " you're already wearing. If you want to wear this, " +
				"you'll need to remove it first.";
		}
		protected string GenericArmorIncompatText(LowerGarmentBase lowerGarment)
		{
			return "It would be awkward to wear this with the " + ItemDescription() + " you're already wearing. If you want to wear this, " +
				"you'll need to remove it first.";
		}

		public static ArmorBase NOTHING { get; } = new Nothing();

		public bool isNothing => this is Nothing;

		public static bool IsNullOrNothing(ArmorBase armor)
		{
			return armor is null || armor.isNothing;
		}

		private sealed class Nothing : ArmorBase
		{
			public Nothing() : base(ArmorType.CLOTHING)
			{
			}

			public override double PhysicalDefensiveRating(Creature wearer) => 0;

			public override bool Equals(ArmorBase other)
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

			public override bool isNearlyNaked => true;

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

			protected override ArmorBase OnRemove(Creature wearer)
			{
				return null;
			}

			protected override string RemoveText(Creature wearer)
			{
				return "";
			}

			public override byte maxCapacityPerSlot => 1;
		}
	}

	public static class ArmorHelpers
	{
		public static string AsText(this ArmorType armorType)
		{
			if (!Enum.IsDefined(typeof(ArmorType), armorType))
			{
				return "Undefined";
			}
			else
			{
				switch (armorType)
				{
					case ArmorType.CLOTHING:
						return "Clothing";
					case ArmorType.LIGHT_ARMOR:
						return "Armor (Light)";
					case ArmorType.MEDIUM_ARMOR:
						return "Armor (Medium)";
					case ArmorType.HEAVY_ARMOR:
						return "Armor (Heavy)";
					default:
						throw new NotImplementedException("Someone added a new ArmorType, but didn't define its text. tell a dev to fix this.");
				}
			}
		}
	}
}
