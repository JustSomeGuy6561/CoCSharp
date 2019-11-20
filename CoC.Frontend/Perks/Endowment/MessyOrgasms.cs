//MessyOrgasms.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.BodyParts;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class MessyOrgasms : EndowmentPerkBase
	{
		public MessyOrgasms() : base(MessyOrgasmsStr, MessyOrgasmsBtn, MessyOrgasmsHint, MessyOrgasmsDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.BonusCumStacked += 0.5f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.BonusCumStacked -= 0.5f;
		}

		//apprently this is lots of jizz despite being called messy orgasms. 
		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.MALE);
		}
	}
}
