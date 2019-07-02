//OrderedHashSet.cs
//Description:
//Author: Andrew Baumher
//6/29/2019, 3:58 AM
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CoC.Backend.Tools
{
	//I HAVE NO IDEA IF THIS IS ACTUALLY SERIALIZABLE!
	//DON'T SERIALIZE IT LOL!

	//I hate this class, and i hate you for asking me to do it. You're lucky i was bored - Andrew.
	//Also, you owe me a beer. 

	/*
	 * ----------------------------------------------------------------------------
	 * "THE BEER-WARE LICENSE" (Revision 42):
	 * Andrew Baumher wrote this file. As long as you retain this notice you
	 * can do whatever you want with this stuff. If we meet some day, and you think
	 * this stuff is worth it, you can buy me a beer in return.     -Andrew Baumher (Contact: abaum912@comcast.net)
	 * ----------------------------------------------------------------------------
	 */

	[Serializable()]
	public class OrderedHashSet<T> : IEnumerable<T>, ICollection<T>, ISerializable, IDeserializationCallback, ISet<T>, IReadOnlyCollection<T>
	{
		protected Dictionary<T, LinkedListNode<T>> lookup; //gives us all the niceties of HashSet, with nodes so we can do O(1) find in the linked list for remove/add.
		protected LinkedList<T> order; //stores the order

		protected SerializationInfo _info;

		public int Count => order.Count;
		public int capacity => Count > _initialCapacity ? Count : _initialCapacity;
		private readonly int _initialCapacity;
		public IEqualityComparer<T> Comparer => lookup.Comparer;

		#region Constructors
		public OrderedHashSet() : this(EqualityComparer<T>.Default)
		{ }

		public OrderedHashSet(IEnumerable<T> source) : this(source, EqualityComparer<T>.Default)
		{ }

		public OrderedHashSet(IEqualityComparer<T> equalityComparer)
		{
			if (equalityComparer == null)
			{
				equalityComparer = EqualityComparer<T>.Default;
			}
			order = new LinkedList<T>();
			lookup = new Dictionary<T, LinkedListNode<T>>();
		}

		public OrderedHashSet(int initialCapacity) : this(initialCapacity, EqualityComparer<T>.Default)
		{ }

		public OrderedHashSet(IEnumerable<T> source, IEqualityComparer<T> equalityComparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}
			if (equalityComparer == null)
			{
				equalityComparer = EqualityComparer<T>.Default;
			}
			order = new LinkedList<T>();
			lookup = new Dictionary<T, LinkedListNode<T>>(source.Count(), equalityComparer);
			//is there a faster way of doing this? probably. I'm not MS though, so this is good enough.
			foreach (T t in source)
			{
				//assure uniqueness before adding to linked list
				if (!lookup.ContainsKey(t))
				{
					LinkedListNode<T> node = order.AddLast(t);
					lookup.Add(t, node);
				}
			}
		}

		public OrderedHashSet(int initialCapacity, IEqualityComparer<T> equalityComparer)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(initialCapacity));
			}
			if (equalityComparer == null)
			{
				equalityComparer = EqualityComparer<T>.Default;
			}
			order = new LinkedList<T>();
			lookup = new Dictionary<T, LinkedListNode<T>>(initialCapacity);
		}

		protected OrderedHashSet(SerializationInfo info, StreamingContext context)
		{
			//just copy doing some copypasta from HashSet.cs, don't mind me.
			_info = info;
			_initialCapacity = 0;
		}
		#endregion
		#region IEnumerable

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
		#endregion
		#region ICollection
		//int ICollection<T>.Count => order.Count;

		bool ICollection<T>.IsReadOnly => false;

		void ICollection<T>.Add(T item)
		{
			addItem(item);
		}

		public void Clear()
		{
			clearAll();
		}

		public bool Contains(T item)
		{
			return containsItem(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			CopyTo(array, arrayIndex, Count);
		}
		public void CopyTo(T[] array)
		{
			CopyTo(array, 0, Count);
		}

		public void CopyTo(T[] array, int arrayIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array));
			}

			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}
			if (arrayIndex > array.Length || count > array.Length - arrayIndex)
			{
				throw new ArgumentException("target array is too small");
			}

			int numCopied = 0;
			LinkedListNode<T> current = order.First;
			while (current != null && numCopied < count)
			{
				array[arrayIndex + numCopied++] = current.Value;
				current = current.Next;
			}
		}

		public bool Remove(T item)
		{
			return removeItem(item);
		}

		public int RemoveWhere(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			int numRemoved = 0;
			LinkedListNode<T> curr = order.First;
			while (curr != null)
			{
				LinkedListNode<T> next = curr.Next;
				if (match(curr.Value))
				{
					removeItem(curr.Value);
					numRemoved++;
				}
				curr = next;
			}

			return numRemoved;
		}
		#endregion
		#region ISet

		public bool Add(T item)
		{
			return addItem(item);
		}

		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}
			if (Count == 0)
			{
				return;
			}
			if (other == this)
			{
				clearAll();
				return;
			}

			foreach (var elem in other)
			{
				removeItem(elem);
			}
		}
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}
			if (Count == 0)
			{
				UnionWith(other);
				return;
			}
			if (other == this)
			{
				Clear();
				return;
			}

			if (other is OrderedHashSet<T> orderedOtherCollection && EqualComparators(this, orderedOtherCollection))
			{
				XOR_OrderedHashSet(orderedOtherCollection);
				return;
			}

			XOR_Enumerable(other);
		}
		protected void XOR_OrderedHashSet(OrderedHashSet<T> other)
		{
			foreach (T t in other)
			{
				if (!removeItem(t))
				{
					addItem(t);
				}
			}
		}

		protected void XOR_Enumerable(IEnumerable<T> other)
		{
			//i know for a fact this is not the fastest method. i don't care lol.
			HashSet<T> cleanOther = new HashSet<T>(other);

			LinkedListNode<T> curr = order.First;
			while (curr != null)
			{
				LinkedListNode<T> next = curr.Next;
				if (other.Contains(curr.Value))
				{
					removeItem(curr.Value);
				}
				curr = next;
			}
		}
		public void IntersectWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}

			if (Count == 0)
			{
				return;
			}

			if (other is ICollection<T> otherAsCollection)
			{
				if (otherAsCollection.Count == 0)
				{
					Clear();
					return;
				}

				if (other is OrderedHashSet<T> otherAsSet && EqualComparators(this, otherAsSet))
				{
					IntersectNicely(otherAsSet);
					return;
				}
			}

			IntersectLessNicely(other);
		}

		private void IntersectNicely(OrderedHashSet<T> other)
		{
			LinkedListNode<T> curr = order.First;
			while (curr != null)
			{
				LinkedListNode<T> next = curr.Next;
				if (other.Contains(curr.Value))
				{
					removeItem(curr.Value);
				}
				curr = next;
			}
		}

		private void IntersectLessNicely(IEnumerable<T> other)
		{
			//i know for a fact this is not the fastest method. i don't care lol.
			HashSet<T> cleanOther = new HashSet<T>(other);

			LinkedListNode<T> curr = order.First;
			while (curr != null)
			{
				LinkedListNode<T> next = curr.Next;
				if (!other.Contains(curr.Value))
				{
					removeItem(curr.Value);
				}
				curr = next;
			}
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}

			// The empty set is a subset of any set
			if (Count == 0)
			{
				return true;
			}

			// faster if other has unique elements according to this equality comparer; so check 
			// that other is a hashset using the same equality comparer.
			if (other is OrderedHashSet<T> otherAsSet && EqualComparators(this, otherAsSet))
			{
				// if this has more elements then it can't be a subset
				if (Count > otherAsSet.Count)
				{
					return false;
				}

				// already checked that we're using same equality comparer. simply check that 
				// each element in this is contained in other.
				return IsSubsetNicely(otherAsSet);
			}
			else
			{
				return IsSubsetLessNicely(other);
			}
		}

		private bool IsSubsetNicely(OrderedHashSet<T> other)
		{
			foreach (var key in lookup.Keys)
			{
				if (!other.lookup.ContainsKey(key))
				{
					return false;
				}
			}
			return true;
		}

		private bool IsSubsetLessNicely(IEnumerable<T> other)
		{
			HashSet<T> cleanOther = new HashSet<T>(other);

			foreach (var entry in order)
			{
				if (!cleanOther.Contains(entry))
				{
					return false;
				}
			}
			return true;
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}

			if (other is ICollection<T> otherAsCollection)
			{
				// the empty set is a proper subset of anything but the empty set
				if (Count == 0)
				{
					return otherAsCollection.Count > 0;
				}
				// faster if other is a hashset (and we're using same equality comparer)
				if (other is OrderedHashSet<T> otherAsSet && EqualComparators(this, otherAsSet))
				{
					if (Count >= otherAsSet.Count)
					{
						return false;
					}
					// this has strictly less than number of items in other, so the following
					// check suffices for proper subset.
					return IsSubsetNicely(otherAsSet);
				}
			}

			if (other.Count() >= Count)
			{
				return false;
			}
			else
			{
				return IsSubsetLessNicely(other);
			}

		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			// try to fall out early based on counts
			if (other is ICollection<T> otherAsCollection)
			{
				// if other is the empty set then this is a superset
				if (otherAsCollection.Count == 0)
				{
					return true;
				}
				// try to compare based on counts alone if other is a hashset with
				// same equality comparer
				if (other is OrderedHashSet<T> otherAsSet && EqualComparators(this, otherAsSet))
				{
					if (otherAsSet.Count > Count)
					{
						return false;
					}
					return ContainsAllElementsNicely(otherAsSet);
				}
			}

			return ContainsAllElementsLessNicely(other);
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			

			// the empty set isn't a proper superset of any set.
			if (Count == 0)
			{
				return false;
			}

			if (other is ICollection<T> otherAsCollection)
			{
				// if other is the empty set then this is a superset
				if (otherAsCollection.Count == 0)
				{
					// note that this has at least one element, based on above check
					return true;
				}
				// faster if other is a hashset with the same equality comparer
				if (other is OrderedHashSet<T> otherAsSet && EqualComparators(this, otherAsSet))
				{
					if (otherAsSet.Count >= Count)
					{
						return false;
					}
					// now perform element check
					return ContainsAllElementsNicely(otherAsSet);
				}
			}

			if (other.Count() >= Count)
			{
				return false;
			}
			return ContainsAllElementsLessNicely(other);

		}

		private bool ContainsAllElementsNicely(OrderedHashSet<T> other)
		{
			foreach (var t in other.order)
			{
				if (!containsItem(t))
				{
					return false;
				}
			}
			return true;
		}

		private bool ContainsAllElementsLessNicely(IEnumerable<T> other)
		{
			foreach (var t in other)
			{
				if (!containsItem(t))
				{
					return false;
				}
			}
			return true;
		}

		public bool Overlaps(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			

			if (Count == 0)
			{
				return false;
			}

			foreach (T element in other)
			{
				if (Contains(element))
				{
					return true;
				}
			}
			return false;
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			

			// faster if other is a hashset and we're using same equality comparer
			if (other is OrderedHashSet<T> otherAsSet && EqualComparators(this, otherAsSet))
			{
				// attempt to return early: since both contain unique elements, if they have 
				// different counts, then they can't be equal
				if (Count != otherAsSet.Count)
				{
					return false;
				}

				// already confirmed that the sets have the same number of distinct elements, so if
				// one is a superset of the other then they must be equal
				return ContainsAllElementsNicely(otherAsSet);
			}
			else
			{
				if (other is ICollection<T> otherAsCollection)
				{
					// if this count is 0 but other contains at least one element, they can't be equal
					if (Count == 0 && otherAsCollection.Count > 0)
					{
						return false;
					}
				}

				return other.Count() == Count && ContainsAllElementsLessNicely(other);
			}
		}

		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			foreach (T item in other)
			{
				addItem(item);
			}
		}

		#endregion
		#region ISerializable

		private const string COMPARER = "Comparer";
		private const string ELEMENTS = "Elements";
		private const string CAPACITY = "Capacity";

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}
			// need to serialize version to avoid problems with serializing while enumerating

			info.AddValue(COMPARER, lookup.Comparer, typeof(IEqualityComparer<T>));
			if (Count != 0)
			{
				info.AddValue(ELEMENTS, order.ToArray(), typeof(T[]));
			}
			info.AddValue(CAPACITY, capacity);
		}
		#endregion
		#region DeserializationCallback
		public void OnDeserialization(object sender)
		{
			if (_info == null)
			{
				// It might be necessary to call OnDeserialization from a container if the 
				// container object also implements OnDeserialization. However, remoting will 
				// call OnDeserialization again. We can return immediately if this function is 
				// called twice. Note we set m_siInfo to null at the end of this method.
				return;
			}

			int capacity = _info.GetInt32(CAPACITY);

			IEqualityComparer<T> equalityComparer = (IEqualityComparer<T>)_info.GetValue(COMPARER, typeof(IEqualityComparer<T>));
			T[] data = (T[])_info.GetValue(ELEMENTS, typeof(T[]));

			lookup = new Dictionary<T, LinkedListNode<T>>(capacity);
			foreach (var elem in data)
			{
				addItem(elem);
			}
			_info = null;
		}
		#endregion

		#region Enumerator
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IEnumerator
		{
			private OrderedHashSet<T> orderedHashSet;
			private T current;
			private int index;
			private LinkedListNode<T> node;
			private LinkedList<T> list => orderedHashSet.order;

			private const string LINKED_LIST = "LinkedList";
			private const string CURRENT = "Current";
			private const string INDEX = "Index";

			internal Enumerator(OrderedHashSet<T> orderedHashSet)
			{
				this.orderedHashSet = orderedHashSet;
				index = 0;
				current = default;
				node = orderedHashSet.order.First;
			}

			public T Current => current;

			object IEnumerator.Current
			{
				get
				{
					if (index == 0 || index == list.Count + 1)
					{
						throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
					}
					return Current;
				}
			}

			void IDisposable.Dispose()
			{ }

			public bool MoveNext()
			{
				if (node == null)
				{
					index = list.Count + 1;
					return false;
				}

				++index;
				current = node.Value;
				node = node.Next;
				return true;
			}

			void IEnumerator.Reset()
			{
				index = 0;
				current = default;
			}
		}
		#endregion

		#region Helpers
		private bool containsItem(T item)
		{
			return lookup.ContainsKey(item);
		}

		private bool addItem(T item)
		{
			if (lookup.ContainsKey(item))
			{
				return false;
			}
			else
			{
				LinkedListNode<T> node = order.AddLast(item);
				lookup.Add(item, node);
				return true;
			}
		}

		private bool removeItem(T item)
		{
			if (!lookup.ContainsKey(item))
			{
				return false;
			}
			else
			{
				LinkedListNode<T> node = lookup[item];
				lookup.Remove(item);
				order.Remove(node);
				return true;
			}
		}

		private void clearAll()
		{
			lookup.Clear();
			order.Clear();
		}

		protected static bool EqualComparators(OrderedHashSet<T> first, OrderedHashSet<T> second)
		{
			return first.lookup.Comparer.Equals(second.lookup.Comparer);
		}


		#endregion
	}


}
