using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Backend;

namespace CoCWinDesktop.Helpers
{
	public sealed class OptionsRowButtonWrapper : OptionsRowBase
	{
		public OptionsRowButtonWrapper(SimpleDescriptor optionName, Action<bool?, bool> SetState, Func<bool, bool?> GetState,
			SimpleDescriptor enabledText, SimpleDescriptor disabledText, Func<bool, string> enabledHint, Func<bool, string> disabledHint, 
			bool allowsNullOnGlobal = true) : base(optionName)
		{
			selectAction = SetState ?? throw new ArgumentNullException(nameof(SetState));
			queryStatus = GetState ?? throw new ArgumentNullException(nameof(GetState));

			if (enabledText == null) throw new ArgumentNullException(nameof(enabledText));
			if (disabledText == null) throw new ArgumentNullException(nameof(disabledText));

			if (enabledHint == null) throw new ArgumentNullException(nameof(enabledHint));
			if (disabledHint == null) throw new ArgumentNullException(nameof(disabledHint));

			EnabledBtn = enabledText();
			DisabledBtn = disabledText();

			globalAllowsNullable = allowsNullOnGlobal;

			GetDescriptionText = (x, y) => x ? enabledHint(y) : disabledHint(y);

			_enabled = GetState(isGlobal);

			EnableCommand = new RelayCommand(() => enabled = true, canEnable);
			DisableCommand = new RelayCommand(() => enabled = false, canDisable);
			ClearCommand = new RelayCommand(() => enabled = null, canClear);

			UpdateSelectedHintText();
		}

		private readonly bool globalAllowsNullable;

		private bool canEnable()
		{
			return enabled !=true;
		}

		private bool canDisable()
		{
			return enabled != false;
		}

		private bool canClear()
		{
			return isGlobal && globalAllowsNullable && enabled != null;
		}

		public bool ClearVisible => globalAllowsNullable && isGlobal;

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

					selectAction(_enabled, isGlobal);
					UpdateSelectedHintText();
				}
			}
		}

		private readonly SimpleDescriptor defaultDesc = () => "No preference. This value will be set on a per-game basis.";

		private void UpdateSelectedHintText()
		{
			if (enabled is null)
			{
				currentDescription = defaultDesc();
			}
			else
			{
				currentDescription = GetDescriptionText((bool)enabled, isGlobal);
			}

		}

		private bool? _enabled;

		//global visible is via isglobal, with bool to collapsed.

		public RelayCommand EnableCommand { get; }
		public RelayCommand DisableCommand { get; }
		public RelayCommand ClearCommand { get; }

		private readonly Func<bool, bool, string> GetDescriptionText;

		public string currentDescription
		{
			get => _selectedItemDescription;
			private set => CheckPropertyChanged(ref _selectedItemDescription, value);
		}
		private string _selectedItemDescription;

		public string EnabledBtn
		{
			get;
		}
		public string DisabledBtn
		{
			get;
		}

		protected override void OnStatusChanged()
		{
			if (globalAllowsNullable)
			{
				ClearCommand.RaiseExecuteChanged();

				RaisePropertyChanged(nameof(ClearVisible));
			}

			enabled = queryStatus(isGlobal);
		}

		private readonly Action<bool?, bool> selectAction;
		private readonly Func<bool, bool?> queryStatus;
	}
}
