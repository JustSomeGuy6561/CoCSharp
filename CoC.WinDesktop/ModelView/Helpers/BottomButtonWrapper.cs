using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoCWinDesktop.ModelView.Helpers
{
	public sealed class BottomButtonWrapper : INotifyPropertyChanged
	{
		public ICommand ClickCommand { get; }

		public Visibility visibility
		{
			get => _visibility;
			private set => IHateYouEnumBoat(ref _visibility, value);
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
			private set => IHateYouBoat(ref _title, value);
		}
		private string _title;

		public string ToolTip
		{
			get => _toolTip;
			private set => IHateYouBoat(ref _toolTip, value);
		}
		private string _toolTip;

		public string ToolTipTitle
		{
			get => _toolTipTitle;
			private set => IHateYouBoat(ref _toolTipTitle, value);
		}
		private string _toolTipTitle;

		public bool isDefault
		{
			get => _isDefault;
			private set => IHateYouPrimitiveBoat(ref _isDefault, value);
		}
		private bool _isDefault;


		public event PropertyChangedEventHandler PropertyChanged;



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

		private void IHateYouBoat<T>(ref T value, T newValue, [CallerMemberName] string propertyName = "") where T : class
		{
			if (value != newValue)
			{
				value = newValue;
				OnPropertyChanged(propertyName);
			}
		}

		private void IHateYouPrimitiveBoat<T>(ref T value, T newValue, [CallerMemberName] string propertyName = "") where T : struct, IEquatable<T>
		{
			if (!value.Equals(newValue))
			{
				value = newValue;
				OnPropertyChanged(propertyName);
			}
		}

		private void IHateYouEnumBoat<T>(ref T value, T newValue, [CallerMemberName] string propertyName = "") where T : Enum
		{
			if (!value.Equals(newValue))
			{
				value = newValue;
				OnPropertyChanged(propertyName);
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
