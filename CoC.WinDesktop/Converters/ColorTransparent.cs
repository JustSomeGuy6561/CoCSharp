using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CoC.WinDesktop.Converters
{
	public sealed class ColorTransparent : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//if (targetType == typeof(bool))
			//{
				if (value is System.Drawing.Color dc)
				{
					return dc == System.Drawing.Color.Transparent;
				}
				else if (value is System.Windows.Media.Color mc)
				{
					return mc.Equals(System.Windows.Media.Colors.Transparent);
				}
				else if (value is System.Windows.Media.SolidColorBrush sb)
				{
					return sb.Color.Equals(System.Windows.Media.Colors.Transparent);
				}
			//}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
