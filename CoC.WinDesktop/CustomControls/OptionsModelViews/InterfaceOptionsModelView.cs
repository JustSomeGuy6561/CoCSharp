using CoC.Backend;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using CoCWinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed class InterfaceOptionsModelView : OptionModelViewDataBase
	{
		#region Interface Options



		#endregion


		//display options was so conviluted it's getting its own view, but the only way to see it is via a button in interface options. 

		public string DisplayOptionsTitleText
		{
			get => _displayOptionsTitleText;
			private set => CheckPropertyChanged(ref _displayOptionsTitleText, value);
		}
		private string _displayOptionsTitleText;
		public string DisplayOptionsHelperText
		{
			get => _displayOptionsHelperText;
			private set => CheckPropertyChanged(ref _displayOptionsHelperText, value);
		}
		private string _displayOptionsHelperText;
		public string DisplayOptionsBtnText
		{
			get => _displayOptionsBtnText;
			private set => CheckPropertyChanged(ref _displayOptionsBtnText, value);
		}
		private string _displayOptionsBtnText;

		private readonly SimpleDescriptor displayOptionTextSource;
		private readonly SimpleDescriptor displayOptionTitleSource;
		private readonly SimpleDescriptor displayOptionHelperSource;

		public ICommand OnDisplayButton { get; }

		public string InterfaceOptionsText
		{
			get => _InterfaceOptionsText;
			private set => CheckPropertyChanged(ref _InterfaceOptionsText, value);
		}
		private string _InterfaceOptionsText;

		public string InterfaceOptionsHelper
		{
			get => _InterfaceOptionsHelper;
			private set => CheckPropertyChanged(ref _InterfaceOptionsHelper, value);
		}
		private string _InterfaceOptionsHelper;

		public ObservableCollection<OptionsRowBase> interfaceOptions { get; }


		public InterfaceOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			displayOptionTextSource = InterfaceStrings.AdjustDisplayOptionsText;
			displayOptionTitleSource = InterfaceStrings.DisplayOptionsTitleText;
			displayOptionHelperSource = InterfaceStrings.DisplayOptionsHelperText;
			_displayOptionsBtnText = displayOptionTextSource();
			_displayOptionsTitleText = displayOptionTitleSource();
			_displayOptionsHelperText = displayOptionHelperSource();
			OnDisplayButton = parent.OnDisplayHandle;

			interfaceOptions = new ObservableCollection<OptionsRowBase>()
			{

			};
		}

		public override void ParseDataForDisplay()
		{
			DisplayOptionsBtnText = displayOptionTextSource();
			DisplayOptionsTitleText = displayOptionTitleSource();
			DisplayOptionsHelperText = displayOptionHelperSource();
		}
	}
}
