//GlobalStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:04 PM
using System;
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

		public static string CantAttackWith<T, U,V>(T type, Player player) where T : BehavioralSaveablePart<T, U,V> where U : SaveableBehavior<U, T, V> 
			where V: BehavioralSaveablePartData<V, T, U>
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

		public static string YES() => YES(true);
		public static string NO() => NO(true);
		public static string OK() => OK(true);
		public static string BACK() => BACK(true);
		public static string CANCEL() => CANCEL(true);
		public static string MAN() => MAN(true);
		public static string WOMAN() => WOMAN(true);
		public static string HERM() => HERM(true);
		public static string DEFAULT() => DEFAULT(true);
		public static string NEXT() => NEXT(true);
		public static string PREVIOUS() => PREVIOUS(true);
		public static string NEXT_PAGE() => NEXT_PAGE(true);
		public static string PREVIOUS_PAGE() => PREVIOUS_PAGE(true);
		public static string CONTINUE() => CONTINUE(true);
		public static string RETURN() => RETURN(true);
		public static string CONFIRM() => CONFIRM(true);
		public static string NEVERMIND() => NEVERMIND(true);

		public static string NEVERMIND(bool capital)
		{
			return capital ? "Nevermind" : "nevermind";
		}
		public static string YES(bool capital)
		{
			return capital ? "Yes" : "yes";
		}

		public static string NO(bool capital)
		{
			return capital ? "No" : "no";
		}

		public static string OK(bool capital)
		{
			return capital ? "OK" : "ok";
		}

		public static string BACK(bool capital)
		{
			return capital ? "Back" : "back";
		}

		public static string CANCEL(bool capital)
		{
			return capital ? "Cancel" : "cancel";
		}

		public static string MAN(bool capital)
		{
			return capital ? "Man" : "man";
		}
		public static string WOMAN(bool capital)
		{
			return capital ? "Woman" : "woman";
		}
		public static string HERM(bool capital)
		{
			return capital ? "Herm" : "herm";
		}

		public static string DEFAULT(bool capital)
		{
			return capital ? "Default" : "default";
		}

		public static string NEXT(bool capital)
		{
			return capital ? "Next" : "next";
		}

		public static string NEXT_PAGE(bool capital)
		{
			return capital ? "Next Page" : "next page";
		}

		public static string PREVIOUS(bool capital)
		{
			return capital ? "Previous" : "previous";
		}

		public static string PREVIOUS_PAGE(bool capital)
		{
			return capital ? "PreviousPage" : "previouspage";
		}

		public static string CONTINUE(bool capital)
		{
			return capital ? "Continue" : "continue";
		}

		public static string RETURN(bool capital)
		{
			return capital ? "Return" : "return";
		}

		public static string CONFIRM(bool capital)
		{
			return capital ? "Confirm" : "confirm";
		}
	}


}
