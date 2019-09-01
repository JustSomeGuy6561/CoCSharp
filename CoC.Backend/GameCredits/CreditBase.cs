using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public class SubCategory
	{
		public readonly SimpleDescriptor CreditCategoryText;
		public readonly ReadOnlyCollection<string> names;
		private readonly List<string> nameStore;

		public SubCategory(SimpleDescriptor categoryStr, string[] creditees)
		{
			CreditCategoryText = categoryStr ?? throw new ArgumentNullException(nameof(categoryStr));

			if (creditees is null)
			{
				creditees = new string[0];
			}

			nameStore = new List<string>(creditees);
			names = new ReadOnlyCollection<string>(nameStore);
		}

		//public void AddCredit(string creditText)
		//{
		//	nameStore.Add(creditText);
		//}
	}
}
