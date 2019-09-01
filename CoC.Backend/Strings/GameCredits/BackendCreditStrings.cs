using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.GameCredits
{
	public partial class BackendCredits
	{
		private static string BackendCreditStr()
		{
			return "Backend (Framework, Engine, Etc)";
		}
	}

	public partial class Framework
	{
		private static string FrameworkStr()
		{
			return "Framework/Source Code";
		}
	}

	public partial class BaseGame
	{
		private static string BaseGameStr()
		{
			return "Base Game";
		}
	}

	public partial class CSharpEngine
	{
		private static string CSharpEngineStr()
		{
			return "C# Engine";
		}
	}

	public partial class FrameworkRefactoring
	{
		private static string RefactoringStr()
		{
			return "C# Refactoring and Code Cleanup";
		}
	}
}
