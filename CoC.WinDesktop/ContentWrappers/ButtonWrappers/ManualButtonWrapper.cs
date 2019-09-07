using CoCWinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoCWinDesktop.ContentWrappers.ButtonWrappers
{
	public sealed class ManualButtonWrapper : ButtonWrapperBase
	{
		public override ICommand ClickCommand { get; }

		private Action onClick
		{
			get => _onClick;
			set
			{
				if (_onClick != value)
				{
					bool wasNull = _onClick is null;
					_onClick = value; //needs to update first because otherwise OnClickCommand fails to change properly

					if (wasNull != value is null)
					{
						((RelayCommand)ClickCommand).RaiseExecuteChanged();
					}
				}
			}
		}
		private Action _onClick;

		public override string Title
		{
			get => _title;
		}
		private string titleSetter
		{
			set => CheckPropertyChanged(ref _title, value, nameof(Title));
		}
		private string _title;

		public override ToolTipWrapper ToolTip
		{
			get => _toolTip;
		}
		private ToolTipWrapper toolTipSetter
		{ 
			set => CheckPropertyChanged(ref _toolTip, value, nameof(ToolTip));
		}
		private ToolTipWrapper _toolTip = null;

		public override bool IsDefault
		{
			get => _isDefault;
		}
		private bool isDefaultSetter
		{ 
			set => CheckPrimitivePropertyChanged(ref _isDefault, value, nameof(IsDefault));
		}
		private bool _isDefault;

		public ManualButtonWrapper()
		{
			ClickCommand = new RelayCommand(OnClickCommand, CanClick);
		}

		private void OnClickCommand()
		{
			onClick?.Invoke();
		}

		private bool CanClick()
		{
			return onClick != null;
		}

		public void UpdateButtonEnabled(Action callback, bool defaultButton, string title, string tip = null, string tipTitle = null)
		{
			visibility = Visibility.Visible;
			_title = title ?? throw new ArgumentNullException(nameof(title));
			if (string.IsNullOrWhiteSpace(Title))
			{
				throw new ArgumentException("Title for a button cannot be empty");
			}

			tipTitle = string.IsNullOrWhiteSpace(tip) ? null : (!string.IsNullOrWhiteSpace(tipTitle) ? tipTitle : Title);

			SetToolTip(tipTitle, tip);

			_isDefault = defaultButton;
			onClick = callback;
		}

		public void UpdateButtonDisabled(string title, string tip = null, string tipTitle = null)
		{
			visibility = Visibility.Visible;
			titleSetter = title ?? throw new ArgumentNullException(nameof(title));
			if (string.IsNullOrWhiteSpace(Title))
			{
				throw new ArgumentException("Title for a button cannot be empty");
			}
			tipTitle = string.IsNullOrWhiteSpace(tip) ? null : (!string.IsNullOrWhiteSpace(tipTitle) ? tipTitle : Title);

			SetToolTip(tipTitle, tip);

			isDefaultSetter = false;
			onClick = null;
		}

		private void SetToolTip(string header, string tip)
		{
			if (string.IsNullOrWhiteSpace(header))
			{
				toolTipSetter = null;
			}
			else
			{
				toolTipSetter = new ToolTipWrapper(header, tip);
			}
		}

		public void UpdateButtonHidden()
		{
			visibility = Visibility.Hidden;
			isDefaultSetter = false;
			onClick = null;
		}
	}
}
