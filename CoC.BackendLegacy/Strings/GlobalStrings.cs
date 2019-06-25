﻿//GlobalStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:04 PM
using System;
using CoC.Backend.Creatures;
using  CoC.Backend.BodyParts;

namespace CoC.Backend.Strings
{
	static class GlobalStrings
	{
		private static readonly string[] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"};

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

		public static string LengthInHalfInch(double lengthInInches)
		{
			double decimalPoint = lengthInInches % 1;
			if (decimalPoint > .75)
			{
				decimalPoint = 1;
			}
			else if (decimalPoint > .25)
			{
				decimalPoint = 0.5f;
			}
			else
			{
				decimalPoint = 0f;
			}
			lengthInInches = Math.Floor(lengthInInches) + decimalPoint;

			return lengthInInches.ToString() + " inch";
		}
		public static string LengthInHalfInches(double lengthInInches)
		{
			return LengthInHalfInch(lengthInInches) + "es";
		}

		public static string LengthInInch(double lengthInInches)
		{
			lengthInInches = Math.Round(lengthInInches, MidpointRounding.AwayFromZero);
			return lengthInInches.ToString() + " inch";
		}

		public static string LengthInInches(double lengthInInches)
		{
			lengthInInches = Math.Round(lengthInInches, MidpointRounding.AwayFromZero);
			return lengthInInches.ToString() + " inches";
		}

		private static bool IMPERIAL = true;

		public static string FeetOrMeters(double inches)
		{
			if (IMPERIAL)
			{
				return inches / 12.0 + "feet";
			}
			else
			{
				return inches * 0.0254 + "meters";
			}
		}


		public static string CantAttackName<T>(T type)
		{
			#if DEBUG
			return "Warning: you called an attack name, but cannot attack with this body part! Type: " + type.GetType().Name;
			#else
			return "";
			#endif
		}

		public static string CantAttackWith<T, U>(T type, Player player) where T : BehavioralSaveablePart<T, U> where U : SaveableBehavior<U,T>
		{
			#if DEBUG
			return "Warning: you called an attack hint, but cannot attack with this body part! Type: " + type.GetType().Name;
			#else
			return "";
			#endif
		}

	}

}