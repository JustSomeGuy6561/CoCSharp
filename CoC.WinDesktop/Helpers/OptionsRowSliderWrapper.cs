using CoC.Backend;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

namespace CoCWinDesktop.Helpers
{
	public sealed class OptionsRowSliderWrapper : OptionsRowBase
	{
		//Design structure: if we're doing globals, we're treating that as slider instance -1. 


		private readonly int[] selectedIndexToSettingValue;
		private readonly Dictionary<int?, int> settingValueToSelectedIndex; //used if it's not null.

#warning ToDO: add these to some GUI lookup text thing. 
		private readonly SimpleDescriptor defaultText = () => "default";
		private readonly SimpleDescriptor defaultDesc = () => "No preference. This value will be set on a per-game basis.";

		private int _selectedIndex;

		private readonly bool globalAllowsNull;

		public int maximum => selectedIndexToSettingValue.Length - 1;
		public int minimum => globalAllowsNull && isGlobal ? -1 : 0;

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
					//selectAction(parseSelectedIndex(), displayMode == OptionRowDisplayMode.GLOBAL);
					selectAction(settingValue, isGlobal);
					UpdateDisplay();
				}
			}
		}

		private void UpdateDisplay()
		{

			if (selectedIndex < 0)
			{
				selectedItemText = defaultText();
				selectedItemDescription = defaultDesc();
			}
			else
			{
				selectedItemText = GetOptionText(selectedIndex, isGlobal);
				selectedItemDescription = GetDescriptionText(selectedIndex, isGlobal);
			}
		}

		private int? parseSelectedIndex()
		{
			if (selectedIndex < 0)
			{
				return null;
			}
			return selectedIndexToSettingValue[selectedIndex];
		}


		private readonly Func<int, bool, string> GetOptionText;
		private readonly Func<int, bool, string> GetDescriptionText;

		private readonly Action<int?, bool> selectAction;
		private readonly Func<bool, int?> queryStatus;


		public OptionsRowSliderWrapper(SimpleDescriptor optionName, OrderedHashSet<int> allOptions, Func<int, bool, string> availableOptions,
			Func<int, bool, string> optionDescriptions, Func<bool, int?> getStatus, Action<int?, bool> onSelect,
			bool allowsNullOnGlobal = true) : base(optionName)
		/*OptionRowDisplayMode defaultMode = OptionRowDisplayMode.GAME) : base(optionName, defaultMode)*/
		{
			if (allOptions is null) throw new ArgumentNullException(nameof(allOptions));
			if (availableOptions is null) throw new ArgumentNullException(nameof(availableOptions));
			if (optionDescriptions is null) throw new ArgumentNullException(nameof(optionDescriptions));

			int count = allOptions.Count;

			globalAllowsNull = allowsNullOnGlobal;

			selectedIndexToSettingValue = new int[count];
			settingValueToSelectedIndex = new Dictionary<int?, int>();

			int iteratation = 0;
			foreach (int x in allOptions)
			{
				settingValueToSelectedIndex.Add(x, iteratation);
				selectedIndexToSettingValue[iteratation] = x;
			}

			GetOptionText = availableOptions ?? throw new ArgumentNullException(nameof(availableOptions));
			GetDescriptionText = optionDescriptions ?? throw new ArgumentNullException(nameof(optionDescriptions));
			selectAction = onSelect ?? throw new ArgumentNullException(nameof(onSelect));
			queryStatus = getStatus ?? throw new ArgumentNullException(nameof(getStatus));

			_selectedIndex = queryStatus(isGlobal) ?? -1;
			UpdateDisplay();
		}

		//protected override void OnOptionStatusChanged(OptionRowDisplayMode oldMode)
		//{
		//	if (isGlobal != (oldMode == OptionRowDisplayMode.GLOBAL))
		//	{
		//		RaisePropertyChanged(nameof(numItems));

		//		if (isGlobal)
		//		{
		//			selectedIndex = (queryStatus(isGlobal) + 1) ?? 0;
		//		}
		//		else
		//		{
		//			selectedIndex = queryStatus(isGlobal) ?? 0;
		//		}
		//	}
		//}

		protected override void OnStatusChanged()
		{
			if (globalAllowsNull)
			{
				RaisePropertyChanged(nameof(minimum));
			}

			selectedIndex = queryStatus(isGlobal) ?? (globalAllowsNull ? -1 : 0);
		}
	}
}
