using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public sealed partial class MiscellaneousCredits : CreditCategoryBase
	{
		private static SubCategory[] backendCategories = new SubCategory[]
		{
			new SpecialThanks(),
		};

		internal MiscellaneousCredits() : base(MiscCreditStr, backendCategories)
		{
		}
	}

	public sealed partial class SpecialThanks : SubCategory
	{
		private static string[] credits = new string[]
		{
			
		};

		public SpecialThanks() : base(SpecialThanksStr, credits)
		{
		}
	}

}
