using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed class InterfaceOptionsModelView : OptionModelViewDataBase
	{
		//display options was so conviluted it's getting its own view, but the only way to see it is via a button in interface options. 


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
		}

		public override void ParseDataForDisplay()
		{

		}
	}
}
