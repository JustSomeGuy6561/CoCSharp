//Enums.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:06 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Tools
{
	//Flags attribute lets you use bitwise operations.
	[Flags]
	public enum Gender { GENDERLESS = 0, MALE = 1, FEMALE = 2, HERM = MALE | FEMALE}
}
