using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using System.Linq;
using CoC.Backend.BodyParts;

namespace CoC.Backend.Items.Wearables.Accessories.CockSocks
{
	public abstract class CockSockBase : WearableItemBase<CockSockBase>
	{
		protected CockSockBase(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			bool anyCocksAvailable = creature.cocks.Any(x => x.cockSock == null);
			if (!anyCocksAvailable)
			{
				whyNot = creature.cocks.Count == 0 ? "You can't wear a cock-sock if you don't have any cocks. " : "You are already wearing cock-socks on all of your cocks.";
				return false;
			}
			else
			{
				whyNot = "";
				return true;
			}
		}

		protected override CockSockBase EquipItem(Creature wearer, out string equipOutput)
		{
			equipOutput = OnEquip();
			return null;
		}

		protected abstract string OnEquip();

		protected internal abstract string PlayerText(PlayerBase player, CockData attachedCock);

		public abstract string ShortDescription();

		public abstract string LongDescription(CockData attachedCock);

		//override on remove to remove any perks.

		protected internal virtual float cockGrowthMultiplier => 1.0f;
		protected internal virtual float cockShrinkMultiplier => 1.0f;


	}
}
