using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
    public static class UrlHelper
    {
		public static void OpenURL(string urlString)
		{
			System.Diagnostics.Process.Start(urlString);
		}
    }
}
