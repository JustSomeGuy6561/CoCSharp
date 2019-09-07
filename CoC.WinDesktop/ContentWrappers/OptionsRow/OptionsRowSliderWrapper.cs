using CoC.Backend;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoCWinDesktop.ContentWrappers.OptionsRow
{
	public sealed class OptionsRowSliderWrapper : OptionsRowWrapperBase
	{
		//Design structure: nullable option sets minimum to -1, and treats -1 as null. The slider is setup accordingly


		private readonly int[] selectedIndexToSettingValue;
		private readonly Dictionary<int?, int> settingValueToSelectedIndex; //used if it's not null.

		private readonly Func<int?, string> GetOptionText;
		private readonly Func<int?, string> GetDescriptionText;

		private readonly Action<int?> setter;
		private readonly Func<int?> getter;

		private readonly EnabledOrDisabledWithTollTipNullSldr disabledTooltipFn;
		private readonly bool nullable;

		public int maximum => selectedIndexToSettingValue.Length - 1;
		public int minimum => nullable ? -1 : 0;

		public string selectedItemText
		{
			get => _selectedItemText;
			private set => CheckPropertyChanged(ref _selectedItemText, value);
		}
		private string _selectedItemText;

		public string selectedItemDescription
		{
			get => _selectedItemDescription;
			private set => CheckPropertyChanged(ref _selectedItemDescription, value);
		}
		private string _selectedItemDescription;

		public int selectedIndex
		{
			get => _selectedIndex;
			set
			{
				if (CheckPrimitivePropertyChanged(ref _selectedIndex, value))
				{
					int? settingValue = parseSelectedIndex();
					setter(settingValue);
					UpdateDisplay();
				}
			}
		}
		private int _selectedIndex;
		private readonly SimpleDescriptor warningTextFn;

		public string WarningText
		{
			get => _warningText;
			private set => CheckPropertyChanged(ref _warningText, value);
		}
		private string _warningText;

		public ObservableCollection<int> Ticks { get; }

		public OptionsRowSliderWrapper(SimpleDescriptor optionName, OrderedHashSet<int?> allOptions, Func<int?, string> availableOptions,
			Func<int?, string> optionDescriptions, Func<int?> getStatus, Action<int?> onSelect, SimpleDescriptor warningTextGetter, 
			EnabledOrDisabledWithTollTipNullSldr disabledTooltipGetter) : base(optionName)
		{
			nullable = true;

			if (allOptions is null) throw new ArgumentNullException(nameof(allOptions));
			int count = allOptions.Count;
			selectedIndexToSettingValue = new int[count];
			settingValueToSelectedIndex = new Dictionary<int?, int>();

			int iteratation = 0;
			foreach (int x in allOptions)
			{
				settingValueToSelectedIndex.Add(x, iteratation);
				selectedIndexToSettingValue[iteratation] = x;
				iteratation++;
			}

			GetOptionText = availableOptions ?? throw new ArgumentNullException(nameof(availableOptions));
			GetDescriptionText = optionDescriptions ?? throw new ArgumentNullException(nameof(optionDescriptions));

			disabledTooltipFn = disabledTooltipGetter ?? throw new ArgumentNullException(nameof(disabledTooltipGetter));

			setter = onSelect ?? throw new ArgumentNullException(nameof(onSelect));
			getter = getStatus ?? throw new ArgumentNullException(nameof(getStatus));

			warningTextFn = warningTextGetter ?? throw new ArgumentNullException(nameof(warningTextGetter));

			_selectedIndex = querySelectedIndex();
			UpdateDisplay();
		}

		public OptionsRowSliderWrapper(SimpleDescriptor optionName, OrderedHashSet<int> allOptions, Func<int, string> availableOptions,
			Func<int, string> optionDescriptions, Func<int> getStatus, Action<int> onSelect, 
			SimpleDescriptor warningTextGetter, EnabledOrDisabledWithToolTipSldr disabledTooltipGetter) : base(optionName)
		{
			nullable = false;

			if (allOptions is null) throw new ArgumentNullException(nameof(allOptions));
			int count = allOptions.Count;
			selectedIndexToSettingValue = new int[count];
			settingValueToSelectedIndex = new Dictionary<int?, int>();

			int iteratation = 0;
			foreach (int x in allOptions)
			{
				settingValueToSelectedIndex.Add(x, iteratation);
				selectedIndexToSettingValue[iteratation] = x;
				iteratation++;
			}

			if (availableOptions is null) throw new ArgumentNullException(nameof(availableOptions));
			if (optionDescriptions is null) throw new ArgumentNullException(nameof(optionDescriptions));

			GetOptionText = (x) => availableOptions((int)x);
			GetDescriptionText = (x) => optionDescriptions((int)x);

			if (onSelect is null) throw new ArgumentNullException(nameof(onSelect));
			if (getStatus is null) throw new ArgumentNullException(nameof(getStatus));

			setter = (x) => onSelect(x ?? throw new ArgumentException("Should Never Occur, as setting globalAllowsNullPrevents this"));
			getter = () => getStatus();

			warningTextFn = warningTextGetter ?? throw new ArgumentNullException(nameof(warningTextGetter));

			_selectedIndex = querySelectedIndex();
			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			int? selection = parseSelectedIndex();

			selectedItemText = GetOptionText(selection);
			selectedItemDescription = GetDescriptionText(selection);
			WarningText = warningTextFn();

			#warning Handle the Ticks and tooltip - whatever is disabled, remove the corresponding index from the Ticks collection. Update the tooltip accordingly. 
		}

		private int? parseSelectedIndex()
		{
			if (selectedIndex < 0)
			{
				return null;
			}
			return selectedIndexToSettingValue[selectedIndex];
		}

		private int querySelectedIndex()
		{
			int? selectedValue = getter();
			if (settingValueToSelectedIndex.TryGetValue(selectedValue, out int result))
			{
				return result;
			}
			else if (nullable)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}


	}
}
