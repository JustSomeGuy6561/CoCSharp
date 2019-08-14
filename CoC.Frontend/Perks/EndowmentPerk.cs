using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;
using CoC.Frontend.Perks.Endowment;
using System;
using System.Collections.Generic;

namespace CoC.Frontend.Perks
{
	//slight buff to endowment perks: they all now have a "permanent," albeit minor effect. For example, large cock/clit/breasts now grant a slight boost to growth rates
	//and/or set a minimum default size when new instances are created. others boost the minimum value for base stats. 

	public abstract class EndowmentPerkBase : PerkBase
	{
		public static List<Func<EndowmentPerkBase>> endowmentPerks = new List<Func<EndowmentPerkBase>>();

		static EndowmentPerkBase()
		{
			AddEndowmentHelper(() => new Strong());
			AddEndowmentHelper(() => new Tough());
			AddEndowmentHelper(() => new Fast());
			AddEndowmentHelper(() => new Smart());
			AddEndowmentHelper(() => new Lusty());
			AddEndowmentHelper(() => new Sensitive());
			AddEndowmentHelper(() => new Pervert());

			AddEndowmentHelper(() => new Trap()); //not available for herms
			AddEndowmentHelper(() => new WetAnus()); //only available for genderless.
			AddEndowmentHelper(() => new BigCock());
			AddEndowmentHelper(() => new MessyOrgasms());
							   
			AddEndowmentHelper(() => new BigTits());
			AddEndowmentHelper(() => new BigClit());
			AddEndowmentHelper(() => new Fertile());
			AddEndowmentHelper(() => new WetPussy());
		}

		private static void AddEndowmentHelper<T>(Func<T> createEndowmentFunction) where T : EndowmentPerkBase
		{
			endowmentPerks.Add(createEndowmentFunction);
		}



		public readonly SimpleDescriptor buttonText;
		public readonly SimpleDescriptor unlockEndowmentText;

		public EndowmentPerkBase(SimpleDescriptor perkName, SimpleDescriptor buttonStr, SimpleDescriptor perkHintStr, SimpleDescriptor hasPerkText) : base(perkName, hasPerkText)
		{
			unlockEndowmentText = perkHintStr;
			buttonText = buttonStr;
		}

		internal bool IsUnlocked(Gender gender) => Unlocked(gender);

		protected virtual bool Unlocked(Gender gender) => true;

		protected override bool KeepOnAscension => false;

		protected ExtraPerkModifiers extraModifiers => this.ExtraModifiers();

	}
}
