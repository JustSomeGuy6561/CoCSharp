//Enums.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:06 PM
using System;

namespace CoC.Backend.Tools
{
	public static class EnumHelper
	{
		//Flags attribute lets you use bitwise operations.
		public static int Length<T>() where T : Enum
		{
			return Enum.GetNames(typeof(T)).Length;
		}

		public static T ByteEnumAdd<T>(this T val, byte amount) where T : Enum
		{
			byte amt = Convert.ToByte(val);
			amt.addIn(amount);
			return (T)Enum.Parse(typeof(T), amt.ToString());
		}

		public static T ByteEnumSubtract<T>(this T val, byte amount) where T : Enum
		{
			byte amt = Convert.ToByte(val);
			amt.subtractOff(amount);
			return (T)Enum.Parse(typeof(T), amt.ToString());
		}

		public static T ByteEnumDelta<T>(this T val, sbyte delta) where T : Enum
		{
			byte amt = Convert.ToByte(val);
			amt.offset(delta);
			return (T)Enum.Parse(typeof(T), amt.ToString());
		}

		public static T CheckValid<T>(this T source, T newValue) where T : Enum
		{
			if (Enum.IsDefined(typeof(T), newValue))
			{
				return newValue;
			}
			return source;
		}

		public static T Min<T>(params T[] values) where T: Enum
		{
			if (values is null || values.Length == 0)
			{
				throw new ArgumentNullException(nameof(values));
			}
			var minVal = values[0];

			foreach (var val in values)
			{
				if (val.CompareTo(minVal) < 0)
				{
					minVal = val;
				}
			}
			return minVal;
		}

		public static T Min<T>(T value1, T value2) where T : Enum
		{
			return value1.CompareTo(value2) < 0 ? value1 : value2;
		}

		public static T Max<T>(params T[] values) where T : Enum
		{
			if (values is null || values.Length == 0)
			{
				throw new ArgumentNullException(nameof(values));
			}
			var maxVal = values[0];

			foreach (var val in values)
			{
				if (val.CompareTo(maxVal) > 0)
				{
					maxVal = val;
				}
			}
			return maxVal;
		}

		public static T Max<T>(T value1, T value2) where T : Enum
		{
			return value1.CompareTo(value2) > 0 ? value1 : value2;
		}
	}
}
