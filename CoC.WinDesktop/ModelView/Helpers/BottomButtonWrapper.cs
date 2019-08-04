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
		public ICommand clickCommand { get; }

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
					if (_onClick is null != value is null)
					{
						((RelayCommand)clickCommand).RaiseExecuteChanged();
					}
					_onClick = value;
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

		public event PropertyChangedEventHandler PropertyChanged;

		public BottomButtonWrapper()
		{
			clickCommand = new RelayCommand(ClickCommand, CanClick);
		}

		private void ClickCommand()
		{
			onClick?.Invoke();
		}

		private bool CanClick()
		{
			return onClick != null;
		}

		public void UpdateButtonEnabled(Action callback, string title, string tip = null, string tipTitle = null)
		{
			visibility = Visibility.Visible;
			Title = title ?? throw new ArgumentNullException(nameof(title));
			if (string.IsNullOrWhiteSpace(Title))
			{
				throw new ArgumentException("Title for a button cannot be empty");
			}
			ToolTip = tip;
			ToolTipTitle = ToolTip is null ? null : (!string.IsNullOrWhiteSpace(tipTitle) ? tipTitle : Title);
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
			onClick = null;
		}

		public void UpdateButtonHidden()
		{
			visibility = Visibility.Hidden;
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
