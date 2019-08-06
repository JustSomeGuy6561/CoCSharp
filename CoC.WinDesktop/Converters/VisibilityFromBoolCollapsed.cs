using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CoCWinDesktop.Converters
{
	public sealed class VisibilityFromBoolCollapsed : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType == typeof(Visibility))
			{
				if (value is bool && (bool)value)
				{
					return Visibility.Visible;
				}
				else
				{
					return Visibility.Collapsed;
				}
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType == typeof(bool) && value is Visibility visibility)
			{
				return visibility == Visibility.Visible;
			}
			return value;
		}
	}
}
