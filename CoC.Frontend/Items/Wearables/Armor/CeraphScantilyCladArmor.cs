using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Items.Wearables.LowerGarment;

using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Wearables.Armor
{
	public sealed class CeraphScantilyCladArmor : ArmorBase
	{
		public CeraphScantilyCladArmor() : base(Short, Full, Desc)
		{
		}

		private static string Short()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Full()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Desc()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string EquipTheArmorDammit()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override int monetaryValue => 0;
		public override bool canSell => true;

		public override bool CanWearWithLowerGarment(LowerGarmentBase lowerGarment, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool CanWearWithUpperGarment(UpperGarmentBase upperGarment, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//swap armor. underclothes are irrelevant. then, remove the armor. return the old armor. Net result: ceraph armor is gone. old armor to inventory (can be reworn if items full). 
		//perk updated. 

		protected override ArmorBase EquipItem(Creature wearer, out string equipOutput)
		{
			var retVal = base.EquipItem(wearer, out equipOutput);
			wearer.RemoveArmorManual(); //and ignore the returned value. Simply let it go out of scope.
			return retVal;
		}

		protected override void OnEquip(Creature wearer)
		{
			if (!wearer.perks.HasPerk<CeraphFetishesPerk>())
			{
				wearer.perks.AddPerk(new CeraphFetishesPerk(2));
			}
			else
			{
				CeraphFetishesPerk ceraphPerk = wearer.perks.GetPerk<CeraphFetishesPerk>();
				if (!ceraphPerk.hasBondageFetish)
				{
					ceraphPerk.SetStacks(2);
				}
			}
		}

		protected override string EquipText(Creature wearer)
		{
			return EquipTheArmorDammit();
		}

		public override bool supportsBulgeArmor => false;
	}
}
