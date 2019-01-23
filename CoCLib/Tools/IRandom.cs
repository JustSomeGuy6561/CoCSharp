//IRandom.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM

namespace CoC.Tools
{
	interface IRandom<T>
	{
		T Select();

		void addItems(params T[] items);
	}
}
