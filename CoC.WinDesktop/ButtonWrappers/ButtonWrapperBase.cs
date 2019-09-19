using CoC.WinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoC.WinDesktop.ContentWrappers.ButtonWrappers
{
	/// <summary>
	/// Wrapper class for buttons in general - it's not necessary, but i've found it's easier to just create one of these and bind a button's data context
	/// to it, and use the corresponding style on the button that binds the values correctly.
	/// </summary>
	public abstract class ButtonWrapperBase : NotifierBase
	{
		public abstract string Title { get; }

		public abstract ToolTipWrapper ToolTip { get; }

		public abstract ICommand ClickCommand { get; }

		public abstract bool IsDefault { get; }

		public Visibility visibility
		{
			get => _visibility;
			protected set => CheckEnumPropertyChanged(ref _visibility, value);
		}
		private Visibility _visibility = Visibility.Hidden;
	}
}
