using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Items.Wearables.Shield
{
	public enum ShieldType { LIGHT, MEDIUM, HEAVY }
	public abstract class ShieldBase : WearableItemBase<ShieldBase>
	{
		protected readonly ShieldType shieldType;

		private protected override ShieldBase UpdateCreatureEquipmentInternal(Creature target)
		{
			throw new NotImplementedException();
		}

		protected ShieldBase(ShieldType type)
		{
			shieldType = type;
		}

		//by default they don't raise defense, they just add a block rate.
		public override double PhysicalDefensiveRating(Creature wearer) => 0;

		//number from 0 to 100. we'll divide it by 100 later. can be affected by wearer stats like lust, hp, etc.
		public abstract double BlockRate(Creature wearer);

		public override string AboutItemWithStats(Creature wearer)
		{
			throw new NotImplementedException();
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			if (!CanWearWithBodyData(target, out whyNot))
			{
				return false;
			}
			else if (target.weapon?.CanUseWithShield(target, this, out whyNot) == false)
			{
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}
		public override byte maxCapacityPerSlot => 1;


		public static ShieldBase NOTHING { get; } = new Nothing();

		public bool isNothing => this is Nothing;

		public static bool IsNullOrNothing(ShieldBase shield)
		{
			return shield is null || shield.isNothing;
		}

		private class Nothing : ShieldBase
		{
			public Nothing() : base(ShieldType.LIGHT)
			{
			}

			public override double BlockRate(Creature wearer) => 0;

			public override bool Equals(ShieldBase other) => other is Nothing;

			public override string AbbreviatedName() =>"";

			public override string ItemName() => "";
			public override string ItemDescription(byte count = 1, bool displayCount = false) => "";
			public override string AboutItem() => "";

			protected override int monetaryValue => 0;

			protected override ShieldBase OnRemove(Creature wearer) => null;

			protected override string EquipText(Creature wearer) => "";

			protected override string RemoveText(Creature wearer) => "";

			public override string AboutItemWithStats(Creature wearer) => "";

			public override bool canBuy => false;
			public override bool canSell => false;
		}

	}
}
