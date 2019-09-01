using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public sealed partial class LocalizationCredits : CreditCategoryBase
	{
		private static SubCategory[] localizationCategories = new SubCategory[]
		{

		};

		internal LocalizationCredits() : base(LocalizationCreditStr, localizationCategories)
		{
		}
	}
}
