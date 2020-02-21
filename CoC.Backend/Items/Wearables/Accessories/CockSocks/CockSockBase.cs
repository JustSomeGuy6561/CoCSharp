using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using System.Linq;
using CoC.Backend.BodyParts;
using CoC.Backend.UI;

namespace CoC.Backend.Items.Wearables.Accessories.CockSocks
{
	public abstract class CockSockBase : WearableItemBase<CockSockBase>
	{
		protected CockSockBase() : base() { }

		private protected override DisplayBase AttemptToUseSafe(Creature target, UseItemCallbackSafe<CockSockBase> postItemUseCallbackSafe)
		{
			return AttemptToUse(target, postItemUseCallbackSafe);
		}

		//you can now give this a menu if you really want. for a valid example, see bimbo skirt.
		protected virtual DisplayBase AttemptToUse(Creature creature, UseItemCallbackSafe<CockSockBase> useItemCallback)
		{
			if (!CanUse(creature, false, out string whyNot))
			{
				useItemCallback(false, whyNot, Author(), this);
				return null;
			}
			else
			{
				CockSockBase retVal = ChangeEquipment(creature, out string resultsOfUse);
				useItemCallback(true, resultsOfUse, Author(), retVal);
				return null;
			}
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

		private protected override CockSockBase UpdateCreatureEquipmentInternal(Creature target)
		{
			if (!target.hasCock)
			{
				return this;
			}
			else
			{
				return (target.cocks.FirstOrDefault(x => x.cockSock is null) ?? target.cocks.FirstOrDefault()).ChangeCockSock(this);
			}
		}

		//by default, cock socks simply disappear when removed. that's fine, i guess.
		protected override CockSockBase OnRemove(Creature wearer)
		{
			return null;
		}

		protected internal abstract string PlayerText(PlayerBase player, CockData attachedCock);

		public abstract string ShortDescription();

		public abstract string LongDescription(CockData attachedCock);

		//override on remove to remove any perks.

		protected internal virtual double cockGrowthMultiplier => 1.0f;
		protected internal virtual double cockShrinkMultiplier => 1.0f;
	}
}
