//Lottery.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System.Collections.Generic;
using System.Linq;

namespace CoC.Backend.Tools
{
	internal class Lottery<T> : IRandom<T>
	{
		private Dictionary<T, int> data = new Dictionary<T, int>();

		public Lottery(){}

		//a constructor that allows you to add items all at once.
		public Lottery(params T[] args)
		{
			foreach (T arg in args)
			{
				if (data.ContainsKey(arg))
				{
					data[arg]++;
				}
				else
				{
					data.Add(arg, 1);
				}
			}
		}

		//add several items at once.
		public void addItems(params T[] args)
		{
			foreach (T arg in args)
			{
				if (data.ContainsKey(arg))
				{
					data[arg]++;
				}
				else
				{
					data.Add(arg, 1);
				}
			}
		}

		//add an item, optionally multiple times
		public void addItem(T item, int numberOfEntries = 1)
		{
			//prevent negative number wrap arounds
			if (numberOfEntries <= 0) return;
			if (data.ContainsKey(item))
			{
				data[item] += numberOfEntries;
			}
			else
			{
				data.Add(item, numberOfEntries);
			}
		}

		public T Select()
		{
			int randCount = data.Values.Aggregate(0, (x, y) => x += y);
			int rand = Utils.Rand(randCount);
			int runningSum = 0;
			foreach (var entry in data)
			{
				runningSum += entry.Value;
				if (rand < runningSum)
				{
					return entry.Key;
				}
			}
			throw new System.Exception("This should never proc. If it does, we've managed to break addition. Read the stack trace.");
		}

		public void Clear()
		{
			data.Clear();
		}

		public void RemoveItem(T item)
		{
			data.Remove(item);
		}
	}
}
