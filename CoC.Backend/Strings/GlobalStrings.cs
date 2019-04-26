//GlobalStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:04 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;

namespace CoC.Backend.Strings
{
	public static class GlobalStrings
	{
		private static readonly string[] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };

		public static string NumberAsText(int number)
		{
			if (number >= 10 || number < 0)
			{
				return number.ToString();
			}
			return numbers[number];
		}

		public static string None() { return ""; }

		public static string TransformToDefault<T, Y>(T type, Player p) where T : BehavioralSaveablePart<T, Y> where Y : SaveableBehavior<Y, T>
		{
			return type.restoreString(p);
		}

		public static string RevertAsDefault<T>(T type, Player p)
		{
#if DEBUG
			return "Debug Warning: you've tried to change to the default " + type.GetType().Name + ", but you're already it!";
#else
			return "";
#endif

		}

		public static string CantAttackName<T>(T type)
		{
#if DEBUG
			return "Warning: you called an attack name, but cannot attack with this body part! Type: " + type.GetType().Name;
#else
			return "";
#endif
		}

		public static string CantAttackWith<T, U>(T type, Player player) where T : BehavioralSaveablePart<T, U> where U : SaveableBehavior<U, T>
		{
#if DEBUG
			return "Warning: you called an attack hint, but cannot attack with this body part! Type: " + type.GetType().Name;
#else
			return "";
#endif
		}

		public static string CapitalizeFirstLetter(this string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				#if DEBUG
				return "Warning: You called capitalize on a null or empty string";
				#else
				return text;
				#endif
			}
			char[] chars = text.ToCharArray();
			return new string(chars);
		}

		//simple plural check for strings ending in S.
		public static bool IsPluralWithS(this string text)
		{
			string s = text.TrimEnd();
			return s[s.Length - 1] == 's';
		}
	}

}
