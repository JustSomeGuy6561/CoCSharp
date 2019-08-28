using CoC.Backend;
using System;

namespace CoCWinDesktop.Helpers
{
	public sealed class OptionsRowButtonWrapper : OptionsRowBase
	{
		private readonly Action<bool?> setter;
		private readonly Func<bool?> getter;
		private readonly bool nullable;
		private readonly EnabledOrDisabledWithToolTipNullBtn enabledFnGetter;
		private readonly SimpleDescriptor warningTextFn;

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool?> SetState, Func<bool?> GetState, Func<bool?, string> GetText,
			Func<bool?, string> GetHint, SimpleDescriptor warningText, EnabledOrDisabledWithToolTipNullBtn disabledWithTooltipGetter) : base(optionName)
		{
			if (GetText is null) throw new ArgumentNullException(nameof(GetText));
			string enabledText() => GetText(true);
			string disabledText() => GetText(false);
			string nullText() => GetText(null);

			DoConstructor(out setter, out getter, out nullable, out _enabledBtn, out _disabledBtn, out _clearBtn, out GetDescriptionText, out warningTextFn,
				out enabledFnGetter, SetState, GetState, enabledText, disabledText, nullText, GetHint, warningText, disabledWithTooltipGetter);
			DoInit(out _enableCommand, out _disableCommand, out _clearCommand);
		}

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool> SetState, Func<bool> GetState, Func<bool, string> GetText,
			Func<bool, string> GetHint, SimpleDescriptor warningText, EnabledOrDisabledWithToolTipBtn disabledWithTooltipGetter) : base(optionName)
		{
			if (GetText is null) throw new ArgumentNullException(nameof(GetText));
			string enabledText() => GetText(true);
			string disabledText() => GetText(false);

			DoConstructor(out setter, out getter, out nullable, out _enabledBtn, out _disabledBtn, out _clearBtn, out GetDescriptionText, out warningTextFn,
				out enabledFnGetter, SetState, GetState, enabledText, disabledText, GetHint, warningText, disabledWithTooltipGetter);
			DoInit(out _enableCommand, out _disableCommand, out _clearCommand);
		}

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool?> SetState, Func<bool?> GetState, SimpleDescriptor enabledText,
			SimpleDescriptor disabledText, SimpleDescriptor nullText, SimpleDescriptor enabledHint, SimpleDescriptor disabledHint, SimpleDescriptor nullHint,
			SimpleDescriptor warningText, EnabledOrDisabledWithToolTipNullBtn disabledWithTooltipGetter) : base(optionName)
		{
			if (enabledHint is null) throw new ArgumentNullException(nameof(enabledHint));
			if (disabledHint is null) throw new ArgumentNullException(nameof(disabledHint));
			if (nullHint is null) throw new ArgumentNullException(nameof(nullHint));

			string hintGetter(bool? x) => x == true ? enabledHint() : x == false ? disabledHint() : nullHint();

			DoConstructor(out setter, out getter, out nullable, out _enabledBtn, out _disabledBtn, out _clearBtn, out GetDescriptionText, out warningTextFn,
				out enabledFnGetter, SetState, GetState, enabledText, disabledText, nullText, hintGetter, warningText, disabledWithTooltipGetter);
			DoInit(out _enableCommand, out _disableCommand, out _clearCommand);
		}

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool> SetState, Func<bool> GetState,
			SimpleDescriptor enabledText, SimpleDescriptor disabledText, Func<string> enabledHint, Func<string> disabledHint,
			SimpleDescriptor warningText, EnabledOrDisabledWithToolTipBtn disabledWithTooltipGetter) : base(optionName)
		{
			if (enabledHint == null) throw new ArgumentNullException(nameof(enabledHint));
			if (disabledHint == null) throw new ArgumentNullException(nameof(disabledHint));

			string hintGetter(bool x) => x ? enabledHint() : disabledHint();

			DoConstructor(out setter, out getter, out nullable, out _enabledBtn, out _disabledBtn, out _clearBtn, out GetDescriptionText, out warningTextFn,
				out enabledFnGetter, SetState, GetState, enabledText, disabledText, hintGetter, warningText, disabledWithTooltipGetter);
			DoInit(out _enableCommand, out _disableCommand, out _clearCommand);
		}

		private void DoConstructor(out Action<bool?> mutator, out Func<bool?> accessor, out bool isNull, out string enabled, out string disabled, 
			out string empty, out Func<bool?, string> description, out SimpleDescriptor warningText, out EnabledOrDisabledWithToolTipNullBtn disabledTooltip, 
			Action<bool?> set, Func<bool?> get, SimpleDescriptor enabledText, SimpleDescriptor disabledText, SimpleDescriptor emptyText, 
			Func<bool?, string> getHint, SimpleDescriptor warning, EnabledOrDisabledWithToolTipNullBtn tooltip)
		{
			isNull = true;

			if (set is null) throw new ArgumentNullException(nameof(set));
			if (get is null) throw new ArgumentNullException(nameof(get));
			mutator = set;
			accessor = get;

			if (enabledText is null) throw new ArgumentNullException(nameof(enabledText));
			if (disabledText is null) throw new ArgumentNullException(nameof(disabledText));
			if (emptyText is null) throw new ArgumentNullException(nameof(emptyText));
			enabled = enabledText();
			disabled = disabledText();
			empty = emptyText();

			description = getHint ?? throw new ArgumentNullException(nameof(getHint));

			warningText = warning ?? throw new ArgumentException(nameof(warning));
			disabledTooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
		}

		private void DoConstructor(out Action<bool?> mutator, out Func<bool?> accessor, out bool isNull, out string enabled, out string disabled, out string empty,
			out Func<bool?, string> description, out SimpleDescriptor warningText, out EnabledOrDisabledWithToolTipNullBtn disabledTooltip, Action<bool> set, Func<bool> get,
			SimpleDescriptor enabledText, SimpleDescriptor disabledText, Func<bool, string> getHint, SimpleDescriptor warning, EnabledOrDisabledWithToolTipBtn tooltip)
		{
			isNull = false;

			if (set is null) throw new ArgumentNullException(nameof(set));
			if (get is null) throw new ArgumentNullException(nameof(get));
			mutator = x => set((bool)x);
			accessor = () => get();

			if (enabledText is null) throw new ArgumentNullException(nameof(enabledText));
			if (disabledText is null) throw new ArgumentNullException(nameof(disabledText));

			enabled = enabledText();
			disabled = disabledText();
			empty = "";

			if (getHint is null) throw new ArgumentNullException(nameof(getHint));

			if (tooltip is null) throw new ArgumentNullException(nameof(tooltip));

			warningText = warning ?? throw new ArgumentException(nameof(warning));

			bool wrapper(bool? x, out string y) => tooltip((bool)x, out y);

			disabledTooltip = wrapper;

			description = (x) => x != null ? getHint((bool)x) : null;
		}

		private void DoInit(out RelayCommand enblCmd, out RelayCommand dsblCmd, out RelayCommand clrCmd)
		{
			_enabled = getter();

			enblCmd = new RelayCommand(() => enabled = true, canEnable);
			dsblCmd = new RelayCommand(() => enabled = false, canDisable);
			clrCmd = new RelayCommand(() => enabled = null, canClear);

			UpdateSelectedHintText();
		}

		private bool canEnable()
		{
			return enabled != true;
		}

		private bool canDisable()
		{
			return enabled != false;
		}

		private bool canClear()
		{
			return nullable && enabled != null;
		}

		public bool ClearVisible => nullable;

		private bool? enabled
		{
			get => _enabled;
			set
			{
				if (_enabled != value)
				{
					var oldValue = _enabled;
					_enabled = value;

					if (oldValue == true)
					{
						EnableCommand.RaiseExecuteChanged();
					}
					else if (oldValue == null)
					{
						ClearCommand.RaiseExecuteChanged();
					}
					else
					{
						DisableCommand.RaiseExecuteChanged();
					}

					if (value == true)
					{
						EnableCommand.RaiseExecuteChanged();
					}
					else if (value == null)
					{
						ClearCommand.RaiseExecuteChanged();
					}
					else
					{
						DisableCommand.RaiseExecuteChanged();
					}

					setter(_enabled);
					UpdateSelectedHintText();
				}
			}
		}

		private void UpdateSelectedHintText()
		{
			currentDescription = GetDescriptionText(enabled);

			EnabledBtnTooltip = !enabledFnGetter(true, out string temp) ? temp : null;
			ClearBtnTooltip = !enabledFnGetter(null, out temp) ? temp : null;
			DisabledBtnTooltip = !enabledFnGetter(false, out temp) ? temp : null;

			WarningText = warningTextFn();
		}

		private bool? _enabled;

		//global visible is via isglobal, with bool to collapsed.

		public RelayCommand EnableCommand => _enableCommand;
		private readonly RelayCommand _enableCommand;

		public RelayCommand DisableCommand => _disableCommand;
		private readonly RelayCommand _disableCommand;

		public RelayCommand ClearCommand => _clearCommand;
		private readonly RelayCommand _clearCommand;

		private readonly Func<bool?, string> GetDescriptionText;

		public string currentDescription
		{
			get => _selectedItemDescription;
			private set => CheckPropertyChanged(ref _selectedItemDescription, value);
		}
		private string _selectedItemDescription;

		public string EnabledBtn => _enabledBtn;
		private readonly string _enabledBtn;
		public string DisabledBtn => _disabledBtn;
		private readonly string _disabledBtn;
		public string ClearBtn => _clearBtn;
		private readonly string _clearBtn;

		public string EnabledBtnTooltip
		{
			get => _enabledBtnTooltip;
			private set => CheckPropertyChanged(ref _enabledBtnTooltip, value);
		}
		private string _enabledBtnTooltip;

		public string DisabledBtnTooltip
		{
			get => _DisabledBtnTooltip;
			private set => CheckPropertyChanged(ref _DisabledBtnTooltip, value);
		}
		private string _DisabledBtnTooltip;

		public string ClearBtnTooltip
		{
			get => _ClearBtnTooltip;
			private set => CheckPropertyChanged(ref _ClearBtnTooltip, value);
		}
		private string _ClearBtnTooltip;

		public string WarningText
		{
			get => _WarningText;
			private set => CheckPropertyChanged(ref _WarningText, value);
		}
		private string _WarningText;
	}
}
