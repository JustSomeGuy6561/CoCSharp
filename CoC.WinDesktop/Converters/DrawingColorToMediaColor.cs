using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CoC.WinDesktop.Converters
{
	public sealed class DrawingColorToMediaColorIgnoreAlpha : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
			{
				var c = System.Drawing.Color.FromName((string)value);
				if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
				{
					return new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
				}
				else if (targetType == typeof(Color))
				{
					return Color.FromRgb(c.R, c.G, c.B);
				}
			}
			if (value is System.Drawing.Color drawingColor)
			{
				if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
				{
					return new SolidColorBrush(Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B));
				}
				else if (targetType == typeof(Color))
				{
					return Color.FromRgb(drawingColor.R, drawingColor.G, drawingColor.B);
				}
			}
			else if (value is null && targetType == typeof(Color?))
			{
				return null;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Color mediaColor)
			{
				return System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
			}
			else if (value is SolidColorBrush solidColor)
			{
				Color color = solidColor.Color;
				return System.Drawing.Color.FromArgb(color.R, color.G, color.B);
			}

			return value;
		}
	}
}
