using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public sealed partial class BackendCredits : CreditCategoryBase
	{
		private static SubCategory[] backendCategories = new SubCategory[]
		{
			new BaseGame(),
			new Framework(),
			new CSharpEngine(),
			//new FrameworkRefactoring(),
		};

		internal BackendCredits() : base(BackendCreditStr, backendCategories)
		{
		}
	}

	public sealed partial class BaseGame : SubCategory
	{
		private static string[] baseGameCredits = new string[]
		{
			"Fenoxo",
		};
		public BaseGame() : base(BaseGameStr, baseGameCredits)
		{
		}
	}

	public sealed partial class Framework : SubCategory
	{
		private static string[] frameworkCredits = new string[]
		{
			"Fenoxo (source)",
			"Kitteh6660 (mod source)",
			"Stradler76 (body parts, refactoring)",
			"JustSomeGuy (C# rewrite, refactoring)",

		};
		public Framework() : base(FrameworkStr, frameworkCredits)
		{
		}
	}

	public sealed partial class CSharpEngine : SubCategory
	{
		private static string[] engineCredits = new string[]
		{
			"JustSomeGuy",

		};
		public CSharpEngine() : base(CSharpEngineStr, engineCredits)
		{
		}
	}

	//any additional framework stuff - save system, combat system, additional credits for backend.

	public sealed partial class FrameworkRefactoring : SubCategory
	{
		private static string[] refactoringCredits = new string[]
		{

		};

		public FrameworkRefactoring() : base(RefactoringStr, refactoringCredits)
		{
		}
	}
}
