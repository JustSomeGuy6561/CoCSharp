using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoCWinDesktop
{
	public class Settings: INotifyPropertyChanged
	{
		private static readonly string[] backgrounds = { "resources\background0.jpg", "resources\background1.png", "resrouces\background2.png", "resrouces\background3.png", "resrouces\background4.png" };

		public event PropertyChangedEventHandler PropertyChanged;

		public static int currBackground { get; private set; } = 0;
		public static BitmapImage UpdateBackgroundImage(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			else if (index >= backgrounds.Length)
			{
				index = backgrounds.Length - 1;
			}
			currBackground = index;
			return new BitmapImage(new Uri(backgrounds[index], UriKind.Relative));
		}

		public static BitmapImage GetBackground()
		{
			return new BitmapImage(new Uri(backgrounds[currBackground], UriKind.Relative));
		}
	}
}
