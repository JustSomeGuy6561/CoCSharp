using System;
using System.Collections.Generic;
using System.Text;

using CreditCategoryBase = CoC.Backend.GameCredits.CreditCategoryBase;
using SubCategory = CoC.Backend.GameCredits.SubCategory;

namespace CoCWinDesktop.GameCredits
{
	public sealed partial class GUICredits : CreditCategoryBase
	{
		private static string GuiCreditsStr()
		{
			return "Graphics/User Interface (GUI)";
		}
	}

	public sealed partial class OriginalGUI : SubCategory
	{
		private static string OriginalGuiStr()
		{
			return "Original Graphics and UI";
		}
	}

	public sealed partial class CSharpGUI : SubCategory
	{
		private static string CSharpGuiStr()
		{
			return "C# Graphics and UI";
		}
	}
}
