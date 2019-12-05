using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public class CreditCategoryBase
	{
		public readonly SimpleDescriptor CreditCategoryText;

		public readonly ReadOnlyCollection<SubCategory> subCategories;
		private readonly List<SubCategory> subcategorySource;
		public CreditCategoryBase(SimpleDescriptor categoryStr, SubCategory[] initialSubCategories)
		{
			CreditCategoryText = categoryStr ?? throw new ArgumentNullException(nameof(categoryStr));

			if (initialSubCategories is null)
			{
				initialSubCategories = new SubCategory[0];
			}

			subcategorySource = new List<SubCategory>(initialSubCategories);
			subCategories = new ReadOnlyCollection<SubCategory>(subcategorySource);


		}

		//public void AddSubCategory(SubCategory subCredit)
		//{
		//	subcategorySource.Add(subCredit);
		//}
	}
}
