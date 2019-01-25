using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CoC.UI
{
	//public class UiData : ObservableCollection<ButtonData>
	//{
	//	internal UiData() : base() {}
	//	internal UiData(List<ButtonData> list) : base(list) {}

	//	internal UiData(IEnumerable<ButtonData> collection) : base(collection) {}
	//}

	public sealed class ButtonData : ICommand
	{
		public readonly int index;
		public readonly string buttonText;
		public readonly string buttonHint;
		private readonly PlayerFunction callback;
		private readonly Controller controller;
		internal ButtonData(int ind, string buttonTxt, string buttonHnt, Controller cont, PlayerFunction function)
		{
			index = ind;
			buttonText = buttonTxt;
			buttonHint = buttonHnt;
			callback = function;
			controller = cont;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			controller.Call(callback);
		}
	}
}