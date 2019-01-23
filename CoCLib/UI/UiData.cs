using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CoC.UI
{
	public class UiData : ObservableCollection<ButtonData>
	{
		public UiData() : base() {}
		public UiData(List<ButtonData> list) : base(list) {}

		public UiData(IEnumerable<ButtonData> collection) : base(collection) {}
	}

	public class ButtonData : ICommand
	{
		public readonly int index;
		public readonly string buttonText;
		public readonly string buttonHint;
		private readonly PlayerFunction callback;

		public ButtonData(int ind, string buttonTxt, string buttonHnt, PlayerFunction function)
		{
			index = ind;
			buttonText = buttonTxt;
			buttonHint = buttonHnt;
			callback = function;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			Controller.Call(callback);
		}
	}
}