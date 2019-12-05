using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public class SubCategory
	{
		public readonly SimpleDescriptor CreditCategoryText;
		public readonly ReadOnlyCollection<Creditor> creditList;
		private readonly List<Creditor> nameStore;

		public SubCategory(SimpleDescriptor categoryStr, Creditor[] creditees)
		{
			CreditCategoryText = categoryStr ?? throw new ArgumentNullException(nameof(categoryStr));

			if (creditees is null)
			{
				creditees = new Creditor[0];
			}

			nameStore = new List<Creditor>(creditees);
			creditList = new ReadOnlyCollection<Creditor>(nameStore);
		}



		//public void AddCredit(string creditText)
		//{
		//	nameStore.Add(creditText);
		//}
	}

	public class Creditor
	{
		public readonly string name;
		public readonly Uri url; //if a url is required, this provides the option to do so.

		public static implicit operator Creditor(string text)
		{
			return new Creditor(text, null);
		}

		public Creditor(string creditee) : this(creditee, null)
		{

		}

		public Creditor(string creditee, string crediteeUrl)
		{
			name = creditee ?? throw new ArgumentNullException(nameof(creditee));
			if (string.IsNullOrWhiteSpace(crediteeUrl)) crediteeUrl = null;
			url = constructFromString(crediteeUrl);//can be null.
		}

		private static Uri constructFromString(string target)
		{
			if (Uri.TryCreate(target, UriKind.Absolute, out Uri uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
			{
				return uriResult;
			}
			return null;
		}
	}
}
