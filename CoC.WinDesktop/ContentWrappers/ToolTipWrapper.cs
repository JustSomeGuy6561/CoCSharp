using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.ContentWrappers
{
	public sealed class ToolTipWrapper
	{
		public string Header { get; }
		public string Hint { get; }

		public ToolTipWrapper(string headerText, string hintText)
		{
			Header = headerText;
			Hint = hintText;
		}
	}
}
