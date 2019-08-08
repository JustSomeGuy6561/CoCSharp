﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoCWinDesktop.ModelView.Helpers
{
	public sealed class ComboBoxWrapper : INotifyPropertyChanged
	{

		public string Title
		{
			get => _title;
			set
			{
				if (_title != value)
				{
					_title = value;
					OnPropertyChanged();
					Console.WriteLine("Fuck You!");
				}
			}
		}
		private string _title;

		private readonly ObservableCollection<ComboBoxItemWrapper> itemHolder = new ObservableCollection<ComboBoxItemWrapper>();
		public ReadOnlyObservableCollection<ComboBoxItemWrapper> items { get; }
		public event PropertyChangedEventHandler PropertyChanged;

		public ComboBoxItemWrapper SelectedItem
		{
			get => _selectedItem;
			set
			{
				if (_selectedItem != value)
				{
					_selectedItem = value;
					OnPropertyChanged();

					Console.WriteLine("Fuck You Too!");

					//check the flag that signals if we shou
					if (value != null)
					{
						_selectedItem.OnSelect();
					}
				}
			}
		}
		private ComboBoxItemWrapper _selectedItem;

		public int SelectedIndex
		{
			get => _selectedIndex;
			set
			{
				if (_selectedIndex != value)
				{
					_selectedIndex = value;
					OnPropertyChanged();

					////check the flag that signals if we shou
					//if (value != null)
					//{
					//	_selectedIndex.OnSelect();
					//}
					Console.WriteLine(_selectedIndex);
				}
			}
		}
		private int _selectedIndex = 0;


		public ComboBoxWrapper(List<ComboBoxItemWrapper> elements, int? selectedIndex = null)
		{
			if (elements is null) throw new ArgumentNullException(nameof(elements));
			foreach (var elem in elements)
			{
				itemHolder.Add(elem);
			}
			items = new ReadOnlyObservableCollection<ComboBoxItemWrapper>(itemHolder);

			SelectedItem = selectedIndex is null ? null : itemHolder[(int)selectedIndex];
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		//able to use clear, but i'd have to use a for loop to add anyway. might as well just do this shit. 
		public void UpdateCollection(List<ComboBoxItemWrapper> newItems, int? newSelectedIndex = null)
		{
			int shorterListCount = Math.Min(itemHolder.Count, newItems.Count);
			int y;
			//replace what we can.
			for (y = 0; y < shorterListCount; y++)
			{
				itemHolder[y] = newItems[y];
			}
			//if we have more than we can replace, add them.
			if (newItems.Count > y)
			{
				for (; y < newItems.Count; y++)
				{
					itemHolder.Add(newItems[y]);
				}
			}
			//otherwise, if we have more than we replaced, remove them.
			else if (itemHolder.Count > y)
			{
				//reverse the remove order - faster this way. 
				for (int x = itemHolder.Count - 1; x >=y; x--)
				{
					itemHolder.RemoveAt(x);
				}
			}
			//if lists are identical, we're fine

			//update selected index.
			SelectedItem = newSelectedIndex is null ? null : itemHolder[(int)newSelectedIndex];
		}
	}

	public sealed class ComboBoxItemWrapper
	{
		public Action OnSelect { get; }

		public string Title { get; set; }

		public ComboBoxItemWrapper(Action selectAction, string text)
		{
			OnSelect = selectAction ?? throw new ArgumentNullException(nameof(selectAction));
			Title = text ?? throw new ArgumentNullException(nameof(text));
		}
	}
}