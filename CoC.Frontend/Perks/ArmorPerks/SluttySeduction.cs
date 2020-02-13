using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Perks;
using CoC.Frontend.Items.Wearables;

namespace CoC.Frontend.Perks.ArmorPerks
{
	class SluttySeduction : ConditionalPerk
	{
		protected readonly byte strength;

		public SluttySeduction() : this(5)
		{
		}

		public SluttySeduction(byte intensity)
		{
			strength = intensity;
		}

		protected override void SetupActivationConditions()
		{
			//sourceCreature.equipmentChanged -= ArmorChanged;
			//sourceCreature.equipmentChanged += ArmorChanged;

			enabled = sourceCreature.armor is ISluttySeductionItem slutty && slutty.SluttySeductionModifier(sourceCreature) > 0;
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override void RemoveActivationConditions()
		{
			enabled = false;
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
			//sourceCreature.equipmentChanged -= ArmorChanged;
		}

		public override string Name()
		{
			throw new NotImplementedException();
		}

		public override string HasPerkText()
		{
			throw new NotImplementedException();
		}
	}
}
