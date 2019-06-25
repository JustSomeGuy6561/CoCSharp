using System;
using System.Collections.Generic;
using System.Text;

namespace CoC
{
	//default operators not in c#. idk why. i'm tired of rewriting this shit.

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

	}

}
