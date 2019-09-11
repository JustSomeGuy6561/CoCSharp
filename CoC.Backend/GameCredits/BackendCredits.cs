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
		private static Creditor[] credits = new Creditor[]
		{
			"Fenoxo",
		};
		public BaseGame() : base(BaseGameStr, credits)
		{
		}
	}

	public sealed partial class Framework : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"Fenoxo (source)",
			"Kitteh6660 (mod source)",
			"Stradler76 (body parts, refactoring)",
			"JustSomeGuy (C# rewrite, refactoring)",

		};
		public Framework() : base(FrameworkStr, credits)
		{
		}
	}

	public sealed partial class CSharpEngine : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"JustSomeGuy",

		};
		public CSharpEngine() : base(CSharpEngineStr, credits)
		{
		}
	}

	//any additional framework stuff - save system, combat system, additional credits for backend.

	public sealed partial class FrameworkRefactoring : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{

		};

		public FrameworkRefactoring() : base(RefactoringStr, credits)
		{
		}
	}
}
