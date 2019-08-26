using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Frontend.UI.ControllerData;
using CoC.UI;

namespace CoCWinDesktop.CustomControls.SideBarModelViews
{
	public sealed class EnemySideBarModelView : SideBarBase
	{
		public EnemySideBarModelView(StatDataCollectionBase stats) : base(stats)
		{
		}

		protected override void GetData(StatDataCollectionBase statData)
		{
			throw new NotImplementedException();
		}

		internal override void ClearArrows()
		{
			throw new NotImplementedException();
		}
	}
}
