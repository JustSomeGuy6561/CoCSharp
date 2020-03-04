//MessyOrgasms.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class MessyOrgasms : EndowmentPerkBase
	{
		public MessyOrgasms() : base(MessyOrgasmsStr, MessyOrgasmsBtn, MessyOrgasmsHint, MessyOrgasmsDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.bonusCumMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.5));
		}

		protected override void OnRemoval()
		{
		}

		//apprently this is lots of jizz despite being called messy orgasms.
		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.MALE);
		}
	}
}
