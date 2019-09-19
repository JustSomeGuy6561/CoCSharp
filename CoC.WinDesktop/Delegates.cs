using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop
{
	//This is purely syntactic sugar, but it exists to denote that the current function callback
	//is expected to work without having to worry about having its last location correctly set. 
	public delegate void SafeAction();

	public delegate bool EnabledOrDisabledWithTollTipNullSldr(int? nullableOption, out string tooltip);
	public delegate bool EnabledOrDisabledWithToolTipSldr(int option, out string tooltip);

	public delegate bool EnabledOrDisabledWithToolTipNullBtn(bool? nullableOption, out string tooltip);
	public delegate bool EnabledOrDisabledWithToolTipBtn(bool nullableOption, out string tooltip);

	public delegate bool EnabledOrDisabledWithToolTip(out string tooltip);
}
