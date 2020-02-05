using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks.Endowment;
using System;
using System.Collections.Generic;

namespace CoC.Frontend.Perks
{
	//slight buff to endowment perks: they all now have a "permanent," albeit minor effect. For example, large cock/clit/breasts now grant a slight boost to growth rates
	//and/or set a minimum default size when new instances are created. others boost the minimum value for base stats.

	public abstract class EndowmentPerkBase : StandardPerk
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



		private readonly SimpleDescriptor buttonText;
		private readonly SimpleDescriptor unlockEndowmentText;

		private readonly SimpleDescriptor nameText;
		private readonly SimpleDescriptor perkText;

		public string ButtonText() => buttonText();
		public string UnlockEndowmentText() => unlockEndowmentText();

		public override string HasPerkText() => perkText();

		public override string Name() => nameText();

		public EndowmentPerkBase(SimpleDescriptor perkName, SimpleDescriptor buttonStr, SimpleDescriptor perkHintStr, SimpleDescriptor hasPerkText) : base()
		{
			nameText = perkName ?? throw new ArgumentNullException(nameof(perkName));
			perkText = hasPerkText ?? throw new ArgumentNullException(nameof(hasPerkText));
			unlockEndowmentText = perkHintStr ?? throw new ArgumentNullException(nameof(perkHintStr));
			buttonText = buttonStr ?? throw new ArgumentNullException(nameof(buttonStr));
		}

		internal bool IsUnlocked(Gender gender) => Unlocked(gender);

		protected virtual bool Unlocked(Gender gender) => true;

		protected override bool keepOnAscension => false;

		protected bool hasExtraModifiers => sourceCreature is IExtendedCreature;
		protected ExtendedPerkModifiers extraModifiers => (sourceCreature as IExtendedCreature).extendedPerkModifiers;

	}
}
