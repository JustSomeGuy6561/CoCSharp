using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.ModelView
{

	//this is a helper - i could do it all in standard, but that gets a bit conviluted. 
	public sealed class DataModelView : ModelViewBase
	{
		public DataModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void ParseDataForDisplay()
		{
			throw new NotImplementedException();
		}

		protected override bool SwitchToThisModelView(Action lastAction)
		{
			throw new NotImplementedException();
		}
	}
}
