using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.Helpers
{
    public static class UrlHelper
    {
		public static void OpenURL(string urlString)
		{
			System.Diagnostics.Process.Start(urlString);
		}
    }
}
