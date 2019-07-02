//Enums.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:06 PM
using System;

namespace CoC.Backend.Tools
{
	public static class EnumHelper
	{
		//Flags attribute lets you use bitwise operations.
		public static int Length<T>() where T : Enum
		{
			return Enum.GetNames(typeof(T)).Length;
		}

		public static T ByteEnumAdd<T>(this T val, byte amount) where T : Enum
		{
			byte amt = Convert.ToByte(val);
			amt.addIn(amount);
			return (T)Enum.Parse(typeof(T), amt.ToString());
		}

		public static T ByteEnumSubtract<T>(this T val, byte amount) where T : Enum
		{
			byte amt = Convert.ToByte(val);
			amt.subtractOff(amount);
			return (T)Enum.Parse(typeof(T), amt.ToString());
		}
	}
}
