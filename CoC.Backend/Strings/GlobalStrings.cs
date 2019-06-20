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
			chars[0] = char.ToUpper(chars[0]);
			return new string(chars);
		}

		//simple plural check for strings ending in S.
		public static bool IsPluralWithS(this string text)
		{
			string s = text.TrimEnd();
			return s[s.Length - 1] == 's';
		}

		public static string YES(bool capital = true)
		{
			return capital ? "Yes" : "yes";
		}

		public static string NO(bool capital = true)
		{
			return capital ? "No" : "no";
		}

		public static string OK(bool capital = true)
		{
			return capital ? "Ok" : "ok";
		}

		public static string BACK(bool capital = true)
		{
			return capital ? "Back" : "back";
		}

		public static string CANCEL(bool capital = true)
		{
			return capital ? "Cancel" : "cancel";
		}

		public static string MAN(bool capital = true)
		{
			return capital ? "Man" : "man";
		}
		public static string WOMAN(bool capital = true)
		{
			return capital ? "Woman" : "woman";
		}
		public static string HERM(bool capital = true)
		{
			return capital ? "Herm" : "herm";
		}

		public static string DEFAULT(bool capital = true)
		{
			return capital ? "Default" : "default";
		}

		public static string NEXT(bool capital = true)
		{
			return capital ? "Next" : "next";
		}

		public static string CONTINUE(bool capital = true)
		{
			return capital ? "Continue" : "continue";
		}

		public static string RETURN(bool capital = true)
		{
			return capital ? "Return" : "return";
		}

		public static string CONFIRM(bool capital = true)
		{
			return capital ? "Confirm" : "confirm";
		}
	}


}
