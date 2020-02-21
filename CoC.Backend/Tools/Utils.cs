//Utils.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

//IK Utils is generally frowned upon, but there's a bunch of useful tools, so.
namespace CoC.Backend.Tools
{
	public static class Utils
	{
		private static readonly string[] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
		private static readonly string[] place = { "zeroth", "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "nineth", "tenth" };

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

		public static T ClampEnum2<T>(T value, T min, T max) where T : System.Enum
		{
			if (value.CompareTo(max) > 0)
			{
				return max;
			}
			else if (value.CompareTo(min) < 0)
			{
				return min;
			}
			return value;
		}

		public static void ClampEnum<T>(ref T value, T min, T max) where T : System.Enum
		{
			if (value.CompareTo(max) > 0)
			{
				value = max;
			}
			else if (value.CompareTo(min) < 0)
			{
				value = min;
			}
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

		public static double Lerp(int min, int max, double percent)
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

		public static double Lerp(uint min, uint max, double percent)
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

		public static double Lerp(int minX, int maxX, double x, int minY, int maxY)
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

		public static double Lerp(double minX, double maxX, double x, double minY, double maxY)
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

		public static string NumberAsText(int x)
		{
			if (x >= 0 && x <= 10)
			{
				return numbers[x];
			}
			else return x.ToString();
		}

		public static string NumberAsPlace(int x)
		{
			if (x >= 0 && x <= 10)
			{
				return place[x];
			}
			else if (x < 0 && x >= -10)
			{
				return "negative " + place[x];
			}
			else if (x % 10 == 1)
			{
				return x + "st";
			}
			else if (x % 10 == 2)
			{
				return x + "nd";
			}
			else if (x % 10 == 3)
			{
				return x + "rd";
			}
			else
			{
				return x + "th";
			}
		}

		public static string DescribeByScale(double value, double[] scale, string lessThan = "less than", string moreThan = "more than")
		{
			if (scale.Length == 0) return "indescribable";
			if (scale.Length == 1) return "about " + scale[0];
			if (value < scale[0]) return lessThan + " " + scale[0];
			if (value == scale[0]) return scale[0].ToString();
			for (int i = 1; i < scale.Length; i++)
			{
				if (value < scale[i]) return "between " + scale[i - 1] + " and " + scale[i];
				if (value == scale[i]) return scale[i].ToString();
			}
			return moreThan + " " + scale[scale.Length - 1];
		}

		//simple version of adding an article for english. not foolproof (i.e. "a hour" vs "an hour"), simply checks first letter, appends a or an accordingly.
		public static string AddArticle(string text)
		{
			text = text.TrimStart();
			if (text[0] == 'a' || text[0] == 'e' || text[0] == 'i' || text[0] == 'o' || text[0] == 'u' ||
				text[0] == 'A' || text[0] == 'E' || text[0] == 'I' || text[0] == 'O' || text[0] == 'U' ||
				text[0] == '8')
			{
				return "an " + text;
			}
			else
			{
				return "a " + text;
			}
		}

		public static string AddArticleIf(string text, bool needsArticle)
		{
			if (needsArticle) return AddArticle(text);
			else return text.TrimStart();
		}

		public static string Pluralize(string text)
		{
			return text.TrimEnd() + "s";
		}
		public static string PluralizeIf(string text, bool plural)
		{
			if (plural) return Pluralize(text);
			else return text.TrimEnd();
		}
	}

	[Serializable]
	public class Pair<T, U> : ISerializable
	{
		public readonly T first;
		public readonly U second;

		public Pair(T f, U s)
		{
			first = f;
			second = s;
		}

		protected Pair(SerializationInfo info, StreamingContext context)
		{
			first = (T)info.GetValue(nameof(first), typeof(T));
			second = (U)info.GetValue(nameof(second), typeof(U));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(first), first, typeof(T));
			info.AddValue(nameof(second), second, typeof(U));
		}
	}

	[Serializable]
	public class Pair<T> : Pair<T, T>
	{
		public Pair(T f, T s) : base(f, s) {}
	}

	[Serializable]
	public class Triple<T, U, V> : ISerializable
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

		protected Triple(SerializationInfo info, StreamingContext context)
		{
			first = (T)info.GetValue(nameof(first), typeof(T));
			second = (U)info.GetValue(nameof(second), typeof(U));
			third = (V)info.GetValue(nameof(third), typeof(V));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(first), first, typeof(T));
			info.AddValue(nameof(second), second, typeof(U));
			info.AddValue(nameof(third), third, typeof(V));
		}
	}

	[Serializable]
	public class Triple<T> : Triple<T, T, T>
	{
		public Triple(T f, T s, T t) : base(f, s, t) {}
	}

	public sealed class ValueDifference<T>
	{
		public readonly T oldValue;
		public readonly T newValue;

		public ValueDifference(T old, T newVal)
		{
			oldValue = old;
			newValue = newVal;
		}
	}
}

