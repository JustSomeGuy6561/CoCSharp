using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CoC.Backend.GameCredits;

namespace CoC.Backend.Engine
{
	public static class CreditManager
	{
		public static readonly ReadOnlyCollection<CreditCategoryBase> categories;
		private static readonly List<CreditCategoryBase> categoryStore;

		static CreditManager()
		{
			categoryStore = new List<CreditCategoryBase>();
			categories = new ReadOnlyCollection<CreditCategoryBase>(categoryStore);
		}

		public static void AddCreditCategory(CreditCategoryBase category)
		{
			categoryStore.Add(category);
		}
	}
}
