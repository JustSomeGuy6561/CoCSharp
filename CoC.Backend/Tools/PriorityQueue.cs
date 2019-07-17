using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.Tools
{
	//Simple priority queue. Priorities do not change. 
	//log push, pop
	//constant peek 
	//

#warning Implement ISerializeable
	public class PriorityQueue<T>: IEnumerable<T> where T : IComparable<T>
	{
		//use an arrayList, bubble up.
		private readonly List<T> data = new List<T>();

		public PriorityQueue()
		{
		}

		public PriorityQueue(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				Push(item);
			}
		}

		private int parentIndex(int index) => (index - 1) / 2;
		private int leftChild(int index) => index * 2 + 1;
		private int rightChild(int index) => (index + 1) * 2;

		private void swapData(int parent, int child)
		{
			T temp = data[child];
			data[child] = data[parent];
			data[parent] = temp;
		}
		public void Push(T item)
		{
			if (item == null) throw new ArgumentNullException(nameof(item));
			data.Add(item);
			int currentIndex = data.Count - 1; // child index; start at end
			while (currentIndex > 0)
			{
				int parent = parentIndex(currentIndex);
				if (data[currentIndex].CompareTo(data[parent]) >= 0)
				{
					return;
				}
				swapData(parent, currentIndex);
				currentIndex = parent;
			}
		}

		public T Pop()
		{
			if (Count == 0) throw new InvalidOperationException("The priority queue is empty. Cannot pop off empty queue");
			int current = 0;
			int last = data.Count - 1; //alias so i don't have to write data.Count-1 everywhere.
			T retVal;
			if (last == 0)
			{
				retVal = data[last];
				data.Clear();
				return retVal;
			}

			swapData(current, last);

			retVal = data[last];
			data.RemoveAt(last);//RemoveAt is O(n), but here n is always 1, so O(1). 

			while (leftChild(current) < data.Count)
			{
				int left = leftChild(current);
				int right = rightChild(current);

				int smallestChild = right < data.Count && data[right].CompareTo(data[left]) < 0 ? right : left;

				if (data[current].CompareTo(data[smallestChild]) < 0)
				{
					break;
				}

				swapData(current, smallestChild);
				current = smallestChild;
			}
			return retVal;
		}

		public T Peek()
		{
			if (Count == 0) throw new InvalidOperationException("The priority queue is empty. Cannot peek an empty queue");
			return data[0];
		}

		public int Count => data.Count;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			data.ForEach(x => sb.Append(x.ToString() + ", "));
			sb.Append(". Count: ");
			sb.Append(data.Count);
			return sb.ToString();
		}

		public bool isEmpty => Count == 0;

		public void Clear()
		{
			data.Clear();
		}

		public bool Contains(T item)
		{
			return data.Contains(item);
		}

		//time: n + log(n) or n.

		public bool Remove(T item)
		{
			int index = data.IndexOf(item);
			if (index == -1)
			{
				return false;
			}
			removeItem(index);
			return true;
		}

		private void removeItem(int index)
		{
			swapData(index, Count - 1);
			data.RemoveAt(Count - 1);

			//and heapify. log(n)

			while (leftChild(index) < data.Count)
			{
				int left = leftChild(index);
				int right = rightChild(index);

				int smallestChild = right < data.Count && data[right].CompareTo(data[left]) < 0 ? right : left;

				if (data[index].CompareTo(data[smallestChild]) < 0)
				{
					break;
				}

				swapData(index, smallestChild);
				index = smallestChild;
			}
		}



		//nLog(n) at worst case. mLog(n) more accurate, where m is the number of objects removed.

		public bool RemoveAllOf(T item)
		{
			IEnumerable<int> indices = data.Select((x, y) => x.CompareTo(item) == 0 ? y : -1).Where(x => x != -1);

			bool hasAnyItems = false; //could use count, but that's longer. i only care if it has more than zero elements removed.
			foreach (var index in indices)
			{
				hasAnyItems = true;
				removeItem(index);
			}
			return hasAnyItems;
		}

		public List<T>.Enumerator GetEnumarator()
		{
			return data.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return data.GetEnumerator();
		}
	}
}
