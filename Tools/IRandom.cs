//IRandom.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Tools
{
	interface IRandom<T>
	{
		T Select();

		void addItems(params T[] items);
	}
}
