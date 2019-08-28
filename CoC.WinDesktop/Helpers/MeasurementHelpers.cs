using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
	public enum SizeUnit { POINTS, PIXELS, EMS}

    public static class MeasurementHelpers
    {
		public const int MinEMFontSize = 20;
		public const int MaxEMFontSize = 48;

		public const double MinPointFontSize = MinEMFontSize / 2.0;
		public const double MaxPointFontSize = MaxEMFontSize / 2.0;
		//not even in pixel space but whatever. close enough. 
		public const double MinPixelFontSize = MinEMFontSize / 2.0 * 3;
		public const double MaxPixelFontSize = MaxEMFontSize / 2.0 * 3;

		public static double ConvertFromEms(int size, SizeUnit sizeUnits)
		{
			switch (sizeUnits)
			{
				case SizeUnit.PIXELS:
					return size * 96 / (72.0 * 2);
				case SizeUnit.POINTS:
					return size /2;
				case SizeUnit.EMS:
				default:
					return size;
			}
		}

		public static int ConvertToEms(double size, SizeUnit sizeUnit)
		{
			switch (sizeUnit)
			{
				case SizeUnit.PIXELS:
					return (int)Math.Round(size * 72 * 2 / 96);
				case SizeUnit.POINTS:
					return (int)Math.Round(size * 2);
				case SizeUnit.EMS:
				default:
					return (int)Math.Round(size);
			}
		}

	}
}
