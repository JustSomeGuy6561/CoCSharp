using CoC.WinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoC.WinDesktop.ContentWrappers
{
	//This class DOES exist on the language change options page, and therefore exists in the scope of a language changed. We should, therefore, worry about languages
	//HOWEVER, this class is the rare exception because all its content is already localized. for all other cases where this exists, the language cannot be changed
	//in the current scope, so it's irrelevant. 
	public sealed class ComboBoxWrapper : NotifierBase
	{
		private readonly ObservableCollection<ComboBoxItemWrapper> itemHolder = new ObservableCollection<ComboBoxItemWrapper>();
		public ReadOnlyObservableCollection<ComboBoxItemWrapper> items { get; }

		public ComboBoxItemWrapper SelectedItem
		{
			get => _selectedItem;
			set
			{
				if (_selectedItem != value)
				{
					_selectedItem = value;
					RaisePropertyChanged();

					//check the flag that signals if we shou
					if (value != null)
					{
						_selectedItem.OnSelect();
					}
				}
			}
		}
		private ComboBoxItemWrapper _selectedItem;

		public ComboBoxWrapper(List<ComboBoxItemWrapper> elements, int? selectedIndex = null)
		{
			if (elements is null) throw new ArgumentNullException(nameof(elements));
			foreach (var elem in elements)
			{
				itemHolder.Add(elem);
			}
			items = new ReadOnlyObservableCollection<ComboBoxItemWrapper>(itemHolder);

			_selectedItem = selectedIndex is null ? null : itemHolder[(int)selectedIndex];
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

	//Normally, I'd be worried about these not being language aware, because their views exist on the language select page and thus need to be language aware.
	//However, these items will be the language itself - "English" is always "English", as is "Deutsch" or "Français", etc. So strings are fine here. 
	public sealed class ComboBoxItemWrapper
	{
		public Action OnSelect { get; }

		public string Title { get; }

		public ComboBoxItemWrapper(Action selectAction, string text)
		{
			OnSelect = selectAction ?? throw new ArgumentNullException(nameof(selectAction));
			Title = text ?? throw new ArgumentNullException(nameof(text));
		}
	}
}
