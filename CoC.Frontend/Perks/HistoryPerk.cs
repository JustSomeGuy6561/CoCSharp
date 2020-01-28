//ForestEncounterStrings.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:36 PM
using CoC.Backend;
using CoC.Backend.Perks;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks.History;
using System;
using System.Collections.Generic;

namespace CoC.Frontend.Perks
{
	public abstract class HistoryPerkBase : PerkBase
	{
		public static List<Func<HistoryPerkBase>> historyPerks = new List<Func<HistoryPerkBase>>();
		static HistoryPerkBase()
		{
			AddHistoryHelper(() => new TestSubject());
			AddHistoryHelper(() => new AlchemicalMastery());
			AddHistoryHelper(() => new Fighter());
			AddHistoryHelper(() => new Fortune());
			AddHistoryHelper(() => new Healer());
			AddHistoryHelper(() => new Religious());
			AddHistoryHelper(() => new Scholar());
			AddHistoryHelper(() => new Slacker());
			AddHistoryHelper(() => new Slut());
			AddHistoryHelper(() => new Smith());
			AddHistoryHelper(() => new Whore());

		}

		private static void AddHistoryHelper<T>(Func<T> createHistoryFunction) where T : HistoryPerkBase
		{
			historyPerks.Add(createHistoryFunction);
		}

		private readonly SimpleDescriptor nameText;
		private readonly SimpleDescriptor perkText;

		public override string Name() => nameText();
		public override string HasPerkText() => perkText();

		private readonly SimpleDescriptor buttonText;
		private readonly SimpleDescriptor unlockHistoryText;

		public string ButtonText() => buttonText();
		public string UnlockHistoryText() => unlockHistoryText();

		public HistoryPerkBase(SimpleDescriptor perkName, SimpleDescriptor buttonStr, SimpleDescriptor perkHintStr, SimpleDescriptor hasPerkText)
		{
			nameText = perkName ?? throw new ArgumentNullException(nameof(perkName));
			perkText = hasPerkText ?? throw new ArgumentNullException(nameof(hasPerkText));

			unlockHistoryText = perkHintStr;
			buttonText = buttonStr;
		}

		protected override bool KeepOnAscension => false;

		protected bool hasExtraModifiers => sourceCreature is IExtendedCreature;
		protected ExtendedPerkModifiers extraModifiers => (sourceCreature as IExtendedCreature)?.extendedPerkModifiers;
	}
}
