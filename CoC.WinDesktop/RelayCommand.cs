using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoC.WinDesktop
{
	public class RelayCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private readonly Action callback;
		private readonly Func<bool> _canExecute;

		public RelayCommand(Action callbackAction, Func<bool> allowCallback)
		{
			callback = callbackAction ?? throw new ArgumentNullException(nameof(callbackAction));
			_canExecute = allowCallback ?? throw new ArgumentNullException(nameof(allowCallback));
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute();
		}

		public void Execute(object parameter)
		{
			callback();
		}

		public void RaiseExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
