using CoC.Backend;
using CoC.WinDesktop.ContentWrappers.ButtonWrappers;
using System;
using System.Diagnostics;
using System.Windows;

namespace CoC.WinDesktop.ContentWrappers.OptionsRow
{
	public sealed class OptionsRowButtonWrapper : OptionsRowWrapperBase
	{
		private readonly Action<bool?> setter;
		private readonly Func<bool?> getter;
		private readonly bool nullable;
		private readonly EnabledOrDisabledWithToolTipNullBtn enabledFnGetter;

		public string currentDescription => GetDescriptionText(currentStatus);
		private bool? currentStatus => getter();

		private readonly Func<bool?, string> GetDescriptionText;

		public AutomaticButtonWrapper enabledButton { get; }
		public AutomaticButtonWrapper disabledButton { get; }
		public AutomaticButtonWrapper clearedButton { get; }

		public string WarningText => warningTextFn()?.Trim();
		private readonly SimpleDescriptor warningTextFn;

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool?> SetState, Func<bool?> GetState, Func<bool?, string> GetText,
			Func<bool?, string> GetHint, SimpleDescriptor warningText, EnabledOrDisabledWithToolTipNullBtn disabledWithTooltipGetter)
			: this(optionName, SetState, GetState, GetText, GetHint, warningText, disabledWithTooltipGetter, true)
		{

		}

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool> SetState, Func<bool> GetState, Func<bool, string> GetText,
			Func<bool, string> GetHint, SimpleDescriptor warningText, EnabledOrDisabledWithToolTipBtn disabledWithTooltipGetter) : this(optionName,
			x => SetState(x ?? false), () => GetState(), x => GetText(x ?? false), x => GetHint(x ?? false), warningText, convertDelegate(disabledWithTooltipGetter), false)
		{

		}

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool?> SetState, Func<bool?> GetState, SimpleDescriptor enabledText,
			SimpleDescriptor disabledText, SimpleDescriptor nullText, SimpleDescriptor enabledHint, SimpleDescriptor disabledHint, SimpleDescriptor nullHint,
			SimpleDescriptor warningText, EnabledOrDisabledWithToolTipNullBtn disabledWithTooltipGetter) : this(optionName, SetState, GetState,
			ConvertToFunc(enabledText, disabledText, nullText), ConvertToFunc(enabledHint, disabledHint, nullHint), warningText, disabledWithTooltipGetter, true)
		{

		}

		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool> SetState, Func<bool> GetState, SimpleDescriptor enabledText,
			SimpleDescriptor disabledText, SimpleDescriptor enabledHint, SimpleDescriptor disabledHint, SimpleDescriptor warningText,
			EnabledOrDisabledWithToolTipBtn disabledWithTooltipGetter) : this(optionName, x=> SetState(x??false), () => GetState(),
			ConvertToFunc(enabledText, disabledText), ConvertToFunc(enabledHint, disabledHint), warningText, convertDelegate(disabledWithTooltipGetter), false)
		{

		}

		private static Func<bool?, string> ConvertToFunc(SimpleDescriptor enabled, SimpleDescriptor disabled, SimpleDescriptor cleared)
		{
			return x => x == true ? enabled() : x == false ? disabled() : cleared();
		}

		private static Func<bool?, string> ConvertToFunc(SimpleDescriptor enabled, SimpleDescriptor disabled)
		{
			return x => x == true ? enabled() : disabled();
		}


		private OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool?> SetState, Func<bool?> GetState, Func<bool?, string> GetText,
		Func<bool?, string> GetHint, SimpleDescriptor warningText, EnabledOrDisabledWithToolTipNullBtn disabledWithTooltipGetter, bool allowsNull) : base(optionName)
		{
			getter = GetState ?? throw new ArgumentNullException(nameof(GetState));
			setter = SetState ?? throw new ArgumentNullException(nameof(SetState));
			if (GetText is null) throw new ArgumentNullException(nameof(GetText));

			GetDescriptionText = GetHint ?? throw new ArgumentNullException(nameof(GetHint));
			warningTextFn = warningText;

			enabledFnGetter = disabledWithTooltipGetter ?? throw new ArgumentNullException(nameof(disabledWithTooltipGetter));

			string getData(bool? target, bool enabled)
			{
				//if enabled FnGetter returns true, we ignore the output.
				return enabledFnGetter(target, out string data) ? null : data;
			}
			nullable = allowsNull;

			enabledButton = new AutomaticButtonWrapper(() => GetText(true), () => SetStatus(true), (x) => getData(true, x), null, GetState() != true, false);
			if (allowsNull)
			{
				clearedButton = new AutomaticButtonWrapper(() => GetText(null), () => SetStatus(null), (x) => getData(null, x), null, GetState() != null, false);
			}
			else
			{
				clearedButton = new AutomaticButtonWrapper(() => "N/A", () => { }, tipCallback:null, null, false, false);
				clearedButton.SetVisibility(Visibility.Collapsed);
			}
			disabledButton = new AutomaticButtonWrapper(() => GetText(false), () => SetStatus(false), (x) => getData(false, x), null, GetState() != false, false);
		}

		private static EnabledOrDisabledWithToolTipNullBtn convertDelegate(EnabledOrDisabledWithToolTipBtn source)
		{
			bool converter(bool? status, out string target)
			{
				return source(status ?? false, out target);
			}
			return converter;
		}

		private void SetStatus(bool? status)
		{
			if (currentStatus != status)
			{
				if (currentStatus == true)
				{
					enabledButton.SetEnabled(true);
				}
				else if (currentStatus == null && nullable)
				{
					clearedButton.SetEnabled(true);
				}
				else
				{
					disabledButton.SetEnabled(true);
				}

				if (status == true)
				{
					enabledButton.SetEnabled(false);
				}
				else if (status == null && nullable)
				{
					clearedButton.SetEnabled(false);
				}
				else
				{
					disabledButton.SetEnabled(false);
				}
				if (!nullable && status == null)
				{
					Debug.WriteLine("Something Broke with displaying options buttons - null button was clicked but null not allowed.");
					SetStatus(false);
				}
				setter(status);

				base.RaisePropertyChanged(nameof(currentDescription));
			}
		}
	}
}
