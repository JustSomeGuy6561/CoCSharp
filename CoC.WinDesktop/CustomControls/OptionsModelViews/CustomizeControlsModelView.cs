using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed class CustomizeControlsModelView : OptionModelViewDataBase
	{
		public CustomizeControlsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
		}

		public override void ParseDataForDisplay()
		{
		}

		public override Action OnConfirmation => ConfirmControls;

		private void ConfirmControls()
		{
			Console.WriteLine("Controls written? NYI, but code works!");
		}
	}
}
