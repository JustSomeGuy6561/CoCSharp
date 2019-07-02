//DropDownMenu.cs
//Description:
//Author: JustSomeGuy
//6/19/2019, 9:32 PM
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Frontend.UI
{
	public sealed class DropDownMenu
	{
		public ReadOnlyCollection<DropDownEntry> entries;

		private DropDownMenu(DropDownEntry[] selectableEntries)
		{
			entries = new ReadOnlyCollection<DropDownEntry>(selectableEntries);
		}

		internal static bool ActivateDropDownMenu(DropDownEntry[] entries)
		{
			if (instance != null)
			{
				return false;
			}
			instance = new DropDownMenu(entries);
			return true;
		}

		internal static bool DeactivateDropDownMenu()
		{
			if (instance == null)
			{
				return false;
			}
			instance = null;
			return true;
		}

		internal static DropDownMenu instance;
	}

	public sealed class DropDownEntry
	{
		public readonly string title;
		public readonly Action onSelect;

		internal DropDownEntry(string label, Action callback)
		{
			title = label;
			onSelect = callback;
		}
	}
}
