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

namespace CoCWinDesktop.Helpers
{
	public sealed class BottomButtonWrapper : NotifierBase
	{
		public ICommand ClickCommand { get; }

		public Visibility visibility
		{
			get => _visibility;
			private set => CheckEnumPropertyChanged(ref _visibility, value);
		}
		private Visibility _visibility = Visibility.Hidden;

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

		public string Title
		{
			get => _title;
			private set => CheckPropertyChanged(ref _title, value);
		}
		private string _title;

		public string ToolTip
		{
			get => _toolTip;
			private set => CheckPropertyChanged(ref _toolTip, value);
		}
		private string _toolTip;

		public string ToolTipTitle
		{
			get => _toolTipTitle;
			private set => CheckPropertyChanged(ref _toolTipTitle, value);
		}
		private string _toolTipTitle;

		public bool isDefault
		{
			get => _isDefault;
			private set => CheckPrimitivePropertyChanged(ref _isDefault, value);
		}
		private bool _isDefault;

		public BottomButtonWrapper()
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
			Title = title ?? throw new ArgumentNullException(nameof(title));
			if (string.IsNullOrWhiteSpace(Title))
			{
				throw new ArgumentException("Title for a button cannot be empty");
			}
			ToolTip = tip;
			ToolTipTitle = ToolTip is null ? null : (!string.IsNullOrWhiteSpace(tipTitle) ? tipTitle : Title);
			isDefault = defaultButton;
			onClick = callback;
		}

		public void UpdateButtonDisabled(string title, string tip = null, string tipTitle = null)
		{
			visibility = Visibility.Visible;
			Title = title ?? throw new ArgumentNullException(nameof(title));
			if (string.IsNullOrWhiteSpace(Title))
			{
				throw new ArgumentException("Title for a button cannot be empty");
			}
			ToolTip = tip;
			ToolTipTitle = ToolTip is null ? null : (!string.IsNullOrWhiteSpace(tipTitle) ? tipTitle : Title);
			isDefault = false;
			onClick = null;
		}

		public void UpdateButtonHidden()
		{
			visibility = Visibility.Hidden;
			isDefault = false;
			onClick = null;
		}
	}
}
