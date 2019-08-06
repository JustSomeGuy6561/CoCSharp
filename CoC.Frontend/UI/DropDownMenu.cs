//DropDownMenu.cs
//Description:
//Author: JustSomeGuy
//6/19/2019, 9:32 PM
using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Frontend.UI
{

	public sealed class DropDownMenu
	{

		private bool postContentChanged;
		private bool statusChanged;

		public bool active { get; private set; } = false;

		public ReadOnlyCollection<DropDownEntry> entries { get; }
		private readonly List<DropDownEntry> entryHolder;

		internal static void SetPostDropDownMenuText(SimpleDescriptor text)
		{
			if (!instance.active)
			{
				return;
			}
			else
			{
				instance.selectPostText = text;
				instance.postContentChanged = true;
			}
		}

		private SimpleDescriptor selectPostText = null;

		internal static void ClearData()
		{
			instance.Clear();
		}

		private void Clear()
		{
			if (entryHolder.Count != 0)
			{
				statusChanged = true;
			}
			entryHolder.Clear();
			if (selectPostText != null)
			{
				postContentChanged = true;
			}
			selectPostText = null;
		}
		private void UpdateInputField(DropDownEntry[] entries)
		{
			entryHolder.Clear();
			entryHolder.AddRange(entries);
		}


		private DropDownMenu(DropDownEntry[] selectableEntries)
		{
			entryHolder = new List<DropDownEntry>(selectableEntries);
			entries = new ReadOnlyCollection<DropDownEntry>(entryHolder);
		}

		internal static bool ActivateDropDownMenu(DropDownEntry[] entries)
		{
			bool retVal = !instance.active;
			instance.active = true;
			instance.UpdateInputField(entries);

			instance.statusChanged = true;

			return retVal;
		}

		internal static bool DeactivateDropDownMenu()
		{
			if (instance.active == false)
			{
				return false;
			}
			instance.active = false;

			instance.Clear();
			return true;
		}

		private static readonly DropDownMenu instance = new DropDownMenu(new DropDownEntry[0]);
		private string postText => selectPostText?.Invoke() ?? "";

		internal static bool QueryStatus(out DropDownMenu dropDownMenu)
		{
			bool retVal = instance.statusChanged;
			dropDownMenu = instance;
			instance.statusChanged = false;
			return retVal;
		}

		internal static bool QueryPostText(out string postControlText)
		{
			bool retVal = instance.postContentChanged;
			postControlText = instance.postText;
			instance.postContentChanged = false;
			return retVal;
		}
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
