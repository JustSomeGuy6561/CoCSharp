using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks
{
	//slight buff to endowment perks: they all now have a "permanent," albeit minor effect. For example, large cock/clit/breasts now grant a slight boost to growth rates
	//and/or set a minimum default size when new instances are created. others boost the minimum value for base stats. 

	public abstract class EndowmentPerkBase : PerkBase
	{
		public readonly SimpleDescriptor buttonText;
		public readonly SimpleDescriptor unlockHistoryText;

		public EndowmentPerkBase(SimpleDescriptor perkName, SimpleDescriptor buttonStr, SimpleDescriptor perkHintStr, SimpleDescriptor hasPerkText) : base(perkName, hasPerkText)
		{
			unlockHistoryText = perkHintStr;
			buttonText = buttonStr;
		}

		protected virtual bool Unlocked(Gender gender) => true;

		protected override bool KeepOnAscension => false;

		protected ExtraPerkModifiers extraModifiers => this.ExtraModifiers();

	}
}
