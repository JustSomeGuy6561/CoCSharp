//DropDownMenu.cs
//Description:
//Author: JustSomeGuy
//6/19/2019, 9:32 PM
using CoC.Backend;
using CoC.Backend.Strings;
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

		public readonly ReadOnlyCollection<DropDownEntry> entries;
		private readonly List<DropDownEntry> entryHolder;

		public DropDownEntry defaultEntry { get; private set; }


		internal void SetPostDropDownMenuText(SimpleDescriptor text)
		{
			if (!active)
			{
				return;
			}
			else
			{
				selectPostText = text;
				postContentChanged = true;
			}
		}

		private SimpleDescriptor selectPostText = null;

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


		internal DropDownMenu()
		{
			entryHolder = new List<DropDownEntry>();
			entries = new ReadOnlyCollection<DropDownEntry>(entryHolder);
		}

		internal bool ActivateDropDownMenu(DropDownEntry[] entries)
		{
			bool retVal = !active;
			active = true;
			UpdateInputField(entries);

			statusChanged = true;

			return retVal;
		}

		internal bool DeactivateDropDownMenu()
		{
			if (active == false)
			{
				return false;
			}
			active = false;

			Clear();
			return true;
		}

		private string postText => selectPostText?.Invoke() ?? "";

		internal bool QueryStatus()
		{
			bool retVal = statusChanged;
			statusChanged = false;
			return retVal;
		}

		internal bool QueryPostText(out string postControlText)
		{
			bool retVal = postContentChanged;
			postControlText = postText;
			postContentChanged = false;
			return retVal;
		}
	}

	public sealed class DropDownEntry
	{
		public readonly string title;
		//NOTE TO GUI DEVS: onSelect can be null, for separators. if you don't support them, make sure to remove
		//all entries where
		public readonly Action onSelect;

		internal DropDownEntry(string label, Action callback)
		{
			title = label ?? throw new ArgumentNullException(nameof(label));
			if (string.IsNullOrWhiteSpace(label)) throw new ArgumentException("cannot have empty text");
			onSelect = callback ?? throw new ArgumentNullException(nameof(callback), "note: if you were trying to create a separator, use CreateSeparator instead");
		}

		private DropDownEntry(string label)
		{
			title = label;
			onSelect = null;
		}

		//A separator is a special type of drop down entry that cannot be selected. it allows the dev to separate entries into
		//groups, if desired.
		internal static DropDownEntry CreateSeparator(string separatorText)
		{
			return new DropDownEntry(separatorText);
		}
	}
}
