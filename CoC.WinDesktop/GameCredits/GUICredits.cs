using CoC.Backend.GameCredits;
using System;
using System.Collections.Generic;
using System.Text;

using CreditCategoryBase = CoC.Backend.GameCredits.CreditCategoryBase;
using SubCategory = CoC.Backend.GameCredits.SubCategory;

namespace CoCWinDesktop.GameCredits
{
	public sealed partial class GUICredits : CreditCategoryBase
	{
		private static SubCategory[] backendCategories = new SubCategory[]
		{
			new OriginalGUI(),
			new CSharpGUI(),
		};

		internal GUICredits() : base(GuiCreditsStr, backendCategories)
		{
		}
	}

	public sealed partial class OriginalGUI : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"Fenoxo (Base Game)",
			"Dasutin (Background Images)",
			"Invader (Button Graphics, Font, and Other Hawtness)",
		};
		public OriginalGUI() : base(OriginalGuiStr, credits)
		{
		}
	}

	public sealed partial class CSharpGUI : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"JustSomeGuy",
			"Please for the love of God get someone else to clean this GUI up!",
		};

		public CSharpGUI() : base(CSharpGuiStr, credits)
		{
		}
	}
}
