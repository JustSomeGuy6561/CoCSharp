using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoCWinDesktop.ModelView;

namespace CoCWinDesktop.CustomControls.ExtraItemModelViews
{
	public partial class MenuCreditsModelView
	{
		private static string CreditStr()
		{
			return "Credits:";
		}

		private static string CreditHelperStr()
		{
			return "The following people/handles (in no particular order) helped create this game.";
		}

		private static string DisclaimerStr()
		{
			return "...and possibly others - we're terrible at keeping the credits up to date. If you have been missed, please let us know!";
		}
	}
}
