//GlobalStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:04 PM
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CoC.BodyParts;
namespace CoC.Strings
{
	static class GlobalStrings
	{
		public static string None() { return ""; }

		public static string TransformToDefault<T, Y>(T type, Player p) where T : BodyPartBase<T, Y> where Y : BodyPartBehavior<Y, T>
		{
			return type.restoreString(type, p);
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

		public static string CantAttackName<T>(T type)
		{
			#if DEBUG
			return "Warning: you called an attack name, but cannot attack with this body part! Type: " + type.GetType().Name;
			#else
			return "";
			#endif
		}

		public static string CantAttackWith<T, U>(T type, Player player) where T : BodyPartBase<T, U> where U : BodyPartBehavior<U,T>
		{
			#if DEBUG
			return "Warning: you called an attack hint, but cannot attack with this body part! Type: " + type.GetType().Name;
			#else
			return "";
			#endif
		}

	}

}
