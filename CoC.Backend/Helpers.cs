//Helpers.cs
//Description:
//Author: JustSomeGuy
//4/9/2019, 1:26 AM

using System.Numerics;

namespace CoC
{
	//default operators not in c#. idk why. i'm tired of rewriting this shit.

	//NOTE: these also have the benefit of being overflow and underflow safe. 
	//by default, unsigned variables in c# wrap when going beyond their bounds.
	//the backend is checked in debug mode. try to bulletproof the backend as much as possible - it's not a front end dev's job to correct the backend shit
	//just as much as it's not your (backend dev) job to correct theirs. If they break something in the frontend, that's on them for not running tests. if it's the backend's fault, everyone's fucked.
	//even if the only way it breaks is if they are trying to do so (like adding byte.MaxValue when a number 1-5 is expected).

	public static class Operators
	{
		public static byte add(this byte first, byte second)
		{
			int result = first + second;
			if (result >= byte.MaxValue)
			{
				return byte.MaxValue;
			}
			return (byte)result;
		}

		public static byte subtract(this byte first, byte second)
		{
			if (second >= first)
			{
				return 0;
			}
			return (byte)(first - second);
		}
		//there's probably some unchecked magic i can do here. not gonna even try.
		public static short diff(this byte first, byte second)
		{
			return (short)(first - second);
		}
		public static byte delta(this byte first, sbyte second)
		{
			if (second >= 0)
			{
				return first.add((byte)second);
			}
			else
			{
				second *= -1;
				return first.subtract((byte)second);
			}
		}

		public static ushort mult(this byte first, byte second)
		{
			int val = first * second;
			return (ushort)val;
		}

		public static byte div(this byte first, byte second) => (byte)(first / second);


		public static sbyte add(this sbyte first, sbyte second)
		{
			int result = first + second;
			if (result >= sbyte.MaxValue)
			{
				return sbyte.MaxValue;
			}
			return (sbyte)result;
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

		public static short mult(this sbyte first, sbyte second) => (short)(first * second);

		public static sbyte div(this sbyte first, sbyte second) => (sbyte)(first / second);


		public static ushort add(this ushort first, ushort second)
		{
			int result = first + second;
			if (result >= ushort.MaxValue)
			{
				return ushort.MaxValue;
			}
			return (ushort)result;
		}

		public static ushort subtract(this ushort first, ushort second)
		{
			if (second >= first)
			{
				return 0;
			}
			return (ushort)(first - second);
		}

		public static ushort delta(this ushort first, short second)
		{
			if (second >= 0)
			{
				return first.add((ushort)second);
			}
			else
			{
				second *= -1;
				return first.subtract((ushort)second);
			}
		}

		public static uint mult(this ushort first, ushort second)
		{
			return (uint)first * (uint)second;
		}

		public static ushort div(this ushort first, ushort second) => (ushort)(first / second);


		public static short add(this short first, short second)
		{
			int result = first + second;
			if (result >= short.MaxValue)
			{
				return short.MaxValue;
			}
			return (short)result;
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
		//don't need a mult, as short * short is actually parsed to int * int => int. imo int * int => int is unsafe but w/e.
		public static int mult(this short first, short second) => first * second;

		public static short div(this short first, short second) => (short)(first / second);


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

		public static uint delta(this uint first, int second)
		{
			if (second >= 0)
			{
				return first.add((uint)second);
			}
			else
			{
				second *= -1;
				return first.subtract((uint)second);
			}
		}
		//uint * uint is defined, but returns a uint. This is safer, i guess, though largely unneeded in most programming.
		public static ulong mult(this uint first, uint second) => (ulong)first * (ulong)second;


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

		public static ulong delta(this ulong first, long second)
		{
			if (second >= 0)
			{
				return first.add((ulong)second);
			}
			else
			{
				second *= -1;
				return first.subtract((ulong)second);
			}
		}

		//div defined. Mult requires BigInteger

		public static BigInteger mult(this ulong first, ulong second) => new BigInteger(first) * new BigInteger(second); //i hate you.

		public static BigInteger mult(this long first, long second) => new BigInteger(first) * new BigInteger(second);

		public static long mult(this int first, int second) => (long)first * (long)second;


		public static byte addIn(ref this byte val, byte amount) => val = add(val, amount);
		public static byte subtractOff(ref this byte val, byte other) => val = subtract(val, other);

		public static ushort addIn(ref this ushort val, ushort amount) => val = add(val, amount);
		public static ushort subtractOff(ref this ushort val, ushort other) => val = subtract(val, other);

		public static uint addIn(ref this uint val, uint amount) => val = add(val, amount);
		public static uint subtractOff(ref this uint val, uint other) => val = subtract(val, other);

		public static ulong addIn(ref this ulong val, ulong amount) => val = add(val, amount);
		public static ulong subtractOff(ref this ulong val, ulong other) => val = subtract(val, other);
	}

	public static class StringExtensions
	{
		public static string Truncate(this string text, int length)
		{
			if (text.Length < length)
			{
				return text;
			}
			else
			{
				return text.Substring(0, length);
			}
		}
	}
}