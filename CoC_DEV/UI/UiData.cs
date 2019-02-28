//UiData.cs
//Description:
//Author: JustSomeGuy
//1/21/2019, 10:49 PM
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
		private readonly ModelView controller;
		internal ButtonData(int ind, string buttonTxt, string buttonHnt, ModelView cont, PlayerFunction function)
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