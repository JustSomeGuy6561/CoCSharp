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
	}
}
