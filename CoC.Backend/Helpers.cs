//Helpers.cs
//Description:
//Author: JustSomeGuy
//4/9/2019, 1:26 AM

using CoC.Backend.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
		//add: a+b. result is the same type as the source. does not overflow.
		//sub: a-b. result is the same type as the source. does not overflow.
		//diff: abs(a-b). if a is signed, returns unsigned equivalent. otherwise, returns original type.
		//delta a-b. result is a type ranging from type.MinValue - type.MaxValue to type.MaxValue - type.MinValue. i.e. byte.delta(byte) is short.
		//offset a+b. a is unsigned. b is signed. result is the same type as a.

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

		public static byte difference(this sbyte first, sbyte second)
		{
			if (second >= first)
			{
				return (byte)(second - first);
			}
			return (byte)(first - second);
		}

		public static byte difference(this byte first, byte second)
		{
			if (second >= first)
			{
				return (byte)(second - first);
			}
			return (byte)(first - second);
		}

		public static byte offset(this byte first, sbyte second)
		{
			return (byte)Utils.Clamp2(first - second, byte.MinValue, byte.MaxValue);
		}

		//there's probably some unchecked magic i can do here. not gonna even try.
		public static short delta(this byte first, byte second)
		{
			return (short)(first - second);
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

		public static ushort offset(this ushort first, short second)
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

		public static uint offset(this uint first, int second)
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

		public static ulong offset(this ulong first, long second)
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

	public static class CollectionHelpers
	{
		public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable)
		{
			//true or null (null is true)
			return enumerable?.IsEmpty() != false;
		}

		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			bool retVal;

			if (enumerable is ICollection<T> col)
			{
				return col.Count > 0;
			}

			using (IEnumerator<T> item = enumerable.GetEnumerator())
			{
				retVal = !item.MoveNext();
			}

			return retVal;
		}

		public static void ForEach<T>(this ReadOnlyCollection<T> collection, Action<T> callback)
		{
			foreach (T item in collection)
			{
				callback(item);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> callback)
		{
			foreach (T item in collection)
			{
				callback(item);
			}
		}

		public static T MaxItem<T, U>(this IEnumerable<T> collection, Func<T, U> getValue) where U : IComparable<U>
		{
			if (getValue is null) throw new ArgumentNullException(nameof(getValue));

			if (IsNullOrEmpty(collection))
			{
				return default;
			}
			return collection.Aggregate((i1, i2) =>
			{
				if (i1 == null) return i2;
				else if (i2 == null) return i1;
				return getValue(i1).CompareTo(getValue(i2)) > 0 ? i1 : i2;
			});
		}

		public static int MaxIndex<T, U>(this IList<T> collection, Func<T, U> getValue) where U : IComparable<U>
		{
			if (getValue is null) throw new ArgumentNullException(nameof(getValue));

			if (IsNullOrEmpty(collection))
			{
				return -1;
			}

			int result = -1;
			for (int x = 0; x < collection.Count; x++)
			{
				if (collection[x] == null) continue;
				else if (result == -1) result = x;
				else if (getValue(collection[x]).CompareTo(getValue(collection[result])) > 0)
				{
					result = x;
				}
			}

			return result;
		}

		public static T MinItem<T, U>(this IEnumerable<T> collection, Func<T, U> getValue) where U : IComparable<U>
		{
			if (IsNullOrEmpty(collection))
			{
				return default;
			}
			return collection.Aggregate((i1, i2) =>
			{
				if (i1 == null) return i2;
				else if (i2 == null) return i1;
				return getValue(i1).CompareTo(getValue(i2)) < 0 ? i1 : i2;
			});
		}

		public static int MinIndex<T, U>(this IList<T> collection, Func<T, U> getValue) where U : IComparable<U>
		{
			if (getValue is null) throw new ArgumentNullException(nameof(getValue));

			if (IsNullOrEmpty(collection))
			{
				return -1;
			}

			int result = -1;
			for (int x = 0; x < collection.Count; x++)
			{
				if (collection[x] == null) continue;
				else if (result == -1) result = x;
				else if (getValue(collection[x]).CompareTo(getValue(collection[result])) < 0)
				{
					result = x;
				}
			}

			return result;
		}



		public static T MinItem<T, U>(this IEnumerable<T> collection, Func<T, U?> getValue) where U : struct, IComparable<U>
		{
			if (IsNullOrEmpty(collection))
			{
				return default;
			}
			return collection.Aggregate((i1, i2) =>
			{
				if (i1 == null) return i2;
				else if (i2 == null) return i1;
				return ((U)getValue(i1)).CompareTo((U)getValue(i2)) < 0 ? i1 : i2;
			});
		}

		public static int FirstIndexOf<T>(this ReadOnlyCollection<T> collection, Predicate<T> match)
		{
			if (collection is null) throw new ArgumentNullException(nameof(collection));
			if (match is null) throw new ArgumentNullException(nameof(match));

			for(int x = 0; x< collection.Count;x++)
			{
				if (match(collection[x]))
				{
					return x;
				}
			}

			return -1;
		}

		public static void Push<T>(this Queue<T> queue, T item)
		{
			queue.Enqueue(item);
		}

		public static T Pop<T>(this Queue<T> queue)
		{
			return queue.Dequeue();
		}

		public static bool Contains<T>(this T[] array, T target) where T : class
		{
			return Array.Exists(array, x => x == target);
		}

		//source: stack overflow
		//https://stackoverflow.com/questions/5063178/counting-bits-set-in-a-net-bitarray-class
		public static int GetCardinality(this BitArray bitArray)
		{

			int[] ints = new int[(bitArray.Count >> 5) + 1];

			bitArray.CopyTo(ints, 0);

			int count = 0;

			// fix for not truncated bits in last integer that may have been set to true with SetAll()
			ints[ints.Length - 1] &= ~(-1 << (bitArray.Count % 32));

			for (int i = 0; i < ints.Length; i++)
			{

				int c = ints[i];

				// magic (http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel)
				unchecked
				{
					c = c - ((c >> 1) & 0x55555555);
					c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
					c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
				}

				count += c;

			}

			return count;

		}

		public static U GetItemClean<T, U>(this IDictionary<T, U> dictionary, T location)
		{
			dictionary.TryGetValue(location, out U retVal);
			return retVal;
		}
	}

}

namespace CoC.LinqHelpers
{
	public static class LinqHelper
	{
		public static bool All<T>(this IEnumerable<T> collection, Func<T, int, bool> checkWithIndex)
		{
			if (collection is null)
			{
				return false;
			}
			if (checkWithIndex is null) throw new ArgumentNullException(nameof(checkWithIndex));


			var enumerator = collection.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return false;
			}
			int x = 0;
			do
			{
				if (!checkWithIndex(enumerator.Current, x))
				{
					return false;
				}
				x++;
			}
			while (enumerator.MoveNext());

			return true;
		}

		public static bool Any<T>(this IEnumerable<T> collection, Func<T, int, bool> checkWithIndex)
		{
			if (collection is null)
			{
				return false;
			}
			if (checkWithIndex is null) throw new ArgumentNullException(nameof(checkWithIndex));


			var enumerator = collection.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return false;
			}
			int x = 0;
			do
			{
				if (checkWithIndex(enumerator.Current, x))
				{
					return true;
				}
				x++;
			}
			while (enumerator.MoveNext());

			return false;
		}
	}
}
