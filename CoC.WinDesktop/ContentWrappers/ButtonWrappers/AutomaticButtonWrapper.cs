using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoCWinDesktop.ContentWrappers.ButtonWrappers
{
	public class AutomaticButtonWrapper : ButtonWrapperBase
	{
		public override string Title => TitleStrFn();
		protected readonly SimpleDescriptor TitleStrFn;

		public override ToolTipWrapper ToolTip => _ToolTip;
		protected ToolTipWrapper _ToolTip;

		protected readonly SimpleDescriptor ToolTipTitleFn;
		protected readonly DescriptorWithArg<bool> ToolTipFn;

		public override ICommand ClickCommand => _command;
		protected RelayCommand _command;
		private bool allowCommand
		{
			get => _allowCommand;
			set
			{
				bool oldValue = _allowCommand;
				_allowCommand = value;

				if (oldValue != value)
				{
					(ClickCommand as RelayCommand).RaiseExecuteChanged();
				}
			}
		}
		private bool _allowCommand;

		public override bool IsDefault { get; }

		public AutomaticButtonWrapper(SimpleDescriptor TitleStrCallback, Action onClick, SimpleDescriptor tipCallback, SimpleDescriptor tipTitleCallback, bool enabled = true, bool defaultButton = false)
		{
			visibility = Visibility.Visible;

			IsDefault = defaultButton;

			_allowCommand = enabled;

			if (onClick is null) throw new ArgumentNullException(nameof(onClick));
			_command = new RelayCommand(onClick, () => allowCommand);

			TitleStrFn = TitleStrCallback ?? throw new ArgumentNullException(nameof(TitleStrCallback));
			if (string.IsNullOrWhiteSpace(TitleStrFn())) throw new ArgumentException("TitleStrCallback cannot be null or empty");


			ToolTipFn = string.IsNullOrWhiteSpace(tipCallback?.Invoke()) ? (DescriptorWithArg<bool>)null : x => tipCallback();
			ToolTipTitleFn = tipTitleCallback;

			_ToolTip = GenerateToolTipWrapper();
		}

		public AutomaticButtonWrapper(SimpleDescriptor TitleStrCallback, Action onClick, DescriptorWithArg<bool> unlockedLockedTipCallback, SimpleDescriptor tipTitleCallback, bool enabled = true, bool defaultButton = false)
		{
			visibility = Visibility.Visible;

			IsDefault = defaultButton;

			_allowCommand = enabled;

			if (onClick is null) throw new ArgumentNullException(nameof(onClick));
			_command = new RelayCommand(onClick, () => allowCommand);

			TitleStrFn = TitleStrCallback ?? throw new ArgumentNullException(nameof(TitleStrCallback));
			if (string.IsNullOrWhiteSpace(TitleStrFn())) throw new ArgumentException("TitleStrCallback cannot be null or empty");

			ToolTipFn = unlockedLockedTipCallback;
			ToolTipTitleFn = tipTitleCallback;

			_ToolTip = GenerateToolTipWrapper();
		}


		protected ToolTipWrapper GenerateToolTipWrapper()
		{
			if (string.IsNullOrWhiteSpace(ToolTipFn?.Invoke(allowCommand)))
			{
				return null;
			}
			else
			{
				string Title = ToolTipTitleFn?.Invoke();
				if (string.IsNullOrWhiteSpace(Title))
				{
					Title = TitleStrFn();
				}

				return new ToolTipWrapper(Title, ToolTipFn(allowCommand));
			}
		}

		public void SetVisibility(Visibility newVisibility)
		{
			visibility = newVisibility;
		}

		public void SetEnabled(bool enabled)
		{
			allowCommand = enabled;
		}
	}
}
