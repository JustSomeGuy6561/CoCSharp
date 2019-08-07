using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.UI;

namespace CoCWinDesktop.CustomControls.SideBarModelViews
{
	public sealed class PrisonSideBarModelView : SideBarBase
	{
		public PrisonSideBarModelView(Controller controller) : base(controller)
		{
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void GetData(Controller controller)
		{
			throw new NotImplementedException();
		}

		internal override void ClearArrows()
		{
			throw new NotImplementedException();
		}
	}
}
