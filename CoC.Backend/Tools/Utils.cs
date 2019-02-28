//Utils.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System;
using System.Drawing;

//IK Utils is generally frowned upon, but there's a bunch of useful tools, so.
namespace CoC.Backend.Tools
{
	public static class Utils
	{
		private static Random rnd = new Random();

		//Random is generic, but strongly, staticly typed.
		//RandomChoice still works with everything, but you
		//have to provide the type like so
		//int diceRoll = RandomChoice<int>(1,2,3,4);
		public static T RandomChoice<T>(params T[] theList)
		{
			return theList[rnd.Next(theList.Length)];
		}

		//Call this for unit testing. Using a predefined seed,
		//you can calculate the next few results, and check if the
		//results are what you'd expect.
		public static void DebugSetRandomSeed(int newSeed)
		{
			rnd = new Random(newSeed);
		}

		//no idea if anyone would ever use this, but 
		//it now supports negative numbers. 
		//(neg_num - 0]
		public static int Rand(int max)
		{
			if (max < 0)
			{
				return -(rnd.Next(-max));
			}
			return rnd.Next(max);
		}

		public static double RandDouble(double max)
		{
			return (rnd.NextDouble() * max);
		}

		public static long RandLong(long max)
		{
			return (long) (rnd.NextDouble() * max);
		}

		public static bool RandBool()
		{
			return Convert.ToBoolean(rnd.Next(0, 2));
		}

		public static void RandBytes(ref byte[] data)
		{
			rnd.NextBytes(data);
		}

		public static byte RandByte()
		{
			byte[] retVal = new byte[1];
			rnd.NextBytes(retVal);
			return retVal[0];
		}

		public static void Clamp<T>(ref T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0) val = min;
			else if (val.CompareTo(max) > 0) val = max;
		}

		

	}

	public class Pair<T, U>
	{
		public readonly T first;
		public readonly U second;

		public Pair(T f, U s)
		{
			first = f;
			second = s;
		}
	}

}
