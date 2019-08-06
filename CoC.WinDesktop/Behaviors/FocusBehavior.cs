using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoCWinDesktop.Behaviors
{
	public static class FocusBehavior
	{
		public static readonly DependencyProperty FocusFirstProperty =
			DependencyProperty.RegisterAttached(
				"FocusFirst",
				typeof(bool),
				typeof(FocusBehavior),
				new PropertyMetadata(false, OnFocusFirstPropertyChanged));

		public static bool GetFocusFirst(Control control)
		{
			return (bool)control.GetValue(FocusFirstProperty);
		}

		public static void SetFocusFirst(Control control, bool value)
		{
			control.SetValue(FocusFirstProperty, value);
		}

		static void OnFocusFirstPropertyChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (!(obj is Control control) || !(args.NewValue is bool argBool))
			{
				return;
			}

			else if (argBool)
			{
				control.Loaded += (sender, e) =>
					control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
		}
	}

}
