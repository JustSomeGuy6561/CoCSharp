using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public sealed partial class MiscellaneousCredits : CreditCategoryBase
	{
		private static SubCategory[] backendCategories = new SubCategory[]
		{
			new ExternalTools(),
			new SpecialThanks(),
		};

		internal MiscellaneousCredits() : base(MiscCreditStr, backendCategories)
		{
		}
	}

	public sealed partial class ExternalTools : SubCategory
	{
		//Weak Events would not be necessary here if Microsoft would implement them into CLR, not just Windows.
		private static Creditor[] credits = new Creditor[]
		{
			new Creditor("Thomas Levesque - Weak Events.", "https://github.com/thomaslevesque/WeakEvent")
		};

		public ExternalTools() : base(ExternalToolsStr, credits) { }
	}

	public sealed partial class SpecialThanks : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{

		};

		public SpecialThanks() : base(SpecialThanksStr, credits)
		{
		}
	}

}
