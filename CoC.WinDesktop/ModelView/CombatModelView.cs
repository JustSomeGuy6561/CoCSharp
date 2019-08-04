using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.UI;

namespace CoCWinDesktop.ModelView
{
	public sealed class CombatModelView : ModelViewBase
	{
		public CombatModelView(ModelViewRunner runner) : base(runner) {}

		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void ParseDataForDisplay()
		{
			
		}

		protected override bool SwitchToThisModelView(Action lastAction)
		{
			throw new NotImplementedException();
		}


	}
}
