using System;
using System.Collections.Generic;
using System.Text;

namespace CoC
{
	//default operators not in c#. idk why. i'm tired of rewriting this shit.

	//NOTE: these also have the benefit of being overflow and underflow safe. 
	//by default, unsigned variables in c# wrap when going beyond their bounds.
	//now, i've made the CoC backend checked, 
	public static class Operators
	{
		public static byte subtract(this byte first, byte second)
		{
			if (second >= first)
			{
				return 0;
			}
			return (byte)(first - second);
		}

		public static byte add(this byte first, byte second)
		{
			int result = first + second;
			if (result >= byte.MaxValue)
			{
				return byte.MaxValue;
			}
			return (byte)result;
		}

		public static short subtract(this short first, short second)
		{
			int result = first - second;
			if (result <= short.MinValue)
			{
				return short.MinValue;
			}
			return (short)result;
		}

		public static short add(this short first, short second)
		{
			int result = first + second;
			if (result >= short.MaxValue)
			{
				return short.MaxValue;
			}
			return (short)result;
		}

		public static sbyte subtract(this sbyte first, sbyte second)
		{
			int result = first - second;
			if (result <= sbyte.MinValue)
			{
				return sbyte.MinValue;
			}
			return (sbyte)result;
		}

		public static sbyte add(this sbyte first, sbyte second)
		{
			int result = first + second;
			if (result >= sbyte.MaxValue)
			{
				return sbyte.MaxValue;
			}
			return (sbyte)result;
		}

		public static ushort subtract(this ushort first, ushort second)
		{
			if (second >= first)
			{
				return 0;
			}
			return (ushort)(first - second);
		}

		public static ushort add(this ushort first, ushort second)
		{
			int result = first + second;
			if (result >= ushort.MaxValue)
			{
				return ushort.MaxValue;
			}
			return (ushort)result;
		}

		public static ushort mult(this byte first, byte second)
		{
			int val = first * second;
			return (ushort)val;
		}

		public static uint mult(this ushort first, ushort second)
		{
			return (uint)(first * second);
		}

		public static uint add(this uint first, uint second)
		{
			unchecked
			{
				uint retVal = first + second;
				if (retVal < first) //if we overflowed.
				{
					return uint.MaxValue;
				}
				return retVal;
			}
		}

		public static uint subtract(this uint first, uint second)
		{
			if (second >= first)
			{
				return 0;
			}
			return first - second;
		}

		public static ulong add(this ulong first, ulong second)
		{
			unchecked
			{
				ulong retVal = first + second;
				if (retVal < first) //if we overflowed.
				{
					return ulong.MaxValue;
				}
				return retVal;
			}
		}

		public static ulong subtract(this ulong first, ulong second)
		{
			if (second >= first)
			{
				return 0;
			}
			return first - second;
		}

		public static ushort addIn(ref this ushort val, ushort amount) => val = add(val, amount);
		public static ushort subtractOff(ref this ushort val, ushort other) => val = subtract(val, other);
		public static uint addIn(ref this uint val, uint amount) => val = add(val, amount);
		public static uint subtractOff(ref this uint val, uint other) => val = subtract(val, other);
		public static ulong addIn(ref this ulong val, ulong amount) => val = add(val, amount);
		public static ulong subtractOff(ref this ulong val, ulong other) => val = subtract(val, other);

		public static byte addIn(ref this byte val, byte amount) => val = add(val, amount);
		public static byte subtractOff(ref this byte val, byte other) => val = subtract(val, other);



		//public static int CompareTo<T>(this T first, T second) where T : Enum
		//{
		//	Type underlying = Enum.GetUnderlyingType(typeof(T));
		//	if (underlying == typeof(ulong))
		//	{
		//		ulong f = Convert.ToUInt64(first);
		//		ulong s = Convert.ToUInt64(second);
		//		return f > s ? 1 : f < s ? -1 : 0;

		//	}
		//	else if (underlying == typeof(uint))
		//	{
		//		uint f = Convert.ToUInt32(first);
		//		uint s = Convert.ToUInt32(second);
		//		return f > s ? 1 : f < s ? -1 : 0;
		//	}
		//	else if (underlying == typeof(long))
		//	{
		//		long f = Convert.ToInt64(first);
		//		long s = Convert.ToInt64(second);
		//		return f > s ? 1 : f < s ? -1 : 0;
		//	}
		//	else //the rest can be converted to ints
		//	{
		//		int f = Convert.ToInt32(first);
		//		int s = Convert.ToInt32(second);
		//		return f - s;
		//	}

		//}
	}
}