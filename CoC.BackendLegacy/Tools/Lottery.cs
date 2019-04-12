//Lottery.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System.Collections.Generic;

namespace CoC.Backend.Tools
{
	internal class Lottery<T> : IRandom<T>
	{
		private List<T> data = new List<T>();

		public Lottery(){}

		//a constructor that allows you to add items all at once.
		public Lottery(params T[] args)
		{
			data.AddRange(args);
		}

		//add several items at once.
		public void addItems(params T[] args)
		{
			data.AddRange(args);
		}

		//add an item, optionally multiple times
		public void addItem(T item, int numberOfEntries = 1)
		{
			//prevent negative number wrap arounds
			if (numberOfEntries <= 0) return;
			for (int x = 0; x < numberOfEntries; x++)
			{
				data.Add(item);
			}
		}

		public T Select()
		{
			T[] temp = data.ToArray();
			return Utils.RandomChoice(temp);
		}
	}
}
