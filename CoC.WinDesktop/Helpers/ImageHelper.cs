using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CoCWinDesktop.Helpers
{
	public static class ImageHelper
	{
		internal static BitmapImage GetImage(string imageUriString)
		{
			BitmapImage retVal = null;
			if (Uri.TryCreate(imageUriString, UriKind.RelativeOrAbsolute, out Uri file))
			{
				try
				{
					retVal = new BitmapImage(file);
				}
				catch (Exception)
				{
					retVal = null;
				}
			}
			return retVal;
		}
	}
}
