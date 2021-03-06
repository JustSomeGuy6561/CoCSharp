﻿//Enums.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:06 PM
using System;
using System.Collections.Generic;
using System.Linq;

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

		public static T ByteEnumOffset<T>(this T val, sbyte offset) where T : Enum
		{
			byte amt = Convert.ToByte(val);
			amt.offset(offset);
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

		public static T Min<T>(params T[] values) where T : Enum
		{
			if (values is null || values.Length == 0)
			{
				throw new ArgumentNullException(nameof(values));
			}
			T minVal = values[0];

			foreach (T val in values)
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

		public static T Min<T>(IEnumerable<T> collection) where T : Enum
		{
			Type type = Enum.GetUnderlyingType(typeof(T));
			if (type == typeof(byte))
			{
				return collection.MaxItem(x => Convert.ToByte(x));
			}
			else if (type == typeof(sbyte))
			{
				return collection.MaxItem(x => Convert.ToSByte(x));
			}
			else if (type == typeof(short))
			{
				return collection.MaxItem(x => Convert.ToInt16(x));
			}
			else if (type == typeof(ushort))
			{
				return collection.MaxItem(x => Convert.ToUInt16(x));
			}
			else if (type == typeof(int))
			{
				return collection.MaxItem(x => Convert.ToInt32(x));
			}
			else if (type == typeof(uint))
			{
				return collection.MaxItem(x => Convert.ToUInt32(x));
			}
			else if (type == typeof(long))
			{
				return collection.MaxItem(x => Convert.ToInt64(x));
			}
			else if (type == typeof(ulong))
			{
				return collection.MaxItem(x => Convert.ToUInt64(x));
			}
			else
			{
				throw new NotImplementedException("C# implemented a new underlying type to enums since this was implemented. This will likely never happen");
			}
		}


		public static T Max<T>(params T[] values) where T : Enum
		{
			if (values is null || values.Length == 0)
			{
				throw new ArgumentNullException(nameof(values));
			}
			T maxVal = values[0];

			foreach (T val in values)
			{
				if (val.CompareTo(maxVal) > 0)
				{
					maxVal = val;
				}
			}
			return maxVal;
		}

		public static T Max<T>(IEnumerable<T> collection) where T : Enum
		{
			Type type = Enum.GetUnderlyingType(typeof(T));
			if (type == typeof(byte))
			{
				return collection.MaxItem(x => Convert.ToByte(x));
			}
			else if (type == typeof(sbyte))
			{
				return collection.MaxItem(x => Convert.ToSByte(x));
			}
			else if (type == typeof(short))
			{
				return collection.MaxItem(x => Convert.ToInt16(x));
			}
			else if (type == typeof(ushort))
			{
				return collection.MaxItem(x => Convert.ToUInt16(x));
			}
			else if (type == typeof(int))
			{
				return collection.MaxItem(x => Convert.ToInt32(x));
			}
			else if (type == typeof(uint))
			{
				return collection.MaxItem(x => Convert.ToUInt32(x));
			}
			else if (type == typeof(long))
			{
				return collection.MaxItem(x => Convert.ToInt64(x));
			}
			else if (type == typeof(ulong))
			{
				return collection.MaxItem(x => Convert.ToUInt64(x));
			}
			else
			{
				throw new NotImplementedException("C# implemented a new underlying type to enums since this was implemented. This will likely never happen");
			}
		}

		public static T Max<T>(T value1, T value2) where T : Enum
		{
			return value1.CompareTo(value2) > 0 ? value1 : value2;
		}
	}
}
