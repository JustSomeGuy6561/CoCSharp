using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoCWinDesktop.ModelView;

namespace CoCWinDesktop.CustomControls.ExtraItemModelViews
{
	class MenuInstructionsModelView : ExtraItemModelViewBase
	{
		public MenuInstructionsModelView(ModelViewRunner modelViewRunner, ExtraMenuItemsModelView parentModelView) : base(modelViewRunner, parentModelView)
		{
		}

		internal override void ParseDataForDisplay()
		{
			throw new NotImplementedException();
		}
	}
}
