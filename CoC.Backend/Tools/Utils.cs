//Utils.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

//IK Utils is generally frowned upon, but there's a bunch of useful tools, so.
namespace CoC.Backend.Tools
{
	public static class Utils
	{
		private static readonly string[] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };

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

		public static T Clamp2<T>(T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0) val = min;
			else if (val.CompareTo(max) > 0) val = max;
			return val;
		}

		public static IEnumerable<T> AsIterable<T>() where T: Enum
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		public static void AddAt<T>(this List<T> list, T item, int index)
		{
			if (list.Count <= index)
			{
				while (list.Count < index)
				{
					list.Add(default);
				}
				list.Add(item);
			}
			else if (list.Count > index)
			{
				list[index] = item;
			}
			else
			{
				list.Add(item);
			}
		}

		public static float Lerp(int min, int max, float percent)
		{
			if (percent <= 0)
			{
				return min;
			}
			else if (percent >= 1)
			{
				return max;
			}
			return min + (max - min) * percent;
		}


		public static float Lerp(int minX, int maxX, float x, int minY, int maxY)
		{
			if (x >= maxX)
			{
				return maxY;
			}
			else if (x <= minX)
			{
				return minY;
			}

			return (minY * (maxX - x) + maxY * (x - minX)) / (maxX - minX);
		}

		public static float Lerp(float minX, float maxX, float x, float minY, float maxY)
		{
			if (x >= maxX)
			{
				return maxY;
			}
			else if (x <= minX)
			{
				return minY;
			}

			return (minY * (maxX - x) + maxY * (x - minX)) / (maxX - minX);
		}

		public static byte LerpRound(byte minX, byte maxX, byte x, byte minY, byte maxY)
		{
			return (byte)Math.Round(Lerp(minX, maxX, x, minY, maxY));
		}

		public static ushort LerpRound(ushort minX, ushort maxX, ushort x, ushort minY, ushort maxY)
		{
			return (ushort)Math.Round(Lerp(minX, maxX, x, minY, maxY));
		}

		public static short LerpRound(short minX, short maxX, short x, short minY, short maxY)
		{
			return (short)Math.Round(Lerp(minX, maxX, x, minY, maxY));
		}

		public static int LerpRound(int minX, int maxX, int x, int minY, int maxY)
		{
			return (int)Math.Round(Lerp(minX, maxX, x, minY, maxY));
		}

		public static string Count(int x)
		{
			if (x >= 0 && x <= 10)
			{
				return numbers[x];
			}
			else return x.ToString();
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

	public class Pair<T> : Pair<T, T>
	{
		public Pair(T f, T s) : base(f, s) {}
	}

	public class Triple<T, U, V>
	{
		public readonly T first;
		public readonly U second;
		public readonly V third;

		public Triple(T f, U s, V t)
		{
			first = f;
			second = s;
			third = t;
		}
	}

	public class Triple<T> : Triple<T, T, T>
	{
		public Triple(T f, T s, T t) : base(f, s, t) {}
	}
}

